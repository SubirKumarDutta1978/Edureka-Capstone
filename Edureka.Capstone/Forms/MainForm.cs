using Edureka.Capstone.Configuration;
using Edureka.Capstone.Forms.Controls;
using Edureka.Capstone.Models;
using Edureka.Capstone.Services.Ingestion;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edureka.Capstone.Forms;

public partial class MainForm : Form
{
    private const int MaxPastedImages = 5;

    private readonly ILogger<MainForm> _logger;
    private readonly AppSettings _settings;
    private readonly IDocumentIngestionService _ingestion;

    private readonly List<DocumentItem> _documents = new();
    private readonly List<PastedImage> _pendingImages = new();
    private readonly ToolTip _toolTip = new();
    private readonly CancellationTokenSource _cts = new();

    private Font? _boldFont;
    private Font? _italicFont;
    private int _imageCounter;

    public MainForm(
        ILogger<MainForm> logger
        , IOptions<AppSettings> options
        , IDocumentIngestionService ingestion
        )
    {
        _logger = logger;
        _settings = options.Value;
        _ingestion = ingestion;

        InitializeComponent();

        txtPrompt.ImagePasted += TxtPrompt_ImagePasted;
        lvDocuments.SizeChanged += (_, _) => ResizeDocumentColumns();
        FormClosed += MainForm_FormClosed;
    }

    // ------------------------------------------------------------------ lifecycle

    private void MainForm_Load(object? sender, EventArgs e)
    {
        Text = _settings.ApplicationName;
        lblTitle.Text = _settings.ApplicationName;

        var uploadPath = Path.Combine(AppContext.BaseDirectory, _settings.UploadFolder);
        Directory.CreateDirectory(uploadPath);

        _boldFont = new Font(rtbConversation.Font, FontStyle.Bold);
        _italicFont = new Font(rtbConversation.Font, FontStyle.Italic);

        lblDropHint.Text =
            "Drag and drop files here, or click \"Add files...\"\r\n" +
            $"Supported: {string.Join(", ", _settings.SupportedExtensions)} (max {_settings.MaxFileSizeMb} MB each)";

        ResizeDocumentColumns();
        UpdateDocumentUi();
        UpdateSendState();

        AppendMessage(
            "System",
            "Add your documents on the left - their text is extracted automatically. " +
            "Select a ready document and click Preview to see what was extracted. " +
            "The AI backend is not connected yet.",
            Color.DimGray,
            italicBody: true);

        lblStatus.Text = "Ready";
        _logger.LogInformation("Application started. Upload folder: {UploadPath}", uploadPath);
    }

    private void MainForm_FormClosed(object? sender, FormClosedEventArgs e)
    {
        _cts.Cancel();

        foreach (var document in _documents)
        {
            _ingestion.Discard(document);
        }

        foreach (var image in _pendingImages)
        {
            image.Dispose();
        }
        _pendingImages.Clear();

        _boldFont?.Dispose();
        _italicFont?.Dispose();
        _toolTip.Dispose();
    }

    // ------------------------------------------------------------------ documents

    private async void btnAddFiles_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Select documents",
            Multiselect = true,
            CheckFileExists = true,
            Filter = BuildFileFilter()
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            await AddFilesAsync(dialog.FileNames);
        }
    }

    private void btnPreview_Click(object? sender, EventArgs e) => ShowPreview();

    private void lvDocuments_DoubleClick(object? sender, EventArgs e) => ShowPreview();

    private void btnRemoveFile_Click(object? sender, EventArgs e) => RemoveSelectedDocuments();

    private void btnClearFiles_Click(object? sender, EventArgs e)
    {
        if (_documents.Count == 0)
        {
            return;
        }

        var answer = MessageBox.Show(
            this,
            $"Remove all {_documents.Count} document(s) from the list?\r\n(Your original files are not touched.)",
            "Clear documents",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (answer == DialogResult.Yes)
        {
            foreach (var document in _documents)
            {
                _ingestion.Discard(document);
            }
            _documents.Clear();
            lvDocuments.Items.Clear();
            UpdateDocumentUi();
            lblStatus.Text = "Document list cleared";
        }
    }

    private void lvDocuments_SelectedIndexChanged(object? sender, EventArgs e) => UpdateDocumentUi();

    private void lvDocuments_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete)
        {
            RemoveSelectedDocuments();
            e.Handled = true;
        }
    }

    private void Documents_DragEnter(object? sender, DragEventArgs e)
    {
        e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) == true
            ? DragDropEffects.Copy
            : DragDropEffects.None;
    }

    private async void Documents_DragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] paths)
        {
            await AddFilesAsync(paths);
        }
    }

    private string BuildFileFilter()
    {
        var all = string.Join(";", _settings.SupportedExtensions.Select(x => "*" + x));
        return $"Supported documents|{all}" +
               "|PDF (*.pdf)|*.pdf" +
               "|Text (*.txt)|*.txt" +
               "|CSV (*.csv)|*.csv" +
               "|Excel (*.xlsx;*.xls)|*.xlsx;*.xls";
    }

    private async Task AddFilesAsync(IEnumerable<string> paths)
    {
        var newDocuments = ValidateAndAdd(paths);
        await IngestAsync(newDocuments);
    }

    /// <summary>Validates the files and adds them to the list as "Queued". Returns the new documents.</summary>
    private List<DocumentItem> ValidateAndAdd(IEnumerable<string> paths)
    {
        var added = new List<DocumentItem>();
        var skipped = new List<string>();
        var maxBytes = _settings.MaxFileSizeMb * 1024L * 1024L;

        foreach (var path in paths)
        {
            var name = Path.GetFileName(path);
            try
            {
                if (Directory.Exists(path))
                {
                    skipped.Add($"{name} - folders are not supported");
                    continue;
                }

                if (!File.Exists(path))
                {
                    skipped.Add($"{name} - file not found");
                    continue;
                }

                var extension = Path.GetExtension(path);
                if (!_settings.SupportedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                {
                    skipped.Add($"{name} - unsupported file type");
                    continue;
                }

                var info = new FileInfo(path);
                if (info.Length == 0)
                {
                    skipped.Add($"{name} - file is empty");
                    continue;
                }

                if (info.Length > maxBytes)
                {
                    skipped.Add($"{name} - larger than {_settings.MaxFileSizeMb} MB");
                    continue;
                }

                if (_documents.Any(d => string.Equals(d.FilePath, info.FullName, StringComparison.OrdinalIgnoreCase)))
                {
                    skipped.Add($"{name} - already added");
                    continue;
                }

                var document = DocumentItem.FromFile(info);
                _documents.Add(document);
                added.Add(document);

                var item = new ListViewItem(document.FileName)
                {
                    Tag = document,
                    UseItemStyleForSubItems = false
                };
                item.SubItems.Add(document.TypeLabel);
                item.SubItems.Add(document.SizeLabel);
                item.SubItems.Add(string.Empty);
                lvDocuments.Items.Add(item);
                UpdateListItem(document);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not add file {Path}", path);
                skipped.Add($"{name} - {ex.Message}");
            }
        }

        UpdateDocumentUi();

        if (skipped.Count > 0)
        {
            MessageBox.Show(
                this,
                "Some files were skipped:\r\n\r\n" + string.Join("\r\n", skipped),
                "Files skipped",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        return added;
    }

    /// <summary>Processes the documents one after another and refreshes the list as each one finishes.</summary>
    private async Task IngestAsync(List<DocumentItem> documents)
    {
        foreach (var document in documents)
        {
            if (IsDisposed || _cts.IsCancellationRequested)
            {
                return;
            }

            if (!_documents.Contains(document))
            {
                continue; // removed while waiting in the queue
            }

            document.Status = DocumentStatus.Processing;
            document.StatusMessage = "Processing...";
            UpdateListItem(document);
            lblStatus.Text = $"Processing {document.FileName}...";

            try
            {
                await _ingestion.IngestAsync(document, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                return; // form is closing
            }

            if (IsDisposed)
            {
                return;
            }

            if (!_documents.Contains(document))
            {
                _ingestion.Discard(document); // removed while it was being processed
                continue;
            }

            UpdateListItem(document);

            if (document.Status == DocumentStatus.Failed)
            {
                lblStatus.Text = $"Failed: {document.FileName}";
                AppendMessage(
                    "System",
                    $"Could not process \"{document.FileName}\": {document.StatusMessage}",
                    Color.Firebrick);
            }
            else
            {
                lblStatus.Text = $"Processed {document.FileName} ({document.StatusMessage})";
            }
        }
    }

    private void ShowPreview()
    {
        if (lvDocuments.SelectedItems.Count != 1)
        {
            return;
        }

        if (lvDocuments.SelectedItems[0].Tag is DocumentItem { Status: DocumentStatus.Ready } document)
        {
            using var preview = new DocumentPreviewForm(document);
            preview.ShowDialog(this);
        }
    }

    private void RemoveSelectedDocuments()
    {
        if (lvDocuments.SelectedItems.Count == 0)
        {
            return;
        }

        foreach (var item in lvDocuments.SelectedItems.Cast<ListViewItem>().ToList())
        {
            if (item.Tag is DocumentItem document)
            {
                _documents.Remove(document);
                _ingestion.Discard(document);
            }
            lvDocuments.Items.Remove(item);
        }

        UpdateDocumentUi();
        lblStatus.Text = "Document removed";
    }

    private void UpdateListItem(DocumentItem document)
    {
        var item = lvDocuments.Items
            .Cast<ListViewItem>()
            .FirstOrDefault(i => ReferenceEquals(i.Tag, document));

        if (item is null)
        {
            return;
        }

        var status = item.SubItems[3];
        status.Text = document.StatusLabel;
        status.ForeColor = document.Status switch
        {
            DocumentStatus.Ready => Color.SeaGreen,
            DocumentStatus.Failed => Color.Firebrick,
            DocumentStatus.Processing => Color.DarkOrange,
            _ => Color.Gray
        };
        item.ToolTipText = $"{document.FilePath}\r\n{document.StatusMessage}";

        UpdateDocumentUi();
    }

    private void UpdateDocumentUi()
    {
        var ready = _documents.Count(d => d.Status == DocumentStatus.Ready);
        var totalBytes = _documents.Sum(d => d.SizeBytes);

        lblDocSummary.Text = _documents.Count == 0
            ? "No documents"
            : $"{_documents.Count} document(s), {DocumentItem.FormatSize(totalBytes)} - {ready} ready";

        btnRemoveFile.Enabled = lvDocuments.SelectedItems.Count > 0;
        btnClearFiles.Enabled = _documents.Count > 0;
        btnPreview.Enabled =
            lvDocuments.SelectedItems.Count == 1 &&
            lvDocuments.SelectedItems[0].Tag is DocumentItem { Status: DocumentStatus.Ready };
    }

    private void ResizeDocumentColumns()
    {
        var available = lvDocuments.ClientSize.Width - colType.Width - colSize.Width - colStatus.Width;
        if (available > 80)
        {
            colName.Width = available;
        }
    }

    // ------------------------------------------------------------------ prompt input

    private void txtPrompt_TextChanged(object? sender, EventArgs e) => UpdateSendState();

    private void txtPrompt_KeyDown(object? sender, KeyEventArgs e)
    {
        // Enter = new line, Ctrl+Enter = send
        if (e.Control && e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
            SubmitPrompt();
        }
    }

    private void btnSend_Click(object? sender, EventArgs e) => SubmitPrompt();

    private void UpdateSendState() => btnSend.Enabled = !string.IsNullOrWhiteSpace(txtPrompt.Text);

    private void TxtPrompt_ImagePasted(object? sender, ImagePastedEventArgs e)
    {
        if (_pendingImages.Count >= MaxPastedImages)
        {
            e.Image.Dispose();
            MessageBox.Show(
                this,
                $"You can attach up to {MaxPastedImages} images per question.",
                "Too many images",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        _imageCounter++;
        var pasted = new PastedImage($"Pasted image {_imageCounter}", e.Image);
        _pendingImages.Add(pasted);
        AddThumbnail(pasted);

        flpAttachments.Visible = true;
        lblStatus.Text = $"{pasted.Name} attached ({pasted.Dimensions})";
    }

    private void AddThumbnail(PastedImage pasted)
    {
        var host = new Panel
        {
            Size = new Size(84, 76),
            Margin = new Padding(0, 0, 8, 0),
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.White
        };

        var picture = new PictureBox
        {
            Dock = DockStyle.Fill,
            Image = pasted.Bitmap,
            SizeMode = PictureBoxSizeMode.Zoom
        };

        var remove = new Button
        {
            Text = "x",
            Size = new Size(20, 20),
            Location = new Point(host.Width - 24, 1),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            Cursor = Cursors.Hand,
            TabStop = false
        };
        remove.FlatAppearance.BorderColor = Color.Silver;
        remove.Click += (_, _) => RemoveThumbnail(host, pasted);

        host.Controls.Add(picture);
        host.Controls.Add(remove);
        remove.BringToFront();

        _toolTip.SetToolTip(picture, $"{pasted.Name} ({pasted.Dimensions})");
        flpAttachments.Controls.Add(host);
    }

    private void RemoveThumbnail(Panel host, PastedImage pasted)
    {
        flpAttachments.Controls.Remove(host);
        host.Dispose();
        _pendingImages.Remove(pasted);
        pasted.Dispose();
        flpAttachments.Visible = _pendingImages.Count > 0;
    }

    private void ClearThumbnailsWithoutDisposingImages()
    {
        foreach (var control in flpAttachments.Controls.Cast<Control>().ToList())
        {
            flpAttachments.Controls.Remove(control);
            control.Dispose(); // PictureBox.Dispose does not dispose its Image
        }
        _pendingImages.Clear();
        flpAttachments.Visible = false;
    }

    private void SubmitPrompt()
    {
        var text = txtPrompt.Text.Trim();
        if (text.Length == 0)
        {
            return;
        }

        var prompt = new UserPrompt(text, _documents.ToList(), _pendingImages.ToList());

        txtPrompt.Clear();
        ClearThumbnailsWithoutDisposingImages();
        txtPrompt.Focus();

        try
        {
            OnPromptSubmitted(prompt);
        }
        finally
        {
            prompt.Dispose();
        }
    }

    /// <summary>
    /// Hook for later steps. For now the UI echoes the prompt and shows a placeholder reply.
    /// </summary>
    private void OnPromptSubmitted(UserPrompt prompt)
    {
        var ready = prompt.Documents.Count(d => d.Status == DocumentStatus.Ready);

        _logger.LogInformation(
            "Prompt submitted ({Length} chars, {Ready}/{Docs} documents ready, {Images} images)",
            prompt.Text.Length, ready, prompt.Documents.Count, prompt.Images.Count);

        var context = $"Documents ready: {ready} of {prompt.Documents.Count}";
        if (prompt.Images.Count > 0)
        {
            context += $"  |  Images attached: {prompt.Images.Count}";
        }

        AppendMessage("You", prompt.Text, Color.FromArgb(31, 78, 121), footer: context);
        AppendMessage(
            "Assistant",
            "Your documents are ingested, but search and answering are not built yet. " +
            "That comes in the next steps (chunking, embeddings, retrieval and the LLM).",
            Color.SeaGreen,
            italicBody: true);

        lblStatus.Text = "Question submitted";
    }

    private void btnClearChat_Click(object? sender, EventArgs e)
    {
        rtbConversation.Clear();
        lblStatus.Text = "Conversation cleared";
    }

    // ------------------------------------------------------------------ transcript

    private void AppendMessage(
        string speaker,
        string body,
        Color speakerColor,
        bool italicBody = false,
        string? footer = null)
    {
        var box = rtbConversation;
        var regular = box.Font;

        void Write(string text, Font font, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionFont = font;
            box.SelectionColor = color;
            box.AppendText(text);
        }

        Write($"{speaker}  {DateTime.Now:HH:mm}\r\n", _boldFont ?? regular, speakerColor);
        Write(body + "\r\n", italicBody ? (_italicFont ?? regular) : regular,
              italicBody ? Color.DimGray : box.ForeColor);

        if (!string.IsNullOrEmpty(footer))
        {
            Write(footer + "\r\n", regular, Color.Gray);
        }

        Write("\r\n", regular, box.ForeColor);

        box.SelectionStart = box.TextLength;
        box.ScrollToCaret();
    }
}

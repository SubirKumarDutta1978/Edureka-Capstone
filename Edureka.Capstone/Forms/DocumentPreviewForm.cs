using Edureka.Capstone.Models;

namespace Edureka.Capstone.Forms;

/// <summary>
/// Read-only viewer for the text extracted from a document, so the user can verify ingestion.
/// </summary>
public sealed class DocumentPreviewForm : Form
{
    private const int MaxDisplayCharacters = 200_000;

    private readonly ListBox _sections = new();
    private readonly TextBox _text = new();
    private readonly Font _monoFont = new("Consolas", 9.5F);

    public DocumentPreviewForm(DocumentItem document)
    {
        var content = document.Content ?? throw new InvalidOperationException("Document has no content.");

        Text = $"Preview - {document.FileName}";
        Size = new Size(950, 620);
        MinimumSize = new Size(600, 400);
        StartPosition = FormStartPosition.CenterParent;

        var info = new Label
        {
            Dock = DockStyle.Top,
            Height = 30,
            Padding = new Padding(8, 0, 0, 0),
            TextAlign = ContentAlignment.MiddleLeft,
            Text = $"{content.Summary}  |  {content.CharacterCount:N0} characters extracted"
        };

        _sections.Dock = DockStyle.Left;
        _sections.Width = 200;
        _sections.IntegralHeight = false;
        _sections.DisplayMember = nameof(DocumentSection.Label);
        _sections.Items.AddRange(content.Sections.Cast<object>().ToArray());
        _sections.SelectedIndexChanged += (_, _) => ShowSelectedSection();

        _text.Dock = DockStyle.Fill;
        _text.Multiline = true;
        _text.ReadOnly = true;
        _text.WordWrap = true;
        _text.ScrollBars = ScrollBars.Vertical;
        _text.BackColor = Color.White;
        _text.Font = _monoFont;
        _text.MaxLength = int.MaxValue;

        // Fill first, then edge controls (WinForms docking order).
        Controls.Add(_text);
        Controls.Add(_sections);
        Controls.Add(info);

        if (_sections.Items.Count > 0)
        {
            _sections.SelectedIndex = 0;
        }
    }

    private void ShowSelectedSection()
    {
        if (_sections.SelectedItem is not DocumentSection section)
        {
            return;
        }

        var text = section.Text;
        var truncated = text.Length > MaxDisplayCharacters;
        if (truncated)
        {
            text = text[..MaxDisplayCharacters];
        }

        text = text.Replace("\r\n", "\n").Replace("\n", "\r\n");
        if (truncated)
        {
            text += "\r\n\r\n[... preview truncated ...]";
        }

        _text.Text = text;
        _text.SelectionStart = 0;
        _text.ScrollToCaret();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _monoFont.Dispose();
        base.OnFormClosed(e);
    }
}

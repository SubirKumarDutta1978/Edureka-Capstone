using System.Runtime.InteropServices;

namespace Edureka.Capstone.Forms.Controls;

public sealed class ImagePastedEventArgs : EventArgs
{
    public Bitmap Image { get; }

    public ImagePastedEventArgs(Bitmap image) => Image = image;
}

/// <summary>
/// Multiline prompt box. Text is always pasted as plain text (no rich formatting from Word / web),
/// and clipboard images are raised through <see cref="ImagePasted"/> so they can be shown as attachments.
/// </summary>
public class PromptRichTextBox : RichTextBox
{
    private const int WM_PASTE = 0x0302;

    public event EventHandler<ImagePastedEventArgs>? ImagePasted;

    public PromptRichTextBox()
    {
        DetectUrls = false;
        AcceptsTab = false;
        EnableAutoDragDrop = false;
        Multiline = true;
        ScrollBars = RichTextBoxScrollBars.Vertical;
    }

    protected override void WndProc(ref Message m)
    {
        // Covers context-menu paste and any other paste path.
        if (m.Msg == WM_PASTE && !DesignMode)
        {
            PasteFromClipboard();
            return;
        }
        base.WndProc(ref m);
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (!DesignMode && (keyData == (Keys.Control | Keys.V) || keyData == (Keys.Shift | Keys.Insert)))
        {
            PasteFromClipboard();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    public void PasteFromClipboard()
    {
        try
        {
            // Text wins: copying cells from Excel puts both text and a picture on the clipboard.
            if (Clipboard.ContainsText())
            {
                var text = Clipboard.GetText();
                if (!string.IsNullOrEmpty(text))
                {
                    SelectedText = text;
                }
                return;
            }

            if (Clipboard.ContainsImage())
            {
                using var image = Clipboard.GetImage();
                if (image is not null)
                {
                    ImagePasted?.Invoke(this, new ImagePastedEventArgs(new Bitmap(image)));
                }
            }
        }
        catch (ExternalException)
        {
            // Clipboard can be locked by another process; ignore and let the user retry.
        }
    }
}

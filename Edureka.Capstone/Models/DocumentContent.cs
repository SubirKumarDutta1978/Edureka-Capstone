namespace Edureka.Capstone.Models;

/// <summary>
/// One logical part of a document: a PDF page, an Excel sheet, a whole text file, ...
/// The label is kept so later steps can cite where an answer came from.
/// </summary>
public sealed record DocumentSection(string Label, string Text);

/// <summary>
/// The plain text extracted from a document (output of Step 3, input for chunking in Step 4).
/// </summary>
public sealed class DocumentContent
{
    public IReadOnlyList<DocumentSection> Sections { get; }
    public string Summary { get; }

    public DocumentContent(IReadOnlyList<DocumentSection> sections, string summary)
    {
        Sections = sections;
        Summary = summary;
    }

    public int CharacterCount => Sections.Sum(s => s.Text.Length);

    public string FullText =>
        string.Join("\n\n", Sections.Select(s => $"[{s.Label}]\n{s.Text}"));
}

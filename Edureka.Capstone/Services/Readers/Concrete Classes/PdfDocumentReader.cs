using Edureka.Capstone.Models;
using Edureka.Capstone.Services.Readers;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace Edureka.Capstone.Services.Readers.Derived;

public sealed class PdfDocumentReader : IDocumentReader
{
    public IReadOnlyCollection<string> SupportedExtensions { get; } = [".pdf"];

    public DocumentContent Read(string filePath, CancellationToken cancellationToken)
    {
        using var pdf = PdfDocument.Open(filePath);
        var sections = new List<DocumentSection>();

        foreach (var page in pdf.GetPages())
        {
            cancellationToken.ThrowIfCancellationRequested();

            var text = ContentOrderTextExtractor.GetText(page).Trim();
            if (text.Length > 0)
            {
                sections.Add(new DocumentSection($"Page {page.Number}", text));
            }
        }

        return new DocumentContent(sections, $"{pdf.NumberOfPages} page(s)");
    }
}

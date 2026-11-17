using System.Text;
using Edureka.Capstone.Models;
using Edureka.Capstone.Services.Readers;

namespace Edureka.Capstone.Services.Readers.Derived;

public sealed class TextDocumentReader : IDocumentReader
{
    public IReadOnlyCollection<string> SupportedExtensions { get; } = [".txt"];

    public DocumentContent Read(string filePath, CancellationToken cancellationToken)
    {
        // Honors a BOM (UTF-8/UTF-16); otherwise assumes UTF-8.
        using var reader = new StreamReader(filePath, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        var text = reader.ReadToEnd().Replace("\r\n", "\n").Trim();
        cancellationToken.ThrowIfCancellationRequested();

        var lines = text.Length == 0 ? 0 : text.Count(c => c == '\n') + 1;
        var sections = new List<DocumentSection> { new("Text", text) };
        return new DocumentContent(sections, $"{lines:N0} lines");
    }
}

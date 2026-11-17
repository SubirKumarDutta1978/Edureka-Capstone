using System.Text;
using Edureka.Capstone.Models;
using Edureka.Capstone.Services.Readers;
using Edureka.Capstone.Services.Utilities;
using ExcelDataReader;

namespace Edureka.Capstone.Services.Readers.Derived;

public sealed class CsvDocumentReader : IDocumentReader
{
    static CsvDocumentReader()
    {
        // Needed by ExcelDataReader for legacy code pages.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public IReadOnlyCollection<string> SupportedExtensions { get; } = [".csv"];

    public DocumentContent Read(string filePath, CancellationToken cancellationToken)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = ExcelReaderFactory.CreateCsvReader(
            stream,
            new ExcelReaderConfiguration { FallbackEncoding = Encoding.UTF8 });

        var (text, rows) = SpreadsheetTextBuilder.Build(reader, cancellationToken);

        var sections = new List<DocumentSection>();
        if (rows > 0)
        {
            sections.Add(new DocumentSection("Data", text));
        }
        return new DocumentContent(sections, $"{rows:N0} rows");
    }
}

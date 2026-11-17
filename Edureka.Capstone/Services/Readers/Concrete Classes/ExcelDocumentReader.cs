using System.Text;
using Edureka.Capstone.Models;
using Edureka.Capstone.Services.Readers;
using Edureka.Capstone.Services.Utilities;
using ExcelDataReader;

namespace Edureka.Capstone.Services.Readers.Derived;

public sealed class ExcelDocumentReader : IDocumentReader
{
    static ExcelDocumentReader()
    {
        // Needed to read the legacy .xls format.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public IReadOnlyCollection<string> SupportedExtensions { get; } = [".xlsx", ".xls"];

    public DocumentContent Read(string filePath, CancellationToken cancellationToken)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = ExcelReaderFactory.CreateReader(stream); // detects .xls vs .xlsx

        var sections = new List<DocumentSection>();
        var totalRows = 0;

        do
        {
            cancellationToken.ThrowIfCancellationRequested();

            var (text, rows) = SpreadsheetTextBuilder.Build(reader, cancellationToken);
            if (rows > 0)
            {
                sections.Add(new DocumentSection($"Sheet: {reader.Name}", text));
                totalRows += rows;
            }
        }
        while (reader.NextResult());

        return new DocumentContent(sections, $"{sections.Count} sheet(s), {totalRows:N0} rows");
    }
}

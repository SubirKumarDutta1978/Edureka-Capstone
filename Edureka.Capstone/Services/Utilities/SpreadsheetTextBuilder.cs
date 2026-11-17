using System.Globalization;
using System.Text;
using ExcelDataReader;

namespace Edureka.Capstone.Services.Utilities;

/// <summary>
/// Turns the current sheet / CSV of an ExcelDataReader into text, one line per row:
///   Row 1: Name: Alice | Dept: Sales | Salary: 52000
/// The first non-empty row is treated as the header. Keeping the column name next to each value
/// makes the text understandable on its own when it is later split into chunks.
/// </summary>
internal static class SpreadsheetTextBuilder
{
    public static (string Text, int Rows) Build(IExcelDataReader reader, CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();
        string[]? headers = null;
        var rows = 0;

        while (reader.Read())
        {
            cancellationToken.ThrowIfCancellationRequested();

            var values = new string[reader.FieldCount];
            var hasData = false;
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = FormatCell(reader.GetValue(i));
                hasData |= values[i].Length > 0;
            }

            if (!hasData)
            {
                continue;
            }

            if (headers is null)
            {
                headers = values
                    .Select((v, i) => string.IsNullOrWhiteSpace(v) ? $"Column{i + 1}" : v)
                    .ToArray();
                continue;
            }

            var parts = new List<string>();
            for (var i = 0; i < values.Length; i++)
            {
                if (values[i].Length == 0)
                {
                    continue;
                }
                var header = i < headers.Length ? headers[i] : $"Column{i + 1}";
                parts.Add($"{header}: {values[i]}");
            }

            rows++;
            builder.Append("Row ").Append(rows).Append(": ").AppendLine(string.Join(" | ", parts));
        }

        return (builder.ToString().TrimEnd(), rows);
    }

    private static string FormatCell(object? value) => value switch
    {
        null => string.Empty,
        DBNull => string.Empty,
        DateTime dt => dt.TimeOfDay == TimeSpan.Zero
            ? dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            : dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
        double d => d.ToString("0.################", CultureInfo.InvariantCulture),
        float f => f.ToString("0.########", CultureInfo.InvariantCulture),
        bool b => b ? "TRUE" : "FALSE",
        _ => Convert.ToString(value, CultureInfo.InvariantCulture)?.Trim() ?? string.Empty
    };
}

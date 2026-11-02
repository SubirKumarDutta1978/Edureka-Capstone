namespace Edureka.Capstone.Models;

/// <summary>
/// A document the user has selected for questioning.
/// Step 2 only keeps a reference to the file; ingestion comes in a later step.
/// </summary>
public sealed class DocumentItem
{
    public Guid Id { get; } = Guid.NewGuid();
    public string FilePath { get; }
    public string FileName { get; }
    public string Extension { get; }
    public long SizeBytes { get; }
    public DateTime AddedAt { get; } = DateTime.Now;

    private DocumentItem(string filePath, string fileName, string extension, long sizeBytes)
    {
        FilePath = filePath;
        FileName = fileName;
        Extension = extension;
        SizeBytes = sizeBytes;
    }

    public static DocumentItem FromFile(FileInfo file) =>
        new(file.FullName, file.Name, file.Extension.ToLowerInvariant(), file.Length);

    public string TypeLabel => Extension switch
    {
        ".pdf" => "PDF",
        ".txt" => "Text",
        ".csv" => "CSV",
        ".xlsx" or ".xls" => "Excel",
        _ => Extension.TrimStart('.').ToUpperInvariant()
    };

    public string SizeLabel => FormatSize(SizeBytes);

    public static string FormatSize(long bytes)
    {
        string[] units = ["B", "KB", "MB", "GB"];
        double size = bytes;
        int unit = 0;
        while (size >= 1024 && unit < units.Length - 1)
        {
            size /= 1024;
            unit++;
        }
        return unit == 0 ? $"{size:0} {units[unit]}" : $"{size:0.#} {units[unit]}";
    }
}

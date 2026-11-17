namespace Edureka.Capstone.Models;

public enum DocumentStatus
{
    Queued,
    Processing,
    Ready,
    Failed
}

/// <summary>
/// A document the user has selected, plus the result of ingesting it.
/// </summary>
public sealed class DocumentItem
{
    public Guid Id { get; } = Guid.NewGuid();
    public string FilePath { get; }
    public string FileName { get; }
    public string Extension { get; }
    public long SizeBytes { get; }
    public DateTime AddedAt { get; } = DateTime.Now;

    // ---- filled in by the ingestion service (Step 3)
    public DocumentStatus Status { get; set; } = DocumentStatus.Queued;
    public string StatusMessage { get; set; } = "Queued";
    public string? StoredPath { get; set; }
    public DocumentContent? Content { get; set; }

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

    public string StatusLabel => Status switch
    {
        DocumentStatus.Queued => "Queued",
        DocumentStatus.Processing => "Processing...",
        DocumentStatus.Ready => $"Ready ({Content?.Summary})",
        DocumentStatus.Failed => "Failed",
        _ => string.Empty
    };

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

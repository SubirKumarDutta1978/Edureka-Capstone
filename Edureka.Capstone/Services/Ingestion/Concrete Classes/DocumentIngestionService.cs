using Edureka.Capstone.Configuration;
using Edureka.Capstone.Models;
using Edureka.Capstone.Services.Readers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edureka.Capstone.Services.Ingestion;

public sealed class DocumentIngestionService : IDocumentIngestionService
{
    private readonly IReadOnlyList<IDocumentReader> _readers;
    private readonly ILogger<DocumentIngestionService> _logger;
    private readonly string _uploadFolder;

    public DocumentIngestionService(
        IEnumerable<IDocumentReader> readers,
        IOptions<AppSettings> options,
        ILogger<DocumentIngestionService> logger)
    {
        _readers = readers.ToList();
        _logger = logger;
        _uploadFolder = Path.Combine(AppContext.BaseDirectory, options.Value.UploadFolder);
    }

    public async Task IngestAsync(DocumentItem document, CancellationToken cancellationToken = default)
    {
        document.Status = DocumentStatus.Processing;
        document.StatusMessage = "Processing...";

        try
        {
            var reader = _readers.FirstOrDefault(r =>
                    r.SupportedExtensions.Contains(document.Extension, StringComparer.OrdinalIgnoreCase))
                ?? throw new NotSupportedException($"No reader is available for '{document.Extension}' files.");

            // File work runs on a background thread so the UI stays responsive.
            var content = await Task.Run(() =>
            {
                var storedPath = CopyToUploadFolder(document);
                document.StoredPath = storedPath;
                return reader.Read(storedPath, cancellationToken);
            }, cancellationToken);

            if (content.CharacterCount == 0)
            {
                throw new InvalidDataException(
                    "No readable text was found. (Scanned PDFs / images need OCR, which is not supported.)");
            }

            document.Content = content;
            document.Status = DocumentStatus.Ready;
            document.StatusMessage = content.Summary;

            _logger.LogInformation(
                "Ingested {File}: {Summary}, {Chars} characters",
                document.FileName, content.Summary, content.CharacterCount);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to ingest {File}", document.FileName);
            document.Content = null;
            document.Status = DocumentStatus.Failed;
            document.StatusMessage = ex.Message;
        }
    }

    public void Discard(DocumentItem document)
    {
        var path = document.StoredPath;
        if (path is null)
        {
            return;
        }

        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            document.StoredPath = null;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            _logger.LogWarning(ex, "Could not delete stored copy {Path}", path);
        }
    }

    private string CopyToUploadFolder(DocumentItem document)
    {
        Directory.CreateDirectory(_uploadFolder);
        var destination = Path.Combine(_uploadFolder, $"{document.Id:N}{document.Extension}");

        // FileShare.ReadWrite lets us copy a file that is currently open in Excel / Word.
        using var source = new FileStream(document.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var target = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None);
        source.CopyTo(target);

        return destination;
    }
}

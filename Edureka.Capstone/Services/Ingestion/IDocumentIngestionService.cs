using Edureka.Capstone.Models;

namespace Edureka.Capstone.Services.Ingestion;

public interface IDocumentIngestionService
{
    /// <summary>
    /// Copies the file into the application's upload folder and extracts its text.
    /// Updates <see cref="DocumentItem.Status"/>, <see cref="DocumentItem.StatusMessage"/> and
    /// <see cref="DocumentItem.Content"/>. Reader errors are reported through the status (Failed),
    /// not thrown; only cancellation is thrown.
    /// </summary>
    Task IngestAsync(DocumentItem document, CancellationToken cancellationToken = default);

    /// <summary>Deletes the stored copy of the document (best effort).</summary>
    void Discard(DocumentItem document);
}

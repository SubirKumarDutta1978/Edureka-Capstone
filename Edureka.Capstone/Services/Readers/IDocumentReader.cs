using Edureka.Capstone.Models;

namespace Edureka.Capstone.Services.Readers;

/// <summary>
/// Extracts plain text from one family of file formats.
/// </summary>
public interface IDocumentReader
{
    /// <summary>Lower-case extensions including the dot, e.g. ".pdf".</summary>
    IReadOnlyCollection<string> SupportedExtensions { get; }

    DocumentContent Read(string filePath, CancellationToken cancellationToken);
}

namespace Edureka.Capstone.Models;

/// <summary>
/// Everything the user submitted in one turn: the question, the documents in scope
/// and any pasted images. This is what later steps (RAG / agents) will consume.
/// </summary>
public sealed class UserPrompt : IDisposable
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime SubmittedAt { get; } = DateTime.Now;
    public string Text { get; }
    public IReadOnlyList<DocumentItem> Documents { get; }
    public IReadOnlyList<PastedImage> Images { get; }

    public UserPrompt(string text, IReadOnlyList<DocumentItem> documents, IReadOnlyList<PastedImage> images)
    {
        Text = text;
        Documents = documents;
        Images = images;
    }

    public void Dispose()
    {
        foreach (var image in Images)
        {
            image.Dispose();
        }
    }
}

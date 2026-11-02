namespace Edureka.Capstone.Models;

/// <summary>
/// An image pasted from the clipboard and attached to a prompt.
/// </summary>
public sealed class PastedImage : IDisposable
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public Bitmap Bitmap { get; }

    public PastedImage(string name, Bitmap bitmap)
    {
        Name = name;
        Bitmap = bitmap;
    }

    public string Dimensions => $"{Bitmap.Width} x {Bitmap.Height}";

    public void Dispose() => Bitmap.Dispose();
}

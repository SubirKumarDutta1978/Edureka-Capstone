namespace Edureka.Capstone.Configuration;

/// <summary>
/// Strongly-typed mapping of the "Capstone" section in appsettings.json.
/// </summary>
public class AppSettings
{
    public static string SectionName = "Capstone"; //const
    public string ApplicationName { get; set; } = "Edureka Capstone";
    public string UploadFolder { get; set; } = "Data\\Uploads";
    public int MaxFileSizeMb { get; set; } = 50;
    public string[] SupportedExtensions { get; set; } = [".pdf", ".txt", ".csv", ".xlsx", ".xls"];

    public LlmSettings Llm { get; set; } = new();
    public EmbeddingSettings Embedding { get; set; } = new();
    public VectorStoreSettings VectorStore { get; set; } = new();
}

public class LlmSettings
{
    public string Provider { get; set; } = "OpenAI";
    public string Model { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}

public class EmbeddingSettings
{
    public string Provider { get; set; } = "OpenAI";
    public string Model { get; set; } = string.Empty;
}

public class VectorStoreSettings
{
    public string Provider { get; set; } = "InMemory";
    public string Path { get; set; } = "Data\\VectorStore";
}

using Edureka.Capstone.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edureka.Capstone.Forms;

public partial class MainForm : Form
{
    private readonly ILogger<MainForm> _logger;
    private readonly AppSettings _settings;

    public MainForm(ILogger<MainForm> logger, IOptions<AppSettings> options)
    {
        _logger = logger;
        _settings = options.Value;

        InitializeComponent();
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        Text = _settings.ApplicationName;
        lblTitle.Text = _settings.ApplicationName;

        var uploadPath = Path.Combine(AppContext.BaseDirectory, _settings.UploadFolder);
        Directory.CreateDirectory(uploadPath);

        lblStatus.Text = "Ready";
        _logger.LogInformation("Application started. Upload folder: {UploadPath}", uploadPath);
    }
}

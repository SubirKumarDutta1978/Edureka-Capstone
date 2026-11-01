using Edureka.Capstone.Configuration;
using Edureka.Capstone.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edureka.Capstone;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => ShowFatal(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) => ShowFatal(e.ExceptionObject as Exception);

        using var host = Host.CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices((context, services) =>
            {
                services.Configure<AppSettings>(
                    context.Configuration.GetSection(AppSettings.SectionName));

                // Forms
                services.AddTransient<MainForm>();

                // Future steps: register services and agents here
                // services.AddSingleton<IDocumentIngestionService, DocumentIngestionService>();
            })
            .Build();

        Application.Run(host.Services.GetRequiredService<MainForm>());
    }

    private static void ShowFatal(Exception? ex)
    {
        MessageBox.Show(
            ex?.Message ?? "An unknown error occurred.",
            "Unexpected error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }
}

using System.IO;
using Microsoft.Extensions.Configuration;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Seq.Input.Azure.EventHub.TestConsole;

public class AppSettings
{
    public AppSettings()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
        var configuration = builder.Build();
        configuration.Bind(nameof(AppSettings), this);
    }

    public string EventHubConnectionString { get; set; }
    public string EventHubName { get; set; }
}

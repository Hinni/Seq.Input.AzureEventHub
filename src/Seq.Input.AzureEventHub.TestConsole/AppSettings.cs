using Microsoft.Extensions.Configuration;
using System.IO;

namespace Seq.Input.AzureEventHub.TestConsole
{
    public class AppSettings
    {
        private readonly IConfigurationRoot _configuration;

        public AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
            _configuration.Bind(nameof(AppSettings), this);
        }

        public string EventHubConnectionString { get; set; }
        public string EventHubName { get; set; }
        public string ConsumerGroupName { get; set; }
        public string StorageConnectionString { get; set; }
        public string StorageContainerName { get; set; }
    }
}
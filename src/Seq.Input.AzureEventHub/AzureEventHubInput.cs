using Microsoft.Azure.EventHubs;
using Seq.Apps;
using System;
using System.IO;

namespace Seq.Input.AzureEventHub
{
    [SeqApp("Azure Event Hub Input",
    Description = "Pulls JSON-formatted events from an Azure Event Hub. For details of the " +
                  "supported JSON schema, see " +
                  "https://github.com/serilog/serilog-formatting-compact/#format-details.")]
    public class AzureEventHubInput : SeqApp, IPublishJson, IDisposable
    {
        private AzureEventHubListener azureEventHubListener;

        [SeqAppSetting(
            DisplayName = "Event Hubs connection string",
            IsOptional = false,
            HelpText = "")]
        public string EventHubConnectionString { get; set; }

        [SeqAppSetting(
            DisplayName = "Event Hub path/name",
            IsOptional = false,
            HelpText = "")]
        public string EventHubName { get; set; }

        [SeqAppSetting(
            DisplayName = "Comsumer group name",
            IsOptional = false,
            HelpText = "The used consumer group name. The default is `$Default`.")]
        public string ConsumerGroupName { get; set; } = PartitionReceiver.DefaultConsumerGroupName;

        [SeqAppSetting(
            DisplayName = "Storage account connection string",
            IsOptional = false,
            HelpText = "")]
        public string StorageConnectionString { get; set; }

        [SeqAppSetting(
            DisplayName = "Storage account container name",
            IsOptional = false,
            HelpText = "")]
        public string StorageContainerName { get; set; }

        public void Start(TextWriter inputWriter)
        {
            azureEventHubListener = new AzureEventHubListener(
                inputWriter, Log,
                EventHubConnectionString, EventHubName, ConsumerGroupName,
                StorageConnectionString, StorageContainerName);
        }

        public void Stop()
        {
            azureEventHubListener.Stop();
        }

        public void Dispose()
        {
            // NOOP
        }
    }
}
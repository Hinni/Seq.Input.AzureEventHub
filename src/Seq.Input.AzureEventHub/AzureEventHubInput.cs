using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
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
        private EventProcessorHost _eventProcessorHost;

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
            DisplayName = "Storage account container name",
            IsOptional = false,
            HelpText = "")]
        public string StorageContainerName { get; set; }

        [SeqAppSetting(
            DisplayName = "Storage account name",
            IsOptional = false,
            HelpText = "")]
        public string StorageAccountName { get; set; }

        [SeqAppSetting(
            DisplayName = "Storage account key",
            IsOptional = false,
            HelpText = "")]
        public string StorageAccountKey { get; set; }

        public void Start(TextWriter inputWriter)
        {
            string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, StorageAccountKey);

            Console.WriteLine("Registering EventProcessor...");

            _eventProcessorHost = new EventProcessorHost(
                EventHubName,
                ConsumerGroupName,
                EventHubConnectionString,
                storageConnectionString,
                StorageContainerName);

            // Registers the Event Processor Host and starts receiving messages
            var factory = new InputEventProcessorFactory<CompactFormatEventProcessor>(inputWriter);
            _eventProcessorHost.RegisterEventProcessorFactoryAsync(factory).Wait();
        }

        public void Stop()
        {
            // Disposes of the Event Processor Host
            _eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }

        public void Dispose()
        {
            // NOOP
        }
    }
}
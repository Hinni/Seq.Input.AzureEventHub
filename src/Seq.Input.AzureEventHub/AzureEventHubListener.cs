using Microsoft.Azure.EventHubs.Processor;
using Serilog;
using System.IO;

namespace Seq.Input.AzureEventHub
{
    public class AzureEventHubListener
    {
        private EventProcessorHost _eventProcessorHost;

        public AzureEventHubListener(TextWriter inputWriter, ILogger logger,
            string eventHubConnectionString, string eventHubName, string consumerGroupName,
            string storageConnectionString, string storageContainerName)
        {
            _eventProcessorHost = new EventProcessorHost(
                eventHubName,
                consumerGroupName,
                eventHubConnectionString,
                storageConnectionString,
                storageContainerName);

            // Registers the Event Processor Host and starts receiving messages
            var factory = new InputEventProcessorFactory<CompactFormatEventProcessor>(inputWriter, logger);
            _eventProcessorHost.RegisterEventProcessorFactoryAsync(factory).Wait();
        }

        public void Stop()
        {
            // Disposes of the Event Processor Host
            _eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
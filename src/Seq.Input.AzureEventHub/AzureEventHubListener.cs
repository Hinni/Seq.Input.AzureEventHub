using Microsoft.Azure.EventHubs.Processor;
using Serilog;
using System.IO;

namespace Seq.Input.AzureEventHub
{
    public class AzureEventHubListener
    {
        private readonly EventProcessorHost _eventProcessorHost;

        public AzureEventHubListener(TextWriter inputWriter, ILogger logger, bool verboseEnabled,
            string eventHubConnectionString, string eventHubName, string consumerGroupName,
            string storageConnectionString, string storageContainerName)
        {
            _eventProcessorHost = new EventProcessorHost(
                eventHubName,
                consumerGroupName,
                eventHubConnectionString,
                storageConnectionString,
                storageContainerName);

            // Registers ClefEventProcessor in running EventProcessorHost instance
            var factory = new InputEventProcessorFactory<ClefEventProcessor>(inputWriter, logger, verboseEnabled);
            _eventProcessorHost.RegisterEventProcessorFactoryAsync(factory);
        }

        public void Stop()
        {
            // Unregister all registered EventProcessors
            _eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
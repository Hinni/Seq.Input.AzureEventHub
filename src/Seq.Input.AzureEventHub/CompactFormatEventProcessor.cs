using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Seq.Input.AzureEventHub
{
    public class CompactFormatEventProcessor : IEventProcessor
    {
        private readonly TextWriter _inputWriter;
        private readonly ILogger _logger;

        public CompactFormatEventProcessor(TextWriter inputWriter, ILogger logger)
        {
            _inputWriter = inputWriter;
            _logger = logger;
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            _logger.Verbose("Processor Shutting Down. Partition {PartitionId}, Reason: {Reason}.", context.PartitionId, reason);
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            _logger.Verbose("{EventProcessor} initialized. Partition: {PartitionId}", nameof(CompactFormatEventProcessor), context.PartitionId);
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            _logger.Error(error, "Error on Partition: {PartitionId}", context.PartitionId);
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                _inputWriter.WriteLine(data);
                _logger.Verbose("Message received. Partition: {PartitionId}", context.PartitionId);
            }

            return context.CheckpointAsync();
        }
    }
}
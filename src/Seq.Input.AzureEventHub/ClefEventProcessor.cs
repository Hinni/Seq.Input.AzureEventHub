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
    public class ClefEventProcessor : IEventProcessor
    {
        private readonly object processLock = new object();
        private readonly TextWriter _inputWriter;
        private readonly ILogger _logger;
        private readonly bool _verboseEnabled;

        public ClefEventProcessor(TextWriter inputWriter, ILogger logger, bool verboseEnabled)
        {
            _inputWriter = inputWriter;
            _logger = logger;
            _verboseEnabled = verboseEnabled;
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            if (_verboseEnabled)
            {
                _logger.Verbose("{EventProcessor} shutting down. Partition {PartitionId}, Reason: {Reason}.", nameof(ClefEventProcessor), context.PartitionId, reason);
            }

            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            if (_verboseEnabled)
            {
                _logger.Verbose("{EventProcessor} initialized. Partition: {PartitionId}", nameof(ClefEventProcessor), context.PartitionId);
            }

            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            _logger.Error(error, "{EventProcessor} detected an error. Partition: {PartitionId}", nameof(ClefEventProcessor), context.PartitionId);

            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                try
                {
                    var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                    lock (processLock)
                    {
                        _inputWriter.WriteLine(data);
                    }

                    if (_verboseEnabled)
                    {
                        _logger.Verbose("{EventProcessor} received message. Partition: {PartitionId}", nameof(ClefEventProcessor), context.PartitionId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "{EventProcessor} detected an error. Partition: {PartitionId}", nameof(ClefEventProcessor), context.PartitionId);
                }
            }

            return context.CheckpointAsync();
        }
    }
}
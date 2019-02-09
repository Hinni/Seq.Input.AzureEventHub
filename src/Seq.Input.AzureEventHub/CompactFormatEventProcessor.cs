﻿using Microsoft.Azure.EventHubs;
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
        private readonly bool _verboseEnabled;

        public CompactFormatEventProcessor(TextWriter inputWriter, ILogger logger, bool verboseEnabled)
        {
            _inputWriter = inputWriter;
            _logger = logger;
            _verboseEnabled = verboseEnabled;
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            if (_verboseEnabled)
            {
                _logger.Verbose("{EventProcessor} shutting down. Partition {PartitionId}, Reason: {Reason}.", nameof(CompactFormatEventProcessor), context.PartitionId, reason);
            }

            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            if (_verboseEnabled)
            {
                _logger.Verbose("{EventProcessor} initialized. Partition: {PartitionId}", nameof(CompactFormatEventProcessor), context.PartitionId);
            }

            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            _logger.Error(error, "{EventProcessor} detected an error. Partition: {PartitionId}", nameof(CompactFormatEventProcessor), context.PartitionId);

            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                _inputWriter.WriteLine(data);

                if (_verboseEnabled)
                {
                    _logger.Verbose("{EventProcessor} received message. Partition: {PartitionId}", nameof(CompactFormatEventProcessor), context.PartitionId);
                }
            }

            return context.CheckpointAsync();
        }
    }
}
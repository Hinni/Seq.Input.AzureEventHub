using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Serilog;

namespace Seq.Input.AzureEventHub;

// ReSharper disable once ClassNeverInstantiated.Global
sealed class ClefEventProcessor : IEventProcessor
{
    readonly SynchronizedInputWriter _synchronizedInputWriter;
    readonly ILogger _logger;

    public ClefEventProcessor(SynchronizedInputWriter synchronizedInputWriter, ILogger logger)
    {
        _synchronizedInputWriter = synchronizedInputWriter;
        _logger = logger;
    }

    public Task CloseAsync(PartitionContext context, CloseReason reason)
    {
        _logger.Information("{EventProcessor} connection closed to partition {PartitionId} reason: {Reason}.", nameof(ClefEventProcessor), context.PartitionId, reason);

        return Task.CompletedTask;
    }

    public Task OpenAsync(PartitionContext context)
    {
        _logger.Information("{EventProcessor} connection initialized to partition {PartitionId}", nameof(ClefEventProcessor), context.PartitionId);

        return Task.CompletedTask;
    }

    public Task ProcessErrorAsync(PartitionContext context, Exception ex)
    {
        _logger.Error(ex, "{EventProcessor} detected an error on partition {PartitionId}", nameof(ClefEventProcessor), context.PartitionId);

        return Task.CompletedTask;
    }

    public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
    {
        foreach (var eventData in messages)
        {
            try
            {
                var data = Encoding.UTF8.GetString(eventData.Body.AsSpan());
                _synchronizedInputWriter.WriteLine(data);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{EventProcessor} detected an error on partition {PartitionId}", nameof(ClefEventProcessor), context.PartitionId);
            }
        }

        return context.CheckpointAsync();
    }
}
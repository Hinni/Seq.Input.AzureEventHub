using System;
using Microsoft.Azure.EventHubs.Processor;
using Serilog;

namespace Seq.Input.AzureEventHub;

sealed class InputEventProcessorFactory<T> : IEventProcessorFactory where T : class, IEventProcessor
{
    readonly SynchronizedInputWriter _synchronizedInputWriter;
    readonly ILogger _logger;

    public InputEventProcessorFactory(SynchronizedInputWriter synchronizedInputWriter, ILogger logger)
    {
        _synchronizedInputWriter = synchronizedInputWriter;
        _logger = logger;
    }

    public IEventProcessor CreateEventProcessor(PartitionContext context)
    {
        return Activator.CreateInstance(typeof(T), _synchronizedInputWriter, _logger) as T;
    }
}
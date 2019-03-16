using Microsoft.Azure.EventHubs.Processor;
using Serilog;
using System;

namespace Seq.Input.AzureEventHub
{
    public class InputEventProcessorFactory<T> : IEventProcessorFactory where T : class, IEventProcessor
    {
        private readonly SynchronizedInputWriter _synchronizedInputWriter;
        private readonly ILogger _logger;

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
}
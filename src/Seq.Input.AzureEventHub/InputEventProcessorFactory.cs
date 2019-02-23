using Microsoft.Azure.EventHubs.Processor;
using Serilog;
using System;

namespace Seq.Input.AzureEventHub
{
    public class InputEventProcessorFactory<T> : IEventProcessorFactory where T : class, IEventProcessor
    {
        private readonly SynchronizedInputWriter _synchronizedInputWriter;
        private readonly ILogger _logger;
        private readonly bool _verboseEnabled;

        public InputEventProcessorFactory(SynchronizedInputWriter synchronizedInputWriter, ILogger logger, bool verboseEnabled)
        {
            _synchronizedInputWriter = synchronizedInputWriter;
            _logger = logger;
            _verboseEnabled = verboseEnabled;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return Activator.CreateInstance(typeof(T), _synchronizedInputWriter, _logger, _verboseEnabled) as T;
        }
    }
}
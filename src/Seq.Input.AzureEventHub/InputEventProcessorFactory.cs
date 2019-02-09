using Microsoft.Azure.EventHubs.Processor;
using Serilog;
using System;
using System.IO;

namespace Seq.Input.AzureEventHub
{
    public class InputEventProcessorFactory<T> : IEventProcessorFactory where T : class, IEventProcessor
    {
        private readonly TextWriter _inputWriter;
        private readonly ILogger _logger;
        private readonly bool _verboseEnabled;

        public InputEventProcessorFactory(TextWriter inputWriter, ILogger logger, bool verboseEnabled)
        {
            _inputWriter = inputWriter;
            _logger = logger;
            _verboseEnabled = verboseEnabled;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return Activator.CreateInstance(typeof(T), _inputWriter, _logger, _verboseEnabled) as T;
        }
    }
}
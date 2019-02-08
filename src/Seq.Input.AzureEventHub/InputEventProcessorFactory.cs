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

        public InputEventProcessorFactory(TextWriter inputWriter, ILogger logger)
        {
            _inputWriter = inputWriter;
            _logger = logger;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return Activator.CreateInstance(typeof(T), _inputWriter, _logger) as T;
        }
    }
}
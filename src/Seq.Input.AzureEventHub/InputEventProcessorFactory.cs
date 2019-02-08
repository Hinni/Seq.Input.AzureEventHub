using Microsoft.Azure.EventHubs.Processor;
using System;
using System.IO;

namespace Seq.Input.AzureEventHub
{
    public class InputEventProcessorFactory<T> : IEventProcessorFactory where T : class, IEventProcessor
    {
        private TextWriter _inputWriter;

        public InputEventProcessorFactory(TextWriter inputWriter)
        {
            _inputWriter = inputWriter;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return Activator.CreateInstance(typeof(T), _inputWriter) as T;
        }
    }
}
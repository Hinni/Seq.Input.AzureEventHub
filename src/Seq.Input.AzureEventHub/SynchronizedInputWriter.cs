using System.IO;

namespace Seq.Input.AzureEventHub
{
    public class SynchronizedInputWriter
    {
        private readonly object _processLock = new object();
        private readonly TextWriter _inputWriter;

        public SynchronizedInputWriter(TextWriter textWriter)
        {
            _inputWriter = textWriter;
        }

        public void WriteLine(string value)
        {
            lock (_processLock)
            {
                _inputWriter.WriteLine(value);
            }
        }
    }
}
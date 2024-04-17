using System.IO;

namespace Seq.Input.AzureEventHub;

sealed class SynchronizedInputWriter
{
    readonly object _processLock = new();
    readonly TextWriter _inputWriter;

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
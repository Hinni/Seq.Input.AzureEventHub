# Seq.Input.Azure.EventHub

A Seq custom input that pulls messages from Azure EventHub. Built by @Hinni, currently being
experimentally updated to test with new dependencies and host versions.

## Sending events to the input

The input accepts events in [compact JSON format](https://github.com/serilog/serilog-formatting-compact#format-details), encoded as UTF-8 text.

The [_Serilog.Sinks.AzureEventHub_ sink](https://github.com/serilog/serilog-sinks-azureeventhub), along with the [_Serilog.Formatting.Compact_ formatter](https://github.com/serilog/serilog-formatting-compact), can be used for this.

See the _TestConsole_ project included in the repository for an example of client configuration that works with the default input configuration.

## Documentation

https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-dotnet-standard-getstarted-receive-eph

# Seq.Input.AzureEventHub

[![Build Status](https://dev.azure.com/hinnipipe/Github%20Pipeline/_apis/build/status/Hinni.Seq.Input.AzureEventHub?branchName=master)](https://dev.azure.com/hinnipipe/Github%20Pipeline/_build/latest?definitionId=4&branchName=master)
[![GitHub release](https://img.shields.io/github/release/Hinni/Seq.Input.AzureEventHub.svg)](https://github.com/Hinni/Seq.Input.AzureEventHub/releases)
[![NuGet](https://img.shields.io/nuget/v/Seq.Input.AzureEventHub.svg)](https://www.nuget.org/packages/Seq.Input.AzureEventHub/)

A Seq custom input that pulls messages from Azure EventHub. **Requires at least Seq 6.0.3403-pre**

## Getting started

The app is published to NuGet as [_Seq.Input.AzureEventHub_](https://nuget.org/packages/Seq.Input.AzureEventHub). Follow the instructions for [installing a Seq App](https://docs.getseq.net/docs/installing-seq-apps) and start an instance of the app, providing your Azure Event Hub details.

## Sending events to the input

The input accepts events in [compact JSON format](https://github.com/serilog/serilog-formatting-compact#format-details), encoded as UTF-8 text.

The [_Serilog.Sinks.AzureEventHub_](https://github.com/serilog/serilog-sinks-azureeventhub) sink, along with the [_Serilog.Formatting.Compact_](https://github.com/serilog/serilog-formatting-compact) formatter, can be used for this.

See the _TestConsole_ project included in the repository for an example of client configuration that works with the default input configuration.

In this cloud scenario, the EventHub can be used as a forwarder, which was previously done on a VM with locally installed [_Seq-Forwarder_](https://github.com/datalust/seq-forwarder).

![How to use](/img/HowToUse.png)

## Documentation

https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-dotnet-standard-getstarted-receive-eph
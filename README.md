# Seq.Input.AzureEventHub

[![Build Status](https://dev.azure.com/hinnipipe/Github%20Pipeline/_apis/build/status/Hinni.Seq.Input.AzureEventHub?branchName=master)](https://dev.azure.com/hinnipipe/Github%20Pipeline/_build/latest?definitionId=4&branchName=master)
[![GitHub release](https://img.shields.io/github/release/Hinni/Seq.Input.AzureEventHub.svg)](https://github.com/Hinni/Seq.Input.AzureEventHub/releases)
[![NuGet](https://img.shields.io/nuget/v/Seq.Input.AzureEventHub.svg)](https://www.nuget.org/packages/Seq.Input.AzureEventHub/)

A Seq custom input that pulls messages from Azure EventHub. **Requires Seq 5.1+ and currently works only on docker and not on Windows instance**

## Getting started

The app is published to NuGet as [_Seq.Input.AzureEventHub_](https://nuget.org/packages/Seq.Input.AzureEventHub). Follow the instructions for [installing a Seq App](https://docs.getseq.net/docs/installing-seq-apps) and start an instance of the app, providing your Azuzre Event Hub details.

## Sending events to the input

The input accepts events in [compact JSON format](https://github.com/serilog/serilog-formatting-compact#format-details), encoded as UTF-8 text.

The [_Serilog.Sinks.AzureEventHub_ sink](https://github.com/serilog/serilog-sinks-azureeventhub), along with the [_Serilog.Formatting.Compact_ formatter](https://github.com/serilog/serilog-formatting-compact), can be used for this.

See the _TestConsole_ project included in the repository for an example of client configuration that works with the default input configuration.

## Documentation

https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-dotnet-standard-getstarted-receive-eph
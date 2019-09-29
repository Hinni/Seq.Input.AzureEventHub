﻿using Microsoft.Azure.EventHubs;
using Seq.Apps;
using System.IO;

namespace Seq.Input.AzureEventHub
{
    [SeqApp("Azure Event Hub Input", AllowReprocessing = false,
        Description = "Pulls JSON-formatted events from an Azure EventHub. " +
                      "For details of the supported JSON schema, see " +
                      "https://github.com/serilog/serilog-formatting-compact/#format-details.")]
    public class AzureEventHubInput : SeqApp, IPublishJson
    {
        private AzureEventHubListener azureEventHubListener;

        [SeqAppSetting(
            DisplayName = "EventHub connection string",
            IsOptional = false,
            InputType = SettingInputType.Password,
            HelpText = "Connection string for the Event Hub to receive from.")]
        public string EventHubConnectionString { get; set; }

        [SeqAppSetting(
            DisplayName = "EventHub path/name",
            IsOptional = false,
            InputType = SettingInputType.Text,
            HelpText = "The name of the EventHub.")]
        public string EventHubName { get; set; }

        [SeqAppSetting(
            DisplayName = "Consumer group name",
            IsOptional = true,
            InputType = SettingInputType.Text,
            HelpText = "The name of the consumer group within the Event Hub. The default is `$Default`.")]
        public string ConsumerGroupName { get; set; } = PartitionReceiver.DefaultConsumerGroupName;

        [SeqAppSetting(
            DisplayName = "Storage account connection string",
            IsOptional = false,
            InputType = SettingInputType.Password,
            HelpText = "Connection string to Azure Storage account used for leases and checkpointing.")]
        public string StorageConnectionString { get; set; }

        [SeqAppSetting(
            DisplayName = "Storage account container name",
            IsOptional = false,
            InputType = SettingInputType.Text,
            HelpText = "Azure Storage container name for use by built-in lease and checkpoint manager.")]
        public string StorageContainerName { get; set; }

        public void Start(TextWriter inputWriter)
        {
            azureEventHubListener = new AzureEventHubListener(
                new SynchronizedInputWriter(inputWriter), Log,
                EventHubConnectionString, EventHubName, ConsumerGroupName,
                StorageConnectionString, StorageContainerName);
        }

        public void Stop()
        {
            azureEventHubListener.Stop();
        }
    }
}
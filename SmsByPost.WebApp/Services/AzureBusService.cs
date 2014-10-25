using System;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using SmsByPost.Controllers;
using SmsByPost.Models;

namespace SmsByPost.Services
{
    public class AzureBusService
    {
        public void EnqueueMessage(Letter letter)
        {
            var connectionString = CloudConfigurationManager.GetSetting("ServiceBusConnectionString");
            var client = QueueClient.CreateFromConnectionString(connectionString, "MessageDispatchQueue");

            var brokeredMessage = new BrokeredMessage();
            brokeredMessage.Properties["outboundmessagebody"] = letter.Message;
            brokeredMessage.Properties["msisdn"] = letter.Address;
            brokeredMessage.Properties["channel"] = letter.Id;

            client.Send(brokeredMessage);
        }

        public void EnqueueMessageEvent(Letter letter, MessageEvent messageEvent, Guid letterId, DateTime sceduleTimeUtc)
        {
            var connectionString = CloudConfigurationManager.GetSetting("ServiceBusConnectionString");
            var client = QueueClient.CreateFromConnectionString(connectionString, "MessageEventQueue");

            var brokeredMessage = new BrokeredMessage();
            brokeredMessage.Properties["url"] = GetEndpointForEvent(messageEvent, letterId);
            
            brokeredMessage.ScheduledEnqueueTimeUtc = sceduleTimeUtc;

            client.Send(brokeredMessage);
        }

        private static string GetEndpointForEvent(MessageEvent messageEvent, Guid letterId)
        {
            switch (messageEvent)
            {
                case MessageEvent.Dispatch:
                    return "api/DispatchEvent/" + letterId;
            }

            return null;
        }
    }
}
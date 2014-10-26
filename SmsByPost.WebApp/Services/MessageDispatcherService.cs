using System;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using SmsByPost.Controllers;
using SmsByPost.Models;

namespace SmsByPost.Services
{
    public class MessageDispatcherService
    {
        public void EnqueueMessage(Letter letter)
        {
            var connectionString = CloudConfigurationManager.GetSetting("ServiceBusConnectionString");
            var client = QueueClient.CreateFromConnectionString(connectionString, "MessageDispatchQueue");

            var brokeredLetter = new BrokeredMessage();
            brokeredLetter.Properties["id"] = letter.Id;
            brokeredLetter.Properties["outboundmessagebody"] = letter.Message;
            brokeredLetter.Properties["msisdn"] = letter.Address;
            brokeredLetter.Properties["channel"] = letter.Id;

            client.Send(brokeredLetter);
        }

        private static string GetNotificationOfSendingMessageBody(Letter letter)
        {
            return string.Format(letter.Originator + " has sent you a package via {1} post, you can track the location of your package by entering your reference number ({0}) in to the tracking page of our website.", letter.Id, letter.Method);
        }

        public void EnqueueMessageEvent(Letter letter, MessageEvent messageEvent, Guid letterId, DateTime sceduleTimeUtc)
        {
            var connectionString = CloudConfigurationManager.GetSetting("ServiceBusConnectionString");
            var client = QueueClient.CreateFromConnectionString(connectionString, "MessageEventQueue");

            var brokeredMessage = new BrokeredMessage();
            brokeredMessage.Properties["url"] = GetEndpointForEvent(messageEvent, letterId);
            
            brokeredMessage.ScheduledEnqueueTimeUtc = sceduleTimeUtc;

            var notification = new Letter
            {
                Address = letter.Address,
                Id = Guid.NewGuid(),
                Message = GetNotificationOfSendingMessageBody(letter)
            };

            EnqueueMessage(notification);
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
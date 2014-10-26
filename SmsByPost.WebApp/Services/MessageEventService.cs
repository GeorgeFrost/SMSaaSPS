using System;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using SmsByPost.Controllers;
using SmsByPost.Models;

namespace SmsByPost.Services
{
    public class MessageEventService
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

        public void EnqueueMessageDispatchEvent(Letter letter, Guid letterId, DateTime sceduleTimeUtc)
        {
            var connectionString = CloudConfigurationManager.GetSetting("ServiceBusConnectionString");
            var client = QueueClient.CreateFromConnectionString(connectionString, "MessageEventQueue");

            var brokeredMessage = new BrokeredMessage();
            brokeredMessage.Properties["url"] = "api/DispatchEvent/" + letterId;
            
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

        public void EnqueueMessageArrivedAtLocalSortingHouse(Letter letter, Guid letterId, DateTime sceduleTimeUtc)
        {
            var connectionString = CloudConfigurationManager.GetSetting("ServiceBusConnectionString");
            var client = QueueClient.CreateFromConnectionString(connectionString, "MessageEventQueue");

            var brokeredMessage = new BrokeredMessage();
            brokeredMessage.Properties["url"] = "api/ArrivedAtLocalSortingHouse/" + letterId;

            brokeredMessage.ScheduledEnqueueTimeUtc = sceduleTimeUtc;

            client.Send(brokeredMessage);
        }

        public void EnqueueMessageArrivedAtNationalSortingHubTime(Letter letter, Guid letterId, DateTime arrivedAtNationalSortingHubTime)
        {
            var connectionString = CloudConfigurationManager.GetSetting("ServiceBusConnectionString");
            var client = QueueClient.CreateFromConnectionString(connectionString, "MessageEventQueue");

            var brokeredMessage = new BrokeredMessage();
            brokeredMessage.Properties["url"] = "api/ArrivedAtNationalSortingHub/" + letterId;

            brokeredMessage.ScheduledEnqueueTimeUtc = arrivedAtNationalSortingHubTime;

            client.Send(brokeredMessage);
        }

        public void EnqueueMessageArrivedAtDestinationSortingDepotTime(Letter letter, Guid letterId, DateTime arrivedAtDestinationSortingDepotTime)
        {
            var connectionString = CloudConfigurationManager.GetSetting("ServiceBusConnectionString");
            var client = QueueClient.CreateFromConnectionString(connectionString, "MessageEventQueue");

            var brokeredMessage = new BrokeredMessage();
            brokeredMessage.Properties["url"] = "api/ArrivedAtDestinationSortingDepot/" + letterId;

            brokeredMessage.ScheduledEnqueueTimeUtc = arrivedAtDestinationSortingDepotTime;

            client.Send(brokeredMessage);
        }

        public void EnqueueMessageOnRouteToDelivery(Letter letter, Guid letterId, DateTime onRouteToDeliveryTime)
        {
            var connectionString = CloudConfigurationManager.GetSetting("ServiceBusConnectionString");
            var client = QueueClient.CreateFromConnectionString(connectionString, "MessageEventQueue");

            var brokeredMessage = new BrokeredMessage();
            brokeredMessage.Properties["url"] = "api/OnRouteToDelivery/" + letterId;

            brokeredMessage.ScheduledEnqueueTimeUtc = onRouteToDeliveryTime;

            client.Send(brokeredMessage);
        }
    }
}
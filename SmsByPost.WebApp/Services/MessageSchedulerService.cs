using System;
using Clockwork.XML;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using SmsByPost.Models;

namespace SmsByPost.Services
{
    public class MessageSchedulerService : IMessageSchedulerService
    {
        public DateTime ScheduleLetterDelivery(Letter letter, DeliveryMethod deliveryMethod)
        {
            int minimumArrivalTime;
            int maximumArrivalTime;

            var randomNumberGenerator = new Random();

            var willBeDelayed = randomNumberGenerator.Next(1, 11) > 9;
           
            switch (deliveryMethod)
            {
                case DeliveryMethod.FirstClass:
                    if (willBeDelayed)
                    {
                        minimumArrivalTime = randomNumberGenerator.Next(3, 6);
                        maximumArrivalTime = randomNumberGenerator.Next(minimumArrivalTime, 14);
                    }
                    else
                    {
                        minimumArrivalTime = int.Parse(CloudConfigurationManager.GetSetting("FirstClassDeliveryMinimumTimeDays"));
                        maximumArrivalTime = int.Parse(CloudConfigurationManager.GetSetting("FirstClassDeliveryMaximumTimeDays"));
                    }
                    break;
                case DeliveryMethod.Special:
                    if (willBeDelayed)
                    {
                        minimumArrivalTime = randomNumberGenerator.Next(1, 1);
                        maximumArrivalTime = randomNumberGenerator.Next(minimumArrivalTime, 3);
                    }
                    else
                    {
                        minimumArrivalTime = int.Parse(CloudConfigurationManager.GetSetting("SpecialDeliveryMinimumTimeDays"));
                        maximumArrivalTime = int.Parse(CloudConfigurationManager.GetSetting("SpecialDeliveryMaximumTimeDays"));
                    }
                    break;
                default:
                    return DateTime.UtcNow;
            }

            var randomDays = randomNumberGenerator.Next(minimumArrivalTime, maximumArrivalTime + 1);
            var deliveryDate = DateTime.UtcNow.AddDays(randomDays);
            var deliveryHour = randomNumberGenerator.Next(8, 16);
            var deliveryMinutes = randomNumberGenerator.Next(0, 60);
            var deliverySeconds = randomNumberGenerator.Next(0, 60);

            return new DateTime(deliveryDate.Year, deliveryDate.Month, deliveryDate.Day, deliveryHour, deliveryMinutes, deliverySeconds);
        }

        public DateTime MessageArrivedAtLocalSortingDepot(DateTime messageDeliveryTime)
        {
            TimeSpan span = messageDeliveryTime - DateTime.UtcNow;
            var TicksToMessageArrivedAtLocalSortingDepot = span.Ticks/100*10;
            return DateTime.UtcNow.AddTicks(TicksToMessageArrivedAtLocalSortingDepot);
        }

        public DateTime MessageArrivedAtNationalSortingHub(DateTime messageDeliveryTime)
        {
            TimeSpan span = messageDeliveryTime - DateTime.UtcNow;
            var TicksToMessageArrivedAtJationalSortingHub = span.Ticks / 100 * 50;
            return DateTime.UtcNow.AddTicks(TicksToMessageArrivedAtJationalSortingHub);
        }

        public DateTime MessageArrivedAtDestinationSortingDepot(DateTime messageDeliveryTime)
        {
            TimeSpan span = messageDeliveryTime - DateTime.UtcNow;
            var TicksToMessageArrivedAtDestrinationSortingDepot = span.Ticks / 100 * 80;
            return DateTime.UtcNow.AddTicks(TicksToMessageArrivedAtDestrinationSortingDepot);
        }

        public DateTime MessageOnRouteToDelivery(DateTime messageDeliveryTime)
        {
            TimeSpan span = messageDeliveryTime - DateTime.UtcNow;
            var TicksToMessageOnRouteToDelivery = span.Ticks / 100 * 90;
            return DateTime.UtcNow.AddTicks(TicksToMessageOnRouteToDelivery);
        }

    }

    public interface IMessageSchedulerService
    {
        DateTime ScheduleLetterDelivery(Letter letter, DeliveryMethod deliveryMethod);
        DateTime MessageArrivedAtLocalSortingDepot(DateTime messageDeliveryTime);
        DateTime MessageArrivedAtNationalSortingHub(DateTime messageDeliveryTime);
        DateTime MessageArrivedAtDestinationSortingDepot(DateTime messageDeliveryTime);
        DateTime MessageOnRouteToDelivery(DateTime messageDeliveryTime);

    }

    public class ImmediateDispatchService : IMessageSchedulerService
    {
        public DateTime MessageArrivedAtLocalSortingDepot(DateTime messageDeliveryTime)
        {
            return DateTime.UtcNow.AddSeconds(5);
        }

        public DateTime MessageArrivedAtNationalSortingHub(DateTime messageDeliveryTime)
        {
            return DateTime.UtcNow.AddSeconds(10);
        }

        public DateTime MessageArrivedAtDestinationSortingDepot(DateTime messageDeliveryTime)
        {
            return DateTime.UtcNow.AddSeconds(20);
        }

        public DateTime MessageOnRouteToDelivery(DateTime messageDeliveryTime)
        {
            return DateTime.UtcNow.AddSeconds(40);
        }

        public DateTime ScheduleLetterDelivery(Letter letter, DeliveryMethod deliveryMethod)
        {
            return DateTime.UtcNow.AddSeconds(60);
        }
    }
}
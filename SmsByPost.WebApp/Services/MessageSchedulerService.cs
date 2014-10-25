using System;
using Microsoft.WindowsAzure;
using SmsByPost.Models;

namespace SmsByPost.Services
{
    public class MessageSchedulerService
    {
        public DateTime ScheduleLetter(Letter letter, DeliveryMethod deliveryMethod)
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmsByPost.Controllers;
using SmsByPost.Models;

namespace SmsByPost.Services
{
    public static class TrackingServiceBuilder
    {
        public static TrackingStatus GetTrackingStatus(Event even)
        {
            var friendlyDescription = GetFriendlyTrackingDescription(even);
            var trackingStatus = new TrackingStatus()
            {
                EventTime = even.ScheduledDateTimeUtc,
                FriendlyEventTime = even.ScheduledDateTimeUtc.ToLongTimeString() + ",  " + even.ScheduledDateTimeUtc.ToLongDateString(),
                FriendlyTrackingName = friendlyDescription.Item1,
                FriendlyTrackingDescription = friendlyDescription.Item2
            };
            return trackingStatus;
        }

        private static Tuple<string, string> GetFriendlyTrackingDescription(Event even)
        {
            if (even.EventName == "ArrivedAtDestinationSortingDepot")
            {
                return new Tuple<string, string>("NOTTINGHAM SORTING DEPOT", "Delivery has been scanned in at the DESTINATION sorting depot and will be out for delivery shortly.");
            }
            else if (even.EventName == "ArrivedAtLocalSortingHouse")
            {
                return new Tuple<string, string>("SHEFFIELD SORTING DEPOT", "Delivery has been scanned in at the LOCAL sorting depot and will be transfered to the NATIONAL SORTING HUB.");
            }
            else if (even.EventName == "ArrivedAtNationalSortingHub")
            {
                return new Tuple<string, string>("NATIONAL SORTING HUB", "Delivery has been scanned in at the NATIONAL SORTING HUB and will be transfered to the DESITINATION SORTING DEPOT shortly.");
            }
            else if (even.EventName == "OnRouteToDelivery")
            {
                return new Tuple<string, string>("ON ROUTE TO RECIPIENT", "Delivery will be made shortly");
            }
            else if (even.EventName == "SmsDispatchedEvent")
            {
                return new Tuple<string, string>("DELIVERY MADE", "Delivery has been attempted with the recipient");
            }

            return new Tuple<string, string>(even.EventName, even.EventName);
        }
    }
}
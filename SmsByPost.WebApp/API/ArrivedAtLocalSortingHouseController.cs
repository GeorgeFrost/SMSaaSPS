using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PusherServer;
using SmsByPost.Models;
using SmsByPost.Services;

namespace SmsByPost.API
{
    public class ArrivedAtLocalSortingHouseController : ApiController
    {
        private readonly AzureBlobService _azureBlobService;

        public ArrivedAtLocalSortingHouseController()
        {
            _azureBlobService = new AzureBlobService();
        }

        public HttpStatusCode Post(Guid id)
        {
            var scheduledDateTimeUtc = DateTime.UtcNow;

            var letter = _azureBlobService.GetFromBlobStore(id);
            var newEvent = new Event() { EventName = "ArrivedAtLocalSortingHouse", ScheduledDateTimeUtc = scheduledDateTimeUtc };
            letter.Events.Add(newEvent);

            _azureBlobService.UploadToAzureBlobStore(letter);

           var eventModelToPush = TrackingServiceBuilder.GetTrackingStatus(newEvent);

            var pusher = new Pusher("94194", "09e07fa6d1e3db728a17", "a1f339dc466359b5915b");
            var result = pusher.Trigger(id.ToString(), "Arrived_At_Local_Sorting_House_Event", new { FriendlyTrackingName = eventModelToPush.FriendlyTrackingName, FriendlyTrackingDescription = eventModelToPush.FriendlyTrackingDescription, FriendlyEventTime = eventModelToPush.FriendlyEventTime });

            return HttpStatusCode.OK;
        }
    }
}

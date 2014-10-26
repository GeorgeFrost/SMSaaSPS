using System;
using System.Net;
using System.Web.Http;
using PusherServer;
using SmsByPost.Models;
using SmsByPost.Services;

namespace SmsByPost.API
{
    public class ArrivedAtDestinationSortingDepotController : ApiController
    {
        private readonly AzureBlobService _azureBlobService;

        public ArrivedAtDestinationSortingDepotController()
        {
            _azureBlobService = new AzureBlobService();
        }

        public HttpStatusCode Post(Guid id)
        {
            var scheduledDateTimeUtc = DateTime.UtcNow;

            var letter = _azureBlobService.GetFromBlobStore(id);
            var newEvent = new Event() { EventName = "ArrivedAtDestinationSortingDepot", ScheduledDateTimeUtc = scheduledDateTimeUtc };

            letter.Events.Add(newEvent);
            _azureBlobService.UploadToAzureBlobStore(letter);

            var eventModelToPush = TrackingServiceBuilder.GetTrackingStatus(newEvent);

            var pusher = new Pusher("94194", "09e07fa6d1e3db728a17", "a1f339dc466359b5915b");
            var result = pusher.Trigger(id.ToString(), "Arrived_At_Destination_Sorting_Depot_Event", new { FriendlyTrackingName = eventModelToPush.FriendlyTrackingName, FriendlyTrackingDescription = eventModelToPush.FriendlyTrackingDescription, FriendlyEventTime = eventModelToPush.FriendlyEventTime });

            return HttpStatusCode.OK;
        }
    }
}
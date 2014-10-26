using System;
using System.Net;
using System.Web.Http;
using PusherServer;
using SmsByPost.Models;
using SmsByPost.Services;

namespace SmsByPost.API
{
    public class OnRouteToDeliveryController : ApiController
    {
        private readonly AzureBlobService _azureBlobService;

        public OnRouteToDeliveryController()
        {
            _azureBlobService = new AzureBlobService();
        }

        public HttpStatusCode Post(Guid id)
        {
            var scheduledDateTimeUtc = DateTime.UtcNow;

            var letter = _azureBlobService.GetFromBlobStore(id);
            letter.Events.Add(new Event() { EventName = "OnRouteToDelivery", ScheduledDateTimeUtc = scheduledDateTimeUtc });
            _azureBlobService.UploadToAzureBlobStore(letter);

            var pusher = new Pusher("94194", "09e07fa6d1e3db728a17", "a1f339dc466359b5915b");
            var result = pusher.Trigger(id.ToString(), "On_Route_To_Delivery", new { status = "The message is out for delivery", timestamp = scheduledDateTimeUtc });

            return HttpStatusCode.OK;
        }
    }
}
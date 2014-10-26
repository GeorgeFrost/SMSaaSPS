using System;
using System.Net;
using System.Web.Http;
using PusherServer;
using SmsByPost.Models;
using SmsByPost.Services;

namespace SmsByPost.API
{
    public class DispatchEventController : ApiController 
    {
        private readonly AzureBlobService _azureBlobService;
        private readonly MessageEventService _messageEventService;

        public DispatchEventController()
        {
            _azureBlobService = new AzureBlobService();
            _messageEventService = new MessageEventService();
        }

        // POST: api/Dispatch/5
        public HttpStatusCode Post(Guid id)
        {
            var scheduledDateTimeUtc = DateTime.UtcNow;

            var letter = _azureBlobService.GetFromBlobStore(id);
            letter.Events.Add(new Event() { EventName = "sms_dispatched_event", ScheduledDateTimeUtc = scheduledDateTimeUtc });
            _azureBlobService.UploadToAzureBlobStore(letter);

            var pusher = new Pusher("94194", "09e07fa6d1e3db728a17", "a1f339dc466359b5915b");
            var result = pusher.Trigger(new[] { letter.Address, id.ToString() }, "sms_dispatched_event", new { message = letter.Message, msisdn = letter.Address });

            _messageEventService.EnqueueMessage(letter);
            
            return HttpStatusCode.OK;
        }
    }
}

using System;
using System.Net;
using System.Web.Http;
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

            _messageEventService.EnqueueMessage(letter);
            
            return HttpStatusCode.OK;
        }
    }
}

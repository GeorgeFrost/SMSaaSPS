using System;
using System.Net;
using System.Web.Http;
using SmsByPost.Services;
using SmsByPost.Services;

namespace SmsByPost.API
{
    public class DispatchEventController : ApiController 
    {
        private readonly AzureBlobService _azureBlobService;
        private readonly MessageDispatcherService _azureBusService;

        public DispatchEventController()
        {
            _azureBlobService = new AzureBlobService();
            _azureBusService = new MessageDispatcherService();
        }

        // POST: api/Dispatch/5
        public HttpStatusCode Post(Guid id)
        {
            var letter = _azureBlobService.GetFromBlobStore(id);

            _azureBusService.EnqueueMessage(letter);
            
            return HttpStatusCode.OK;
        }
    }
}

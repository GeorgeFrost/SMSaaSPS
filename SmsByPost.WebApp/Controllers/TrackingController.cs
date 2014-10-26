using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmsByPost.Models;
using SmsByPost.Services;

namespace SmsByPost.Controllers
{
    public class TrackingController : Controller
    {
        private AzureBlobService _azureBlobService;

        public TrackingController()
        {
            _azureBlobService = new AzureBlobService();
        }

        // GET: Tracking
        public ActionResult Index(Guid id)
        {
            var ViewModel = new TrackingModel();

            Letter letter = _azureBlobService.GetFromBlobStore(id);

            foreach (var messageEvent in letter.Events)
            {
                var trackingStatus = TrackingServiceBuilder.GetTrackingStatus(messageEvent);

                ViewModel.TrackingStatuses.Add(trackingStatus);
            }

            ViewModel.LetterId = id;

            return View(ViewModel);
        }

    }

    public class TrackingModel
    {
        public TrackingModel()
        {
            TrackingStatuses = new List<TrackingStatus>();
        }

        public List<TrackingStatus> TrackingStatuses { get; set; }
        public Guid LetterId { get; set; }
    }

    public class TrackingStatus
    {
        public string FriendlyTrackingName { get; set; }
        public string FriendlyTrackingDescription { get; set; }
        public string FriendlyEventTime { get; set; }
        public DateTime EventTime { get; set; }
    }
}

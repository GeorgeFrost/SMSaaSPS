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

            foreach (var even in letter.Events)
            {
                var friendlyDescription = GetFriendlyTrackingDescription(even);
                var trackingStatus = new TrackingStatus()
                {
                    EventTime = even.ScheduledDateTimeUtc,
                    FriendlyEventTime = even.ScheduledDateTimeUtc.ToShortDateString(),
                    FriendlyTrackingName = friendlyDescription.Item1,
                    FriendlyTrackingDescription = friendlyDescription.Item2
                };

                ViewModel.TrackingStatuses.Add(trackingStatus);
            }

            ViewModel.LetterId = id;

            return View(ViewModel);
        }

        private Tuple<string, string> GetFriendlyTrackingDescription(Event even )
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

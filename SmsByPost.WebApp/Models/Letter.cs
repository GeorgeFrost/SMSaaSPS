using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsByPost.Models
{
    [Serializable]
    public class Letter
    {
        public bool Wrapped;
        public string Originator;
        public Packaging PackagingType;
        public string Address;
        public string Message;
        public DeliveryMethod Method;
        public Guid Id;

        public List<Event> Events { get; set; }

        public Letter(Guid id, string address, string message, DeliveryMethod method, Packaging packagingType, bool wrapped, string originator)
        {
            Wrapped = wrapped;
            Originator = originator;
            PackagingType = packagingType;
            Id = id;
            Address = address;
            Message = message;
            Method = method;

            Events = new List<Event>();
        }

        public Letter()
        {
            Events = new List<Event>();
        }
    }

    public class Event
    {
        public string EventName { get; set; }
        public DateTime ScheduledDateTimeUtc { get; set; }
    }
}
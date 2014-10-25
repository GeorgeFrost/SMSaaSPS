using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsByPost.API.Response
{
    public class response
    {
        public SmsMessagesResponseData messages { get; set; }

        public string errors { get; set; }
    }
    public class SmsMessagesResponseData
    {
        public SmsMessageResponseData message { get; set; }
    }

    public class SmsMessageResponseData
    {
        public string elementat { get; set; }

        public string characterset { get; set; }

        public string parts { get; set; }

        public string availablecharactersinlastpart { get; set; }
    }
}
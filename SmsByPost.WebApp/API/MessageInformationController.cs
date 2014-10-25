using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace SmsByPost.API
{
    public class MessageInformationController : ApiController
    {
        // GET: api/MessageInformation/5
        public MessageInformationViewModel Get(string message)
        {
            string authInfo = "oliver.tomlinson@esendex.com" + ":" + "naahLE97CHRF";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

            var restClient = new RestClient("http://api.esendex.com/v1.0");

            restClient.AddHandler("application/xml", (IDeserializer)new DotNetXmlDeserializer());

            var request = new RestRequest("/messages/information", Method.POST);

            request.AddHeader("Accept", "application/xml");
            request.AddHeader("Authorization", "Basic " + authInfo);
            request.AddHeader("Content-Type", "application/xml");

            var data = new Request.messages(
                new Request.SmsMessageRequestData() { body = message, characterset = "Auto" });

            request.AddBody(data, "http://api.esendex.com/ns/");

            var result = restClient.Post<Response.response>(request);

            var serializeObject = JsonConvert.SerializeObject(result.Content);

            var res = new MessageInformationViewModel();

            if (result.Content.Contains("<parts>"))
            {
                var parts = result.Content.Substring(result.Content.IndexOf("<parts>") + 7, 1);
                res.Parts = parts;
            }

            if (result.Content.Contains("<availablecharactersinlastpart>"))
            {
                var charsleft = result.Content.Substring(result.Content.IndexOf("<availablecharactersinlastpart>") + 31, 3);

                charsleft = charsleft.Replace("<", string.Empty).Replace("/", string.Empty);
                res.CharsLeft = charsleft;
            }

            if (result.Content.Contains("<characterset>Unicode</characterset>"))
            {
                res.CharacterSet = "Unicode";
            }
            else
            {
                res.CharacterSet = "GSM";
            }

            return res;
        }
    }

    public class MessageInformationViewModel
    {
        public string Parts { get; set; }
        public string CharsLeft { get; set; }
        public string CharacterSet { get; set; }
    }



}

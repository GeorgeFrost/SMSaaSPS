using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using SmsByPost.Models;

namespace SmsByPost.API
{
    public class MessageInformationController : ApiController
    {
        // GET: api/MessageInformation/5
        public PostageInformationViewModel Get(string message, bool isGiftWrapped, string protection, string deliveryMethod)
        {
            var postageInformationViewModel = new PostageInformationViewModel();

            var messageInformation = GetMessageInformation(message);

            postageInformationViewModel.MessageInformation = messageInformation;

            var pricingOption = CalculatePricingOption(messageInformation, isGiftWrapped, protection, deliveryMethod);

            postageInformationViewModel.PriceOption = pricingOption;

            return postageInformationViewModel;
        }

        private PricingOption CalculatePricingOption(MessageInformationViewModel messageInformation, bool isGiftWrapped, string protection, string deliveryType)
        {
            var parts = int.Parse(messageInformation.Parts);
            decimal price = 0;

            //weight
            if (parts == 1)
                price += 0.62m;
            else if (parts == 2)
                price += 0.92m;
            else if (parts > 2)
                price = 0.92m*parts;


            //delivery method
            var deliveryMethod = ParseEnum<DeliveryMethod>(deliveryType);
            if (deliveryMethod == DeliveryMethod.FirstClass)
                price *= 1;
            else if (deliveryMethod == DeliveryMethod.Special)
                price *= 1.2m;
            

            //protection/packaging
            var packaging = ParseEnum<Packaging>(protection);
            if (packaging == Packaging.Envelope)
                price *= 1;
            else if (packaging == Packaging.PaddedEnvelope)
                price *= 1.1m;
            else if (packaging == Packaging.Parcel)
                price *= 1.3m;
            
            //isGiftWrapped
            if (isGiftWrapped)
                price *= 1.1m;


            //special delivery flat rate addition
            if (deliveryMethod == DeliveryMethod.Special)
                price += 5.95m; 

            return new PricingOption(){ Price = price.ToString()};
        }

        private static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }



        private static MessageInformationViewModel GetMessageInformation(string message)
        {
            string authInfo = "oliver.tomlinson@esendex.com" + ":" + "naahLE97CHRF";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

            var restClient = new RestClient("http://api.esendex.com/v1.0");

            restClient.AddHandler("application/xml", (IDeserializer) new DotNetXmlDeserializer());

            var request = new RestRequest("/messages/information", Method.POST);

            request.AddHeader("Accept", "application/xml");
            request.AddHeader("Authorization", "Basic " + authInfo);
            request.AddHeader("Content-Type", "application/xml");

            var data = new Request.messages(
                new Request.SmsMessageRequestData() {body = message, characterset = "Auto"});

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

    public class PricingOption
    {
        public string Price { get; set; }
    }

    public class PostageInformationViewModel
    {
        public MessageInformationViewModel MessageInformation { get; set; }
        public PricingOption PriceOption { get; set; }
    }

    public class MessageInformationViewModel
    {
        public string Parts { get; set; }
        public string CharsLeft { get; set; }
        public string CharacterSet { get; set; }
    }



}

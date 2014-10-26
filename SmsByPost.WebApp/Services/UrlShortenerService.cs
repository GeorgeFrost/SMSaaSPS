using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace SmsByPost.Services
{
    public class UrlShortenerService
    {
        public UrlShortenerService()
        {

        }

        public string Shorten(string longUri)
        {
            var uri = string.Format("https://api-ssl.bitly.com/v3/shorten?access_token=a5123d9f8b3a70845075f66d7845f85cdf57bc66&longUrl={0}", longUri);
            
            var request = WebRequest.Create(uri);

            var objStream = request.GetResponse().GetResponseStream();
            var response = new StreamReader(objStream).ReadToEnd();

            var url = response.IndexOf("\"url\":");
            var endUrl = response.IndexOf("\",", url);

            return response.Substring(url + 8, (endUrl - url) - 8).Replace("\\", "");
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmsByPost.API
{
    public class PushNotificationsController : ApiController
    {
        // GET: api/PushNotifications
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/PushNotifications/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PushNotifications
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PushNotifications/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PushNotifications/5
        public void Delete(int id)
        {
        }
    }
}

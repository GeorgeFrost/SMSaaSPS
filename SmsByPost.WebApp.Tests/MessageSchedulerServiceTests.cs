using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SmsByPost.Services;

namespace SmsByPost.WebApp.Tests
{
    [TestFixture]
    public class MessageSchedulerServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            var service = new MessageSchedulerService();

            var messageDeliveryTime = DateTime.UtcNow.AddSeconds(60);

            var result = service.MessageArrivedAtLocalSortingDepot(messageDeliveryTime);
        }

        [Test]
        public void sdfasdf()
        {
            

        }
        
    }
}

using System;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using SmsByPost.API.Request;
using SmsByPost.Models;
using SmsByPost.Services;
using SmsByPost.Services;

namespace SmsByPost.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult SendMessage(Guid godGuid, string originator, string address, string message, string deliveryType, string packagingType, string paperDesign = "Standard", bool wrappingRequired = false)
        {
            var body = message;

            var parsedDeliveryMethod = ParseEnum<DeliveryMethod>(deliveryType);
            var parsedPackagingType = ParseEnum<Packaging>(packagingType);
            var parsedPaperDesign = ParseEnum<WrappingPaperPatterns>(paperDesign);

            var corruptionCreatorService = new CorruptionCreatorService();
            body = corruptionCreatorService.PossiblyCorrupt(body, parsedPackagingType);

            if (wrappingRequired)
            {
                var wrappingService = new WrappingService();
                body = wrappingService.Wrap(body, parsedPaperDesign);
            }

            var letter = new Letter(godGuid, address, body, parsedDeliveryMethod, parsedPackagingType, wrappingRequired,originator);

            IMessageSchedulerService messageScheduler = !message.StartsWith("NOW ") ? (IMessageSchedulerService) new MessageSchedulerService() : new ImmediateDispatchService();
            var deliveryTime = messageScheduler.ScheduleLetterDelivery(letter, parsedDeliveryMethod);

            new AzureBlobService().UploadToAzureBlobStore(letter);

            var arrivedAtLocalSortingDepotTime = messageScheduler.MessageArrivedAtLocalSortingDepot(deliveryTime);
            var arrivedAtNationalSortingHubTime = messageScheduler.MessageArrivedAtNationalSortingHub(deliveryTime);
            var arrivedAtDestinationSortingDepotTime = messageScheduler.MessageArrivedAtDestinationSortingDepot(deliveryTime);
            var onRouteToDeliveryTime = messageScheduler.MessageOnRouteToDelivery(deliveryTime);

            new MessageEventService().EnqueueMessageArrivedAtLocalSortingHouse(letter, godGuid, arrivedAtLocalSortingDepotTime);
            new MessageEventService().EnqueueMessageArrivedAtNationalSortingHubTime(letter, godGuid, arrivedAtNationalSortingHubTime);
            new MessageEventService().EnqueueMessageArrivedAtDestinationSortingDepotTime(letter, godGuid,arrivedAtDestinationSortingDepotTime);
            new MessageEventService().EnqueueMessageOnRouteToDelivery(letter, godGuid, onRouteToDeliveryTime);
            new MessageEventService().EnqueueMessageDispatchEvent(letter, godGuid, deliveryTime);

            return RedirectToAction("Index");
        }

        private static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }

    public class WrappingService
    {
        public string Wrap(string bodyWithSimulatedDamage, WrappingPaperPatterns parsedPaperDesign)
        {
            var builder = new StringBuilder();

            switch (parsedPaperDesign)
            {
                case WrappingPaperPatterns.Standard:
                    builder.Append("*giftwrap* ");
                    builder.Append(bodyWithSimulatedDamage);
                    builder.Append(" *giftwrap*");
                    break;
                case WrappingPaperPatterns.Snow:
                    builder.Append("❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄");
                    builder.Append(bodyWithSimulatedDamage);
                    builder.Append("❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄❄");
                    break;
                case WrappingPaperPatterns.Sun:
                    builder.Append("☀☀☀☀☀☀☀☀☀☀☀☀☀☀☀☀☀☀☀");
                    builder.Append(bodyWithSimulatedDamage);
                    builder.Append("☀☀☀☀☀☀☀☀☀☀☀☀☀☀☀☀☀☀☀");
                    break;
                case WrappingPaperPatterns.YingYang:
                    builder.Append("☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯");
                    builder.Append(bodyWithSimulatedDamage);
                    builder.Append("☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯🐳🐳");
                    break;
            }

            return builder.ToString();
        }
    }
}
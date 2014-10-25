using System;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
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
        public ActionResult SendMessage(Guid godGuid, string address, string message, string deliveryType, string packagingType, string paperDesign, bool wrappingRequired = false)
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

            var letter = new Letter(godGuid, address, body, parsedDeliveryMethod, parsedPackagingType, wrappingRequired);

            var messageScheduler = new MessageSchedulerService();            
            var deliveryTime = messageScheduler.ScheduleLetter(letter, parsedDeliveryMethod);

            new AzureBlobService().UploadToAzureBlobStore(letter);
            new AzureBusService().EnqueueMessageEvent(letter, MessageEvent.Dispatch, godGuid, deliveryTime);

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
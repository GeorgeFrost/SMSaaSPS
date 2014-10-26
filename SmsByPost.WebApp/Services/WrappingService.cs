using System.Text;
using SmsByPost.Models;

namespace SmsByPost.Services
{
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
                    builder.Append("☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯☯");
                    break;
            }

            return builder.ToString();
        }
    }
}
using System;
using System.Text;
using SmsByPost.Models;

namespace SmsByPost.Services
{
    public class CorruptionCreatorService
    {
        public string PossiblyCorrupt(string message, Packaging packagingType)
        {
            var randomNumberGenerator = new Random();
            var builder = new StringBuilder(message);
           
            switch (packagingType)
            {
                case Packaging.Envelope:
                    for (var i = 0; i < message.Length; i++)
                    {
                        if (randomNumberGenerator.Next(0, 31) > 26)
                        {
                            builder[i] = (char) randomNumberGenerator.Next(32, 128);
                        }
                    }

                    return builder.ToString();
                case Packaging.PaddedEnvelope:
                    for (var i = 0; i < message.Length; i++)
                    {
                        if (randomNumberGenerator.Next(0, 31) > 28)
                        {
                            builder[i] = (char) randomNumberGenerator.Next(32, 128);
                        }
                    }

                    return builder.ToString();
                case Packaging.Parcel:
                    for (var i = 0; i < message.Length; i++)
                    {
                        if (randomNumberGenerator.Next(0, 31) > 29)
                        {
                            builder[i] = (char) randomNumberGenerator.Next(32, 128);
                        }
                    }

                    return builder.ToString();    
            }

            return null;
        }
    }
}
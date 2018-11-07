using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace OCR
{
    internal static class Program
    {
        // Replace <Subscription Key> with your valid subscription key.
        private const string subscriptionKey = "<KEY>";

        // You must use the same Azure region in your REST API method as you used to
        // get your subscription keys. For example, if you got your subscription keys
        // from the West US region, replace "westcentralus" in the URL
        // below with "westus".
        //
        // Free trial subscription keys are generated in the West Central US region.
        // If you use a free trial subscription key, you shouldn't need to change
        // this region.
        private const string uriBase = "https://canadacentral.api.cognitive.microsoft.com";

        private static void Main()
        {
            // Get the path and filename to process from the user.
            Console.WriteLine("Optical Character Recognition:");
            Console.Write("Enter the path to an image with text you wish to read: ");
            var imageFilePath = Console.ReadLine();

            if (File.Exists(imageFilePath))
            {
                // Call the REST API method.
                Console.WriteLine("\nWait a moment for the results to appear.\n");
                MakeApiRequest(imageFilePath).Wait();
            }
            else
            {
                Console.WriteLine("\nInvalid file path");
            }

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }

        private static async Task MakeApiRequest(string imageFilePath)
        {
            try
            {
                using (var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscriptionKey))
                    {Endpoint = uriBase})
                {
                    var result =
                        await client.RecognizePrintedTextInStreamWithHttpMessagesAsync(true,
                            File.OpenRead(imageFilePath));
                    Console.WriteLine(result.Body.GetText());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        public static string GetText(this OcrResult result)
        {
            var builder = new StringBuilder();

            foreach (var region in result.Regions)
            {
                foreach (var line in region.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        builder.Append(word.Text);
                        builder.Append(" ");
                    }
                    builder.AppendLine();
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
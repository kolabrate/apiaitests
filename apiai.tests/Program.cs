using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiAiSDK;
using ApiAiSDK.Model;

namespace apiai.tests
{
    class Program
    {

        private static string accessToken = "e0cdd76a123a48f194e626e15af15c02";
        private static ApiAi apiAi;
        private static string input = "Can I make an appointment tomorrow at 9 AM for a haircut?";
        static void Main(string[] args)
        {
            try
            {
                RunAsync().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.Read();
                
            }
           

        }

        private static async Task RunAsync()
        {
           var response = await GetResponse();
            Console.WriteLine(response.Result.Fulfillment.Speech);
            Console.Read();
        }

        private static async Task<AIResponse> GetResponse()
        {
            
            var config = new AIConfiguration(accessToken, SupportedLanguage.English);
            var dataService = new AIDataService(config);
            var request = new AIRequest(input);
            request.OriginalRequest = PopulateOriginalRequest();
            var response = await dataService.RequestAsync(request);
           
            if (response.Status.Code == 200)
            {

                return response;
            }
            else
            {
                return null;
            }
        }

        private static OriginalRequest PopulateOriginalRequest()
        {
            
            return new OriginalRequest()
            {
                Data = new
                {
                    ApiVersion = "",
                    SmsSid = "",
                    SmsStatus = "",
                    SmsMessageSid = "",
                    NumSegments = "",
                    ToState = "",
                    From = "+61400000000", //a temp phone number that represents its from console app.
                    MessageSid = "SMf0000000000000000000000",//a rep of console.
                    AccountSid = "",
                    ToCity = "",
                    FromCountry = "",
                    ToZip = "",
                    FromCity = "",
                    To = "",
                    FromZip = "",
                    ToCountry = "",
                    Body = input,
                    NumMedia = "",
                    FromState = ""
                },
                Source = "console"
            };

        }
    }
}

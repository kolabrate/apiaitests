using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private static AIDataService dataService;
        static void Main(string[] args)
        {
            try
            {
                Initialise();
                do
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("user:");
                    UserQuery = Console.ReadLine();
                    if (!string.IsNullOrEmpty(UserQuery))
                    {
                        ProcessUserQueryAsync().Wait();
                        Console.ForegroundColor = IsError == true
                            ? ConsoleColor.Red
                            : ConsoleColor.DarkGreen;
                        Console.Write($"Booka:{BookaResponse} \n");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"Booka:Please enter a message(or) enter 'end' to end the chat \n");
                    }
                    
                    
                    
                } while (UserQuery != "end");
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.Read();
                
            }

        }


        private static async Task ProcessUserQueryAsync()
        {
            try
            {
                var response = await GetResponse(UserQuery);
                if (response.IsError == false)
                {
                    BookaResponse = response.Result.Fulfillment.Speech;
                }
                else
                {
                    IsError = true;
                    BookaResponse = "Sorry something went wrong, please try again later.";
                }
            }
            catch (Exception)
            {
                //resetContexts();
                IsError = true;
                BookaResponse = "Sorry something went wrong, please try again later.";
            }
           
            // BookaResponse = response.Result.Fulfillment.Speech;
            
        }

        private static async Task<AIResponse> GetResponse(string userQuery)
        {
            var request = new AIRequest(userQuery);
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


        #region private methods to populate object

        private static void Initialise()
        {
            var config = new AIConfiguration(accessToken, SupportedLanguage.English);
            dataService = new AIDataService(config);
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
                    Body = UserQuery,
                    NumMedia = "",
                    FromState = ""
                },
                Source = "console"
            };

        }
        private static string UserQuery { get; set; }
        private static string BookaResponse { get; set; }
        private static bool IsError { get; set; }


        #endregion
    }
}

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

        private static string accessToken = "787e0b5d351b4a06ba126e8ba0121a0c";
        private static ApiAi apiAi;
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
            await GetResponse();
        }

        private static async Task<AIResponse> GetResponse()
        {
            var config = new AIConfiguration(accessToken, SupportedLanguage.English);
            apiAi = new ApiAi(config);
            var response = await apiAi.TextRequestAsync("Can I make an appoinment");
            if (response.Status.Code == 200)
            {

                return response;
            }
            else
            {
                return null;
            }
        }
    }
}

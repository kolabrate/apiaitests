using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiAiSDK;
using ApiAiSDK.Model;
using Newtonsoft.Json;

namespace Chuvy.BotService.Test.apiai
{
    public class AiDataServiceExtn : AIDataService, IDisposable
    {
        private readonly AIConfiguration config;
        private string SessionId { get; }
        public AiDataServiceExtn(AIConfiguration config) : base(config)
        {
            this.config = config;
            if (string.IsNullOrEmpty(config.SessionId))
                this.SessionId = Guid.NewGuid().ToString();
            else
                this.SessionId = config.SessionId;

        }

        public async Task<AIResponse> RequestAsync(AIRequest request, HttpClient webClient)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                string data = JsonConvert.SerializeObject((object)request, Formatting.None, settings);
                HttpResponseMessage response = await webClient.PostAsync(new Uri(config.RequestUrl), new StringContent(data));
                return (AIResponse) JsonConvert.DeserializeObject(response.Content.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        public void Dispose()
        {

           
        }

      
    }


}

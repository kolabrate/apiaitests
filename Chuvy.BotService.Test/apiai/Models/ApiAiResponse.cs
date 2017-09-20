using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiAiSDK.Model;
using Newtonsoft.Json;

namespace Chuvy.BotService.Test.apiai.Models
{
    [JsonObject]
    public class ApiAiResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("sessionId")]
        public string SessionId { get; set; }

        public bool IsError
        {
            get
            {
                if (this.Status != null && this.Status.Code.HasValue)
                {
                    int? code = this.Status.Code;
                    int num = 400;
                    if ((code.GetValueOrDefault() >= num ? (code.HasValue ? 1 : 0) : 0) != 0)
                        return true;
                }
                return false;
            }
        }
    }
}

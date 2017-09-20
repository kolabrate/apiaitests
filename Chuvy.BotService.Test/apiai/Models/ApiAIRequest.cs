using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiAiSDK;
using ApiAiSDK.Model;
using Newtonsoft.Json;

namespace Chuvy.BotService.Test.apiai.Models
{
    [JsonObject]
    public class ApiAIRequest : ApiAiMetadata
    {
        [JsonProperty("query")]
        public string[] Query { get; set; }

        [JsonProperty("confidence")]
        public float[] Confidence { get; set; }

        [JsonProperty("contexts")]
        public List<AIContext> Contexts { get; set; }

        [JsonProperty("resetContexts")]
        public bool? ResetContexts { get; set; }

        [JsonProperty("originalRequest")]
        public OriginalRequest OriginalRequest { get; set; }

        public ApiAIRequest()
        {
        }

        public ApiAIRequest(string text)
        {
            this.Query = new string[1] { text };
            this.Confidence = new float[1] { 1f };
        }

    }
}

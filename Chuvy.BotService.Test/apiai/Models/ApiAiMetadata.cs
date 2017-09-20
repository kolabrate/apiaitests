using System.Collections.Generic;
using ApiAiSDK.Model;
using Newtonsoft.Json;

namespace Chuvy.BotService.Test.apiai.Models
{
    [JsonObject]
    public class ApiAiMetadata
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("lang")]
        public string Language { get; set; }

        [JsonProperty("sessionId")]
        internal string SessionId { get; set; }

        [JsonProperty("entities")]
        public List<Entity> Entities { get; set; }
    }
}

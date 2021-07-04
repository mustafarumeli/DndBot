using Newtonsoft.Json;

namespace DndBot.Abstraction
{
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get;  set; }

        [JsonProperty("prefix")]
        public string CommandPrefix { get;  set; }
    }
}

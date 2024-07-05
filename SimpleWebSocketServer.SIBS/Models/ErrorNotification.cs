using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Models
{
    public class ErrorNotification
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Version Version { get; set; } = Version.V_1;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.ERROR_NOTIFICATION;

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("messagePT")]
        public string MessagePT { get; set; }
    }
}

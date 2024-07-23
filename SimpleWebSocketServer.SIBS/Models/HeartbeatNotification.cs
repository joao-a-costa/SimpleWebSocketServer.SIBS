using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class HeartbeatNotification
    {
        [JsonProperty("appid")]
        public string Appid { get; set; }
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.SET_AUTH_CREDENTIAL_REQUEST;
        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.Enums.Version Version { get; set; } = Enums.Enums.Version.V_1;
    }
}

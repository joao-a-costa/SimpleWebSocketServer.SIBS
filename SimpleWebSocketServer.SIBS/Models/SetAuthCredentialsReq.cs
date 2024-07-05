using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class SetAuthCredentialsReq
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Version Version { get; set; } = Version.V_1;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.SET_AUTH_CREDENTIAL_REQUEST;

        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }

        [JsonProperty("merchantAuthId")]
        public string MerchantAuthId { get; set; }

        [JsonProperty("merchantAuthKey")]
        public string MerchantAuthKey { get; set; }
    }
}

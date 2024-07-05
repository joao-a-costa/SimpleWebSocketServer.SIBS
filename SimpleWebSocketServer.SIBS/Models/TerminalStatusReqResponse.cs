using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Models
{
    public class TerminalStatusReqResponse
    {
        [JsonProperty("deviceSerialNumber")]
        public string DeviceSerialNumber { get; set; }

        [JsonProperty("hasCredentials")]
        public bool HasCredentials { get; set; }

        [JsonProperty("merchantId")]
        public int MerchantId { get; set; }

        [JsonProperty("sdkId")]
        public string SdkId { get; set; }

        [JsonProperty("sdkVersion")]
        public string SdkVersion { get; set; }

        [JsonProperty("terminalId")]
        public int TerminalId { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.STATUS_RESPONSE;

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}

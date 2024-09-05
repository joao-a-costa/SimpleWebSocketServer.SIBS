using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class PairingReq
    {
        [JsonProperty("pairingStep")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PairingStep PairingStep { get; set; } = PairingStep.GENERATE_PAIRING_CODE;

        [JsonProperty("pairingCode")]
        public string PairingCode { get; set; }

        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Version Version { get; set; } = Version.V_1;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.PAIRING_REQUEST;
    }
}

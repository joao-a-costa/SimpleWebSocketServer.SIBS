using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class PairingNotification
    {
        private const string _pairingStatusStringSuccess = "SUCCESS";

        [JsonProperty("pairingStatus")]
        public string PairingStatusString{ get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonIgnore]
        public bool PairingStatus => PairingStatusString == _pairingStatusStringSuccess;

        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Version Version { get; set; } = Version.V_1;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.PAIRING_RESPONSE;
    }
}

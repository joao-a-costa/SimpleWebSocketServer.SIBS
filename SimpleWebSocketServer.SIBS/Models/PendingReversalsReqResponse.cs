using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class PendingReversalsReqResponse
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Version Version { get; set; } = Version.V_1;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.PENDING_REVERSALS_RESPONSE;

        [JsonProperty("pendingReversals")]
        public List<PendingReversal> PendingReversals { get; set; }
    }
}

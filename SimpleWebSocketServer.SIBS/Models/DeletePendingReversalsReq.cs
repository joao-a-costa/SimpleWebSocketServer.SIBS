﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class DeletePendingReversalsReq
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Version Version { get; set; } = Version.V_1;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.DELETE_PENDING_REVERSALS_REQUEST;

        [JsonProperty("reversalId")]
        public string ReversalId { get; set; }
    }
}

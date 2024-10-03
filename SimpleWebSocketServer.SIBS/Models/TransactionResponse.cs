using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class TransactionResponse
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; } = Guid.NewGuid().ToString().Substring(0, 20);

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.TX_RESPONSE;

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}

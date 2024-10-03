using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class ProcessPaymentReqResponse
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Version Version { get; set; } = Version.V_1;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.PROCESS_PAYMENT_RESPONSE;

        [JsonProperty("paymentData")]
        public PaymentData PaymentData { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; } = System.Guid.NewGuid().ToString().Substring(0, 20);
    }
}

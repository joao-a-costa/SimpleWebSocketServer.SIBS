using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class RefundReq
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Version Version { get; set; } = Version.V_1;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.REFUND_REQUEST;

        [JsonProperty("originalTransactionData")]
        public OriginalTransactionData OriginalTransactionData { get; set; }

        [JsonProperty("refundTransactionId")]
        public string RefundTransactionId { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }
    }

    public class OriginalTransactionData
    {
        [JsonProperty("authorizationType")]
        public string AuthorizationType { get; set; }

        [JsonProperty("paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty("transactionType")]
        public string TransactionType { get; set; }

        [JsonProperty("serverId")]
        public string ServerId { get; set; }

        [JsonProperty("sdkId")]
        public string SdkId { get; set; }

        [JsonProperty("serverDateTime")]
        public string ServerDateTime { get; set; }

        [JsonProperty("dccInitiatorId")]
        public string DccInitiatorId { get; set; }
    }
}

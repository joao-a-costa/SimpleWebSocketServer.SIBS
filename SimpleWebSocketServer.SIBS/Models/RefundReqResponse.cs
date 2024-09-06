using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class RefundReqResponse
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Version Version { get; set; } = Version.V_1;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.REFUND_REQUEST;

        [JsonProperty("refundData")]
        public RefundData RefundData { get; set; }
    }

    public class RefundData
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("errorCode")]
        public double ErrorCode { get; set; }

        [JsonProperty("errorCodeExtension")]
        public string ErrorCodeExtension { get; set; }

        [JsonProperty("gratuityAmount")]
        public double GratuityAmount { get; set; }

        [JsonProperty("periodId")]
        public int PeriodId { get; set; }

        [JsonProperty("receiptTextsData")]
        public ReceiptTextsData ReceiptTextsData { get; set; }

        [JsonProperty("shouldShowAmountToCardholder")]
        public bool ShouldShowAmountToCardholder { get; set; }

        [JsonProperty("terminalCode")]
        public int TerminalCode { get; set; }

        [JsonProperty("wasOnline")]
        public bool WasOnline { get; set; }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class ConfigTerminalReq
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Version Version { get; set; } = Version.V_1;
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.CONFIG_TERMINAL_REQUEST;
        [JsonProperty("reconciliationReceiptPrintMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PrintMode ReconciliationReceiptPrintMode { get; set; } = PrintMode.MODE_A;
        [JsonProperty("refundReceiptPrintMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PrintMode RefundReceiptPrintMode { get; set; } = PrintMode.MODE_A;
        [JsonProperty("saleReceiptPrintMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PrintMode SaleReceiptPrintMode { get; set; } = PrintMode.MODE_A;
        [JsonProperty("receiptFormat")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReceiptFormat ReceiptFormat { get; set; } = ReceiptFormat.TWENTY_COLUMNS;
        [JsonProperty("systemTimeoutMs")]
        public int SystemTimeoutMs { get; set; }
        [JsonProperty("screenTimeoutMs")]
        public int ScreenTimeoutMs { get; set; }
        [JsonProperty("notifyMerchantPeriodMs")]
        public int NotifyMerchantPeriodMs { get; set; }
    }
}

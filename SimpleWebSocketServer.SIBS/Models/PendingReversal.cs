using Newtonsoft.Json;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class PendingReversal
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("dateTime")]
        public string DateTime { get; set; }

        [JsonProperty("sdkId")]
        public string SdkId { get; set; }

        [JsonProperty("reversalId")]
        public string ReversalId { get; set; }

        [JsonProperty("periodId")]
        public string PeriodId { get; set; }

        [JsonProperty("paymentType")]
        public string PaymentType { get; set; }
    }
}

using Newtonsoft.Json;

namespace SimpleWebSocketServer.SIBS.Models
{
    public class ReceiptTextsData
    {
        [JsonProperty("acquirerText")]
        public string AcquirerText { get; set; }
    }
}

using Newtonsoft.Json;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class ReceiptTextsData
    {
        [JsonProperty("acquirerText")]
        public string AcquirerText { get; set; }
    }
}

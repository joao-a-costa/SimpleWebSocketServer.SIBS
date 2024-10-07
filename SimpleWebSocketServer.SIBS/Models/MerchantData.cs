using Newtonsoft.Json;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class MerchantData
    {
        [JsonProperty("acceptorLocation")]
        public string AcceptorLocation { get; set; } = string.Empty;

        [JsonProperty("acceptorName")]
        public string AcceptorName { get; set; } = string.Empty;

        [JsonProperty("fiscalNumber")]
        public string FiscalNumber { get; set; } = string.Empty;

        [JsonProperty("merchantName")]
        public string MerchantName { get; set; } = string.Empty;

        [JsonProperty("terminalName")]
        public string TerminalName { get; set; } = string.Empty;

        [JsonProperty("acceptorAddress")]
        public string AcceptorAddress { get; set; } = string.Empty;
    }
}

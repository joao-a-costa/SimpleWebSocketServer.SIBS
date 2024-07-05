using Newtonsoft.Json;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class CardData
    {
        [JsonProperty("appLabel")]
        public string AppLabel { get; set; }

        [JsonProperty("appid")]
        public string Appid { get; set; }

        [JsonProperty("bin")]
        public string Bin { get; set; }

        [JsonProperty("cardBalance")]
        public string CardBalance { get; set; }

        [JsonProperty("expirityDate")]
        public string ExpirityDate { get; set; }

        [JsonProperty("financialProductDescLong")]
        public string FinancialProductDescLong { get; set; }

        [JsonProperty("financialProductDescMedium")]
        public string FinancialProductDescMedium { get; set; }

        [JsonProperty("financialProductDescShort")]
        public string FinancialProductDescShort { get; set; }

        [JsonProperty("issuerName")]
        public string IssuerName { get; set; }

        [JsonProperty("maskedPAN")]
        public string MaskedPAN { get; set; }

        [JsonProperty("trackData")]
        public string TrackData { get; set; }
    }
}

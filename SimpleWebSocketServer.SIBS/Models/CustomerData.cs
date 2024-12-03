using Newtonsoft.Json;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class CustomerData
    {
        [JsonProperty("fiscalNumber")]
        public string FiscalNumber { get; set; }
        [JsonProperty("fiscalNumberCountryISO2Code")]
        public string FiscalNumberCountryISO2Code { get; set; }
    }
}
using System;
using Newtonsoft.Json;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class PaymentData
    {
        [JsonProperty("apc")]
        public string Apc { get; set; }

        [JsonProperty("atc")]
        public string Atc { get; set; }

        [JsonProperty("cardData")]
        public CardData CardData { get; set; }

        [JsonProperty("clientFee")]
        public string ClientFee { get; set; }

        [JsonProperty("exchangeRate")]
        public double ExchangeRate { get; set; }

        [JsonProperty("paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("tsi")]
        public string Tsi { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("appId")]
        public string AppId { get; set; }

        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("errorCodeExtension")]
        public string ErrorCodeExtension { get; set; }

        [JsonProperty("gratuityAmount")]
        public double GratuityAmount { get; set; }

        [JsonProperty("periodId")]
        public int PeriodId { get; set; }

        [JsonProperty("receiptTextsData")]
        public ReceiptTextsData ReceiptTextsData { get; set; }

        [JsonProperty("sdkDateTime")]
        public DateTime SdkDateTime { get; set; }

        [JsonProperty("sdkId")]
        public string SdkId { get; set; }

        [JsonProperty("serverDateTime")]
        public DateTime ServerDateTime { get; set; }

        [JsonProperty("serverId")]
        public string ServerId { get; set; }

        [JsonProperty("shouldShowAmountToCardholder")]
        public bool ShouldShowAmountToCardholder { get; set; }

        [JsonProperty("terminalCode")]
        public int TerminalCode { get; set; }

        [JsonProperty("transactionType")]
        public string TransactionType { get; set; }

        [JsonProperty("wasOnline")]
        public bool WasOnline { get; set; }
    }
}

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class ReconciliationReqResponse
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.Enums.Version Version { get; set; } = Enums.Enums.Version.V_1;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.RECONCILIATION_RESPONSE;

        [JsonProperty("reconciliationData")]
        public ReconciliationData ReconciliationData { get; set; }

    }

    public class AdditionalContentList
    {
        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class FinancialTotalItem
    {
        [JsonProperty("commissionTotal")]
        public double CommissionTotal { get; set; }

        [JsonProperty("nrTotalTransactions")]
        public double NrTotalTransactions { get; set; }

        [JsonProperty("totalPaymentAmount")]
        public double TotalPaymentAmount { get; set; }
    }

    public class MerchantTotals
    {
        [JsonProperty("nrTotalTransactions")]
        public double NrTotalTransactions { get; set; }

        [JsonProperty("paymentName")]
        public string PaymentName { get; set; }

        [JsonProperty("totalPaymentAmount")]
        public double TotalPaymentAmount { get; set; }

        [JsonProperty("totalPaymentsCurrency")]
        public string TotalPaymentsCurrency { get; set; }
    }

    public class OperationTotalItem
    {
        [JsonProperty("nrTotalTransactions")]
        public double NrTotalTransactions { get; set; }

        [JsonProperty("paymentName")]
        public string PaymentName { get; set; }

        [JsonProperty("totalPaymentAmount")]
        public double TotalPaymentAmount { get; set; }

        [JsonProperty("totalPaymentsCurrency")]
        public string TotalPaymentsCurrency { get; set; }
    }

    public class ReconciliationData
    {
        [JsonProperty("additionalContentList")]
        public List<AdditionalContentList> AdditionalContentList { get; set; }

        [JsonProperty("creditReverseTotalItems")]
        public List<object> CreditReverseTotalItems { get; set; }

        [JsonProperty("creditTotalItems")]
        public List<object> CreditTotalItems { get; set; }

        [JsonProperty("debitReversalTotalItems")]
        public List<object> DebitReversalTotalItems { get; set; }

        [JsonProperty("debitTotalItems")]
        public List<object> DebitTotalItems { get; set; }

        [JsonProperty("financialTotalItems")]
        public List<FinancialTotalItem> FinancialTotalItems { get; set; }

        [JsonProperty("merchantTotals")]
        public MerchantTotals MerchantTotals { get; set; }

        [JsonProperty("operationTotalItems")]
        public List<OperationTotalItem> OperationTotalItems { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("appId")]
        public string AppId { get; set; }

        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

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

        [JsonProperty("wasOnline")]
        public bool WasOnline { get; set; }
    }
}

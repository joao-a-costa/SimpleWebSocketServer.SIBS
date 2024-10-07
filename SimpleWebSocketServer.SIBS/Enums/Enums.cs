using System.ComponentModel;
using System.Diagnostics;

namespace SimpleWebSocketServer.SIBS.Lib.Enums
{
    public class Enums
    {
        public enum RequestType
        {
            STATUS_REQUEST,
            STATUS_RESPONSE,
            SET_AUTH_CREDENTIAL_REQUEST,
            SET_AUTH_CREDENTIAL_RESPONSE,
            PROCESS_PAYMENT_REQUEST,
            PROCESS_PAYMENT_RESPONSE,
            EVENT_NOTIFICATION,
            ERROR_NOTIFICATION,
            HEARTBEAT_NOTIFICATION,
            TX_REQUEST,
            TX_RESPONSE,
            RECEIPT_NOTIFICATION,
            PAIRING_REQUEST,
            PAIRING_RESPONSE,
            PAIRING_NOTIFICATION,
            RECONCILIATION_REQUEST,
            RECONCILIATION_RESPONSE,
            REFUND_REQUEST,
            REFUND_RESPONSE,
            COMMUNICATIONS_REQUEST,
            COMMUNICATIONS_RESPONSE,
            GET_MERCHANT_DATA_REQUEST,
            GET_MERCHANT_DATA_RESPONSE,
            SET_MERCHANT_DATA_REQUEST,
            SET_MERCHANT_DATA_RESPONSE,
            CONFIG_TERMINAL_REQUEST,
            CONFIG_TERMINAL_RESPONSE,
        }

        public enum AuthorizationType
        {
            NA,
            GENERAL,
            AFD,
            EVC,
            FUEL
        }

        public enum PairingStep
        {
            GENERATE_PAIRING_CODE,
            VALIDATE_PAIRING_CODE,
            CANCEL_PAIRING
        }

        public enum Version
        {
            V_1
        }

        public enum PrintMode
        {
            [Description("MODE_A")]
            MODE_A,
            [Description("MODE_B")]
            MODE_B,
            [Description("MODE_C")]
            MODE_C,
        }

        public enum ReceiptFormat
        {
            [Description("TWENTY_COLUMNS")]
            TWENTY_COLUMNS,
            [Description("FORTY_COLUMNS")]
            FORTY_COLUMNS,
        }

        public enum TerminalCommandOptions
        {
            [Description("Send terminal status request")]
            SendTerminalStatusRequest = 1,
            [Description("Send process payment request")]
            SendProcessPaymentRequest = 2,
            [Description("Send pairing request")]
            SendPairingRequest = 3,
            [Description("Send pairing request cancel")]
            SendPairingRequestCancel = 4,
            [Description("Send refund payment request")]
            SendRefundPaymentRequest = 5,
            [Description("Send reconciliation request")]
            SendReconciliationRequest = 6,
            [Description("Send communication status request")]
            SendCommunicationStatusRequest = 7,
            [Description("Send get merchant data request")]
            SendGetMerchantDataRequest = 8,
            [Description("Send set merchant data request")]
            SendSetMerchantDataRequest = 9,
            [Description("Send configuration terminal request")]
            SendConfigTerminalRequest = 10,
            [Description("Show list of commands")]
            ShowListOfCommands = 9998,
            [Description("Stop the server")]
            StopTheServer = 9999
        }
    }
}

﻿using System.ComponentModel;
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
            REFUND_RESPONSE
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

        public enum TerminalCommandOptions
        {
            [Description("Send terminal status request")]
            SendTerminalStatusRequest = 1,
            [Description("Send process payment request")]
            SendProcessPaymentRequest = 2,
            [Description("Send pairing request")]
            SendPairingRequest = 3,
            [Description("Send refund payment request")]
            SendRefundPaymentRequest = 4,
            [Description("Show list of commands")]
            ShowListOfCommands = 9998,
            [Description("Stop the server")]
            StopTheServer = 9999
        }
    }
}

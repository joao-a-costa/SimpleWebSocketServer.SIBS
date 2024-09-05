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
            RECONCILIATION_RESPONSE
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
    }
}

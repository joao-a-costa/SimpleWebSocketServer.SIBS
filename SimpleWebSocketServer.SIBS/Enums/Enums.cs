namespace SimpleWebSocketServer.SIBS.Enums
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
            ERROR_NOTIFICATION
        }

        public enum Version
        {
            V_1
        }
    }
}

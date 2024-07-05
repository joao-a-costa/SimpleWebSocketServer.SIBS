using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleWebSocketServer.SIBS.Models;

namespace SimpleWebSocketServer.SIBS.Console
{
    internal static class Program
    {
        private static WebSocketServerSibs server;

        static void Main()
        {
            server = new WebSocketServerSibs();
            server.ClientConnected += Server_ClientConnected;
            server.TerminalStatusReqResponseReceived += Server_TerminalStatusReqResponseReceived;
            server.SetAuthCredentialsReqReceived += Server_SetAuthCredentialsReqReceived;
            server.EventNotificationReceived += Server_EventNotificationReceived;
            server.ProcessPaymentReqReceived += Server_ProcessPaymentReqReceived;
            server.ErrorNotificationReceived += Server_ErrorNotificationReceived;
            server.Start("http://+:10005/");

        }

        private async static void Server_ClientConnected(object sender, System.EventArgs e)
        {
            await server.SendMessageToClient(JsonConvert.SerializeObject(
                new TerminalStatusReq()));
        }

        private async static void Server_TerminalStatusReqResponseReceived(object sender, TerminalStatusReqResponse reqResponse)
        {
            if (reqResponse != null && !reqResponse.HasCredentials)
            {
                // Perform credentials
                await server.SendMessageToClient(JsonConvert.SerializeObject(
                    JsonConvert.DeserializeObject<SetAuthCredentialsReq>(
                        System.Text.Encoding.UTF8.GetString(
                    Properties.Resources.credentials))));
            }
            else
            {
#if DEBUG
                // Perform transaction
                
                await server.SendMessageToClient(JsonConvert.SerializeObject(
                    new ProcessPaymentReq
                    {
                        AmountData = new AmountData
                        {
                            Amount = 99.98,
                        }
                    }));
#endif
            }
        }

        private static void Server_SetAuthCredentialsReqReceived(object sender, SetAuthCredentialsReqResponse reqResponse)
        {
            // To be implemented
        }

        private static void Server_ProcessPaymentReqReceived(object sender, ProcessPaymentReqResponse reqResponse)
        {
            // To be implemented
        }

        private static void Server_EventNotificationReceived(object sender, EventNotification reqResponse)
        {
            // To be implemented
        }

        private static void Server_ErrorNotificationReceived(object sender, ErrorNotification reqResponse)
        {
            // To be implemented
        }
    }
}

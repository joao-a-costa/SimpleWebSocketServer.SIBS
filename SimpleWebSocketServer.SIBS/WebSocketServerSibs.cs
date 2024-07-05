using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleWebSocketServer.SIBS.Models;

namespace SimpleWebSocketServer.SIBS
{
    public class WebSocketServerSibs
    {
        private WebSocketServer server;

        public delegate void ClientConnectedEventHandler(object sender, EventArgs e);
        public delegate void TerminalStatusReqResponseReceivedEventHandler(object sender, TerminalStatusReqResponse reqResponse);
        public delegate void SetAuthCredentialsReqResponseEventHandler(object sender, SetAuthCredentialsReqResponse reqResponse);
        public delegate void ProcessPaymentReqResponseEventHandler(object sender, ProcessPaymentReqResponse reqResponse);
        public delegate void EventNotificationEventHandler(object sender, EventNotification reqResponse);
        public delegate void ErrorNotificationEventHandler(object sender, ErrorNotification reqResponse);

        public event ClientConnectedEventHandler ClientConnected;
        public event TerminalStatusReqResponseReceivedEventHandler TerminalStatusReqResponseReceived;
        public event SetAuthCredentialsReqResponseEventHandler SetAuthCredentialsReqReceived;
        public event ProcessPaymentReqResponseEventHandler ProcessPaymentReqReceived;
        public event EventNotificationEventHandler EventNotificationReceived;
        public event ErrorNotificationEventHandler ErrorNotificationReceived;

        #region "Public methods"

        /// <summary>
        /// Start the WebSocket server
        /// </summary>
        /// <param name="prefix">The prefix to listen to</param>
        public async Task StartAsync(string prefix)
        {
            // Create an instance of WebSocketServer
            server = new WebSocketServer(prefix);

            try
            {
                // Define events for server lifecycle
                server.ServerStarted += Server_ServerStarted;
                server.ClientConnected += Server_ClientConnected;
                server.MessageReceived += Server_MessageReceived;

                // Start the WebSocket server
                await server.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        public async Task SendMessageToClient(string message)
        {
            await server.SendMessageToClient(message);
        }

        #endregion

        #region "Private methods"

        private void OnClientConnected()
        {
            ClientConnected?.Invoke(this, new EventArgs());
        }

        private void Server_MessageReceived(object sender, string e)
        {
            try
            {
                // Try to parse the message as a TerminalStatusReqResponse object
                var result = JsonConvert.DeserializeObject<TerminalStatusReqResponse>(e);

                if (result != null)
                {
                    
                    switch (result.Type)
                    {
                        case Enums.Enums.RequestType.STATUS_RESPONSE:
                            OnTerminalStatusReqResponseReceived(result);
                            break;
                        case Enums.Enums.RequestType.SET_AUTH_CREDENTIAL_RESPONSE:
                            OnSetAuthCredentialsReqReceived(JsonConvert
                                .DeserializeObject<SetAuthCredentialsReqResponse>(e));
                            break;
                        case Enums.Enums.RequestType.EVENT_NOTIFICATION:
                            OnEventNotificationReceived(JsonConvert
                                .DeserializeObject<EventNotification>(e));
                            break;
                        case Enums.Enums.RequestType.PROCESS_PAYMENT_RESPONSE:
                            OnProcessPaymentReqReceived(JsonConvert
                                .DeserializeObject<ProcessPaymentReqResponse>(e));
                            break;
                        case Enums.Enums.RequestType.ERROR_NOTIFICATION:
                            OnErrorNotificationReceived(JsonConvert
                                .DeserializeObject<ErrorNotification>(e));
                            break;
                        default:
                            Console.WriteLine("Received unknown message");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle deserialization errors
                Console.WriteLine($"Error deserializing message: {ex.Message}");
            }
        }

        private void Server_ClientConnected(object sender, string e)
        {
            OnClientConnected();
        }

        private void Server_ServerStarted(object sender, string e)
        {
            // To be implemented
        }

        private void OnTerminalStatusReqResponseReceived(TerminalStatusReqResponse reqResponse)
        {
            TerminalStatusReqResponseReceived?.Invoke(this, reqResponse);
        }

        private void OnSetAuthCredentialsReqReceived(SetAuthCredentialsReqResponse reqResponse)
        {
            SetAuthCredentialsReqReceived?.Invoke(this, reqResponse);
        }

        private void OnProcessPaymentReqReceived(ProcessPaymentReqResponse reqResponse)
        {
            ProcessPaymentReqReceived?.Invoke(this, reqResponse);
        }

        private void OnEventNotificationReceived(EventNotification reqResponse)
        {
            EventNotificationReceived?.Invoke(this, reqResponse);
        }

        private void OnErrorNotificationReceived(ErrorNotification reqResponse)
        {
            ErrorNotificationReceived?.Invoke(this, reqResponse);
        }

        #endregion
    }
}
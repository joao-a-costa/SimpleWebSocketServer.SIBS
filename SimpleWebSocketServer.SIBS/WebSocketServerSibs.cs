using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleWebSocketServer.SIBS.Lib.Models;

namespace SimpleWebSocketServer.SIBS.Lib
{
    public class WebSocketServerSibs
    {
        #region "Fields"

        /// <summary>
        /// The WebSocket server
        /// </summary>
        private WebSocketServer server;

        #endregion

        #region "Events"

        public delegate void ClientConnectedEventHandler(object sender, EventArgs e);
        public delegate void ClientDisconnectedEventHandler(object sender, EventArgs e);
        public delegate void TerminalStatusReqResponseReceivedEventHandler(object sender, TerminalStatusReqResponse reqResponse);
        public delegate void SetAuthCredentialsReqResponseEventHandler(object sender, SetAuthCredentialsReqResponse reqResponse);
        public delegate void ProcessPaymentReqResponseEventHandler(object sender, ProcessPaymentReqResponse reqResponse);
        public delegate void EventNotificationEventHandler(object sender, EventNotification reqResponse);
        public delegate void HeartbeatNotificationEventHandler(object sender, HeartbeatNotification reqResponse);
        public delegate void ErrorNotificationEventHandler(object sender, ErrorNotification reqResponse);

        public event ClientConnectedEventHandler ClientConnected;
        public event ClientDisconnectedEventHandler ClientDisconnected;
        public event TerminalStatusReqResponseReceivedEventHandler TerminalStatusReqResponseReceived;
        public event SetAuthCredentialsReqResponseEventHandler SetAuthCredentialsReqReceived;
        public event ProcessPaymentReqResponseEventHandler ProcessPaymentReqReceived;
        public event EventNotificationEventHandler EventNotificationReceived;
        public event HeartbeatNotificationEventHandler HeartbeatNotificationReceived;
        public event ErrorNotificationEventHandler ErrorNotificationReceived;

        #endregion

        #region "Properties"

        public bool IsStarted => server.IsStarted;

        #endregion

        #region "Public methods"

        /// <summary>
        /// Start the WebSocket server
        /// </summary>
        /// <param name="prefix">The prefix to listen to</param>
        public void Start(string prefix)
        {
            // Create an instance of WebSocketServer
            server = new WebSocketServer(prefix);

            try
            {
                // Define events for server lifecycle
                server.ServerStarted += Server_ServerStarted;
                server.ClientConnected += Server_ClientConnected;
                server.ClientDisconnected += Server_ClientDisconnected;
                server.MessageReceived += Server_MessageReceived;

                // Start the WebSocket server
                server.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Send a message to the client
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns>The task</returns>
        public async Task SendMessageToClient(string message)
        {
            await server.SendMessageToClient(message);
        }

        /// <summary>
        /// Stop the WebSocket server
        /// </summary>
        /// <returns>The task</returns>
        public void Stop()
        {
            server.Stop();
        }   

        #endregion

        #region "Private methods"

        /// <summary>
        /// OnClientConnected event handler
        /// </summary>
        private void OnClientConnected()
        {
            ClientConnected?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// OnClientDisconnected event handler
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The message</param>
        private void Server_ClientDisconnected(object sender, string e)
        {
            ClientDisconnected?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// OnMessageReceived event handler
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The message</param>
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
                        case Enums.Enums.RequestType.TX_REQUEST:
                            server.SendMessageToClient(JsonConvert.SerializeObject(new TransactionResponse()));
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
                        case Enums.Enums.RequestType.HEARTBEAT_NOTIFICATION:
                            OnHeartbeatNotificationReceived(JsonConvert
                                .DeserializeObject<HeartbeatNotification>(e));
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

        /// <summary>
        /// OnClientConnected event handler
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The message</param>
        private void Server_ClientConnected(object sender, string e)
        {
            OnClientConnected();
        }

        /// <summary>
        /// OnServerStarted event handler
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The message</param>
        private void Server_ServerStarted(object sender, string e)
        {
            // To be implemented
        }

        /// <summary>
        /// OnTerminalStatusReqResponseReceived event handler
        /// </summary>
        /// <param name="reqResponse">The response</param>
        private void OnTerminalStatusReqResponseReceived(TerminalStatusReqResponse reqResponse)
        {
            TerminalStatusReqResponseReceived?.Invoke(this, reqResponse);
        }

        /// <summary>
        /// OnSetAuthCredentialsReqReceived event handler
        /// </summary>
        /// <param name="reqResponse">The response</param>
        private void OnSetAuthCredentialsReqReceived(SetAuthCredentialsReqResponse reqResponse)
        {
            SetAuthCredentialsReqReceived?.Invoke(this, reqResponse);
        }

        /// <summary>
        /// OnProcessPaymentReqReceived event handler
        /// </summary>
        /// <param name="reqResponse">The response</param>
        private void OnProcessPaymentReqReceived(ProcessPaymentReqResponse reqResponse)
        {
            ProcessPaymentReqReceived?.Invoke(this, reqResponse);
        }

        /// <summary>
        /// OnEventNotificationReceived event handler
        /// </summary>
        /// <param name="reqResponse">The response</param>
        private void OnEventNotificationReceived(EventNotification reqResponse)
        {
            EventNotificationReceived?.Invoke(this, reqResponse);
        }

        /// <summary>
        /// OnHeartbeatNotificationReceived event handler
        /// </summary>
        /// <param name="reqResponse">The response</param>
        private void OnHeartbeatNotificationReceived(HeartbeatNotification reqResponse)
        {
            HeartbeatNotificationReceived?.Invoke(this, reqResponse);
        }

        /// <summary>
        /// OnErrorNotificationReceived event handler
        /// </summary>
        /// <param name="reqResponse">The response</param>
        private void OnErrorNotificationReceived(ErrorNotification reqResponse)
        {
            ErrorNotificationReceived?.Invoke(this, reqResponse);
        }

        #endregion
    }
}
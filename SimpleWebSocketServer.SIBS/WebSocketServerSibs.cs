﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleWebSocketServer.SIBS.Lib.Models;

namespace SimpleWebSocketServer.SIBS.Lib
{
    public class WebSocketServerSibs
    {
        private const string _MessageReceivedUnknownMessage = "Received unknown message";
        private const string _MessageErrorOccurred = "Error occurred";
        private const string _MessageErrorDeserializingMessage = "Error deserializing message";

        #region "Fields"

        /// <summary>
        /// The WebSocket server
        /// </summary>
        private WebSocketServer server;
        private CancellationTokenSource cancellationTokenSource;
        private Task serverTask;

        #endregion

        #region "Events"

        public delegate void ClientConnectedEventHandler(object sender, EventArgs e);
        public delegate void ClientDisconnectedEventHandler(object sender, EventArgs e);
        public delegate void TerminalStatusReqResponseReceivedEventHandler(object sender, TerminalStatusReqResponse reqResponse);
        public delegate void SetAuthCredentialsReqResponseEventHandler(object sender, SetAuthCredentialsReqResponse reqResponse);
        public delegate void ProcessPaymentReqResponseEventHandler(object sender, ProcessPaymentReqResponse reqResponse);
        public delegate void EventNotificationEventHandler(object sender, EventNotification reqResponse);
        public delegate void HeartbeatNotificationEventHandler(object sender, HeartbeatNotification reqResponse);
        public delegate void ReceiptNotificationEventHandler(object sender, ReceiptNotification reqResponse);
        public delegate void PairingReqEventHandler(object sender, PairingReqResponse reqResponse);
        public delegate void ReconciliationReqEventHandler(object sender, ReconciliationReqResponse reqResponse);
        public delegate void PairingNotificationEventHandler(object sender, PairingNotification reqResponse);
        public delegate void ErrorNotificationEventHandler(object sender, ErrorNotification reqResponse);
        public delegate void RefundReqResponseEventHandler(object sender, RefundReqResponse reqResponse);

        public event ClientConnectedEventHandler ClientConnected;
        public event ClientDisconnectedEventHandler ClientDisconnected;
        public event TerminalStatusReqResponseReceivedEventHandler TerminalStatusReqResponseReceived;
        public event SetAuthCredentialsReqResponseEventHandler SetAuthCredentialsReqReceived;
        public event ProcessPaymentReqResponseEventHandler ProcessPaymentReqReceived;
        public event EventNotificationEventHandler EventNotificationReceived;
        public event HeartbeatNotificationEventHandler HeartbeatNotificationReceived;
        public event ReceiptNotificationEventHandler ReceiptNotificationReceived;
        public event PairingReqEventHandler PairingReqReceived;
        public event ReconciliationReqEventHandler ReconciliationReqReceived;
        public event PairingNotificationEventHandler PairingNotificationReceived;
        public event ErrorNotificationEventHandler ErrorNotificationReceived;
        public event RefundReqResponseEventHandler RefundReqReceived;

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

            // Initialize the cancellation token source
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            // Start the WebSocket server asynchronously
            serverTask = Task.Run(() =>
            {
                try
                {
                    // Define events for server lifecycle
                    server.ServerStarted += Server_ServerStarted;
                    server.ClientConnected += Server_ClientConnected;
                    server.ClientDisconnected += Server_ClientDisconnected;
                    server.MessageReceived += Server_MessageReceived;

                    // Start the WebSocket server
                    server.Start();

                    // Keep the server running until a cancellation is requested
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        // You can add more logic here if needed (e.g., monitor the server)
                        Task.Delay(1000, cancellationToken).Wait(cancellationToken); // Poll every 1 second
                    }
                }
                catch (OperationCanceledException)
                {
                    // This is expected when the task is canceled
                }
                catch (HttpListenerException ex)
                {
                    // Log and handle HttpListener specific exceptions
                    Console.WriteLine($"{_MessageErrorOccurred}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Log any other exceptions
                    Console.WriteLine($"{_MessageErrorOccurred}: {ex.Message}");
                }
            }, cancellationToken);
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
            // Signal the task to cancel
            cancellationTokenSource?.Cancel();

            // Stop the WebSocket server
            if (serverTask != null && !serverTask.IsCompleted)
            {
                server.Stop().Wait();
                serverTask.Wait();  // Optionally wait for the task to complete
            }

            // Dispose of the cancellation token source
            cancellationTokenSource?.Dispose();
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
                            Task.Run(() => server.SendMessageToClient(JsonConvert.SerializeObject(new TransactionResponse()))).Wait();
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
                        case Enums.Enums.RequestType.RECEIPT_NOTIFICATION:
                            OnReceiptNotificationReceived(JsonConvert
                                .DeserializeObject<ReceiptNotification>(e));
                            break;
                        case Enums.Enums.RequestType.PAIRING_RESPONSE:
                            OnPairingReqReceived(JsonConvert
                                .DeserializeObject<PairingReqResponse>(e));
                            break;
                        case Enums.Enums.RequestType.PAIRING_NOTIFICATION:
                            OnPairingNotificationReceived(JsonConvert
                                .DeserializeObject<PairingNotification>(e));
                            break;
                        case Enums.Enums.RequestType.RECONCILIATION_RESPONSE:
                            OnReconciliationReqReceived(JsonConvert
                                .DeserializeObject<ReconciliationReqResponse>(e));
                            break;
                        case Enums.Enums.RequestType.REFUND_RESPONSE:
                            OnRefundReqReceived(JsonConvert
                                .DeserializeObject<RefundReqResponse>(e));
                            break;
                        default:
                            Console.WriteLine(_MessageReceivedUnknownMessage);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle deserialization errors
                Console.WriteLine($"{_MessageErrorDeserializingMessage}: {ex.Message}");
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

        /// <summary>
        /// OnReceiptNotificationReceived event handler
        /// </summary>
        /// <param name="reqResponse">The response</param>
        private void OnReceiptNotificationReceived(ReceiptNotification reqResponse)
        {
            ReceiptNotificationReceived?.Invoke(this, reqResponse);
        }

        /// <summary>
        /// OnPairingReqReceived event handler
        /// </summary>
        /// <param name="reqResponse">The response</param>
        private void OnPairingReqReceived(PairingReqResponse reqResponse)
        {
            PairingReqReceived?.Invoke(this, reqResponse);
        }

        /// <summary>
        /// OnPairingNotificationReceived event handler
        /// </summary>
        /// <param name="reqResponse">The response</param>
        private void OnPairingNotificationReceived(PairingNotification reqResponse)
        {
            PairingNotificationReceived?.Invoke(this, reqResponse);
        }

        /// <summary>
        /// OnReconciliationReqReceived event handler
        /// </summary>
        /// <param name="reqResponse">The response</param>
        private void OnReconciliationReqReceived(ReconciliationReqResponse reqResponse)
        {
            ReconciliationReqReceived?.Invoke(this, reqResponse);
        }

        /// <summary>
        /// OnRefundReqReceived event handler
        /// </summary>
        /// <param name="reqResponse">The response</param>
        private void OnRefundReqReceived(RefundReqResponse reqResponse)
        {
            RefundReqReceived?.Invoke(this, reqResponse);
        }

        #endregion
    }
}
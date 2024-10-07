using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleWebSocketServer.SIBS.Lib;
using SimpleWebSocketServer.SIBS.Lib.Models;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Console
{
    internal static class Program
    {
        #region "Constants"

        private const string _WebSocketServerPrefix = "https://+:10005/";
        private const string _MessageEnterJSONCommand = "Enter 'q' to stop:";
        private const string _MessageErrorErrorOccurred = "Error occurred";
        private const string _MessageErrorProcessingRequest = "Error processing request";
        private const string _MessagePressAnyKeyToExit = "Press any key to exit...";
        private const string _MessageStoppingTheServer = "Stopping the server...";
        private const string _MessageTheFollowingCommandsAreAvailable = "The following commands are available:";
        private const string _MessageEnterCode = "Enter code or 'q' to stop: ";
        private const string _MessageEnterAmount = "Enter amount: ";
        private const string _MessageInvalidInput = "Invalid input";
        private const string _MessageUsingLastSuccessfullTranscationDataForRefund = "Using last successfull transcation data for refund";
        private const string _MessageEnterIBANOrEmptyToUseLastOne = "Enter IBAN or 'enter' to use last one: ";

        #endregion

        #region "Members"

        private static WebSocketServerSibs server;
        private static readonly ManualResetEvent statusEventReceived = new ManualResetEvent(false);
        private static PaymentData LastPaymentData = null;

        #endregion

        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        static void Main()
        {
            // Define the WebSocket server prefix
            string prefix = _WebSocketServerPrefix;

            // Create an instance of WebSocketServer
            server = new WebSocketServerSibs();
            server.ClientConnected += Server_ClientConnected;
            server.TerminalStatusReqResponseReceived += Server_TerminalStatusReqResponseReceived;
            server.PairingReqReceived += Server_PairingReqReceived;
            server.PairingNotificationReceived += Server_PairingNotificationReceived;
            server.ProcessPaymentReqReceived += Server_ProcessPaymentReqReceived;
            server.ErrorNotificationReceived += Server_ErrorNotificationReceived;
            server.ReconciliationReqReceived += Server_ReconciliationReqReceived;
            server.CommunicationsReqReceived += Server_CommunicationsReqReceived;
            server.GetMerchantDataReqReceived += Server_GetMerchantDataReqReceived;
            server.SetMerchantDataReqReceived += Server_SetMerchantDataReqReceived;
            server.ConfigTerminalReqReceived += Server_ConfigTerminalReqReceived;

            try
            {
                System.Console.WriteLine(_MessageEnterJSONCommand);

                // Start the WebSocket server asynchronously
                server.Start(prefix);

                // Listen for user input asynchronously
                ListenForUserInput();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorErrorOccurred}: {ex.Message}");
            }
            finally
            {
                // Ensure to stop the server if it's running
                if (server.IsStarted)
                {
                    server.Stop();
                }
            }

            System.Console.WriteLine(_MessagePressAnyKeyToExit);
            System.Console.ReadKey();
        }

        #region "Private Methods"

        /// <summary>
        /// Listens for user input and sends the input to the WebSocket server.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static void ListenForUserInput()
        {
            var serverIsRunning = true;

            while (serverIsRunning)
            {
                // Read user input synchronously
                string input = System.Console.ReadLine();

                // Check if the input is 'q' to stop the server
                if (input.ToLower() == "q")
                {
                    if (server != null && server.IsStarted)
                    {
                        System.Console.WriteLine(_MessageStoppingTheServer);
                        server.Stop(); // Stop the WebSocket server
                    }
                    break; // Exit the loop
                }

                // Parse the input to the enum
                if (int.TryParse(input, out int commandValue) && Enum.IsDefined(typeof(TerminalCommandOptions), commandValue))
                {
                    var command = (TerminalCommandOptions)commandValue;

                    switch (command)
                    {
                        case TerminalCommandOptions.SendTerminalStatusRequest:
                            SendTerminalStatusRequest().Wait();
                            break;
                        case TerminalCommandOptions.SendProcessPaymentRequest:
                            SendProcessPaymentRequest().Wait();
                            WaitForEvent(statusEventReceived);
                            break;
                        case TerminalCommandOptions.SendPairingRequest:
                            SendPairingRequest().Wait();
                            WaitForEvent(statusEventReceived);
                            break;
                        case TerminalCommandOptions.SendPairingRequestCancel:
                            SendPairingRequestCancel().Wait();
                            WaitForEvent(statusEventReceived);
                            break;
                        case TerminalCommandOptions.SendRefundPaymentRequest:
                            SendRefundPaymentRequest().Wait();
                            WaitForEvent(statusEventReceived);
                            break;
                        case TerminalCommandOptions.SendReconciliationRequest:
                            SendReconciliationRequest().Wait();
                            WaitForEvent(statusEventReceived);
                            break;
                        case TerminalCommandOptions.SendCommunicationStatusRequest:
                            SendCommunicationRequest().Wait();
                            WaitForEvent(statusEventReceived);
                            break;
                        case TerminalCommandOptions.SendGetMerchantDataRequest:
                            SendGetMerchantDataRequest(new GetMerchantDataReq()).Wait();
                            WaitForEvent(statusEventReceived);
                            break;
                        case TerminalCommandOptions.SendSetMerchantDataRequest:
                            SendSetMerchantDataRequest(new MerchantData()).Wait();
                            WaitForEvent(statusEventReceived);
                            break;
                        case TerminalCommandOptions.SendConfigTerminalRequest:
                            SendConfigTerminalRequest(new ConfigTerminalReq()).Wait();
                            WaitForEvent(statusEventReceived);
                            break;
                        case TerminalCommandOptions.ShowListOfCommands:
                            ShowListOfCommands();
                            break;
                        case TerminalCommandOptions.StopTheServer:
                            if (server != null && server.IsStarted)
                            {
                                System.Console.WriteLine(_MessageStoppingTheServer);
                                server.Stop(); // Stop the WebSocket server
                            }
                            serverIsRunning = false;
                            break;
                    }
                }
                else
                {
                    System.Console.WriteLine(_MessageInvalidInput);
                    ShowListOfCommands();
                }
            }
        }

        /// <summary>
        /// Shows the list of commands.
        /// </summary>
        private static void ShowListOfCommands()
        {
            System.Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}{_MessageTheFollowingCommandsAreAvailable}");

            foreach (TerminalCommandOptions command in Enum.GetValues(typeof(TerminalCommandOptions)))
            {
                System.Console.WriteLine($"   {(int)command} - {Lib.Utilities.UtilitiesSibs.GetEnumDescription(command)}");
            }

            System.Console.WriteLine($"{Environment.NewLine}");
        }

        /// <summary>
        /// Event wait handler
        /// </summary>
        /// <param name="eventHandle">The event handle</param>
        /// <param name="actionName">The action name</param>
        private static void WaitForEvent(ManualResetEvent eventHandle)
        {
            eventHandle.WaitOne();
            eventHandle.Reset();
        }

        private static void Server_ErrorNotificationReceived(object sender, ErrorNotification reqResponse)
        {
            //throw new NotImplementedException();
        }

        private static void Server_ProcessPaymentReqReceived(object sender, ProcessPaymentReqResponse reqResponse)
        {
            LastPaymentData = reqResponse.PaymentData;
        }

        #endregion

        #region "Server Events"

        /// <summary>
        /// Event handler for the PairingReqReceived event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="reqResponse">The PairingReqResponse object.</param>
        private static async void Server_PairingReqReceived(object sender, PairingReqResponse reqResponse)
        {
            try
            {
                System.Console.Write(_MessageEnterCode);

                string input = System.Console.ReadLine();
                double code = 0;
                var invalidAmount = true;

                if (double.TryParse(input, out code))
                    invalidAmount = false;

                if (invalidAmount || input == "q")
                    await SendPairingRequestCancel();
                else
                    await SendPairingRequestCode(code.ToString());

                statusEventReceived.Set();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
        }

        private static void Server_PairingNotificationReceived(object sender, PairingNotification reqResponse)
        {
            statusEventReceived.Set();
        }

        /// <summary>
        /// Event handler for the ClientConnected event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs object.</param>
        private static void Server_ClientConnected(object sender, EventArgs e)
        {
            ShowListOfCommands();
        }

        /// <summary>
        /// Event handler for the TerminalStatusReqResponseReceived event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="reqResponse">The TerminalStatusReqResponse object.</param>
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
        }

        private static void Server_ReconciliationReqReceived(object sender, ReconciliationReqResponse reqResponse)
        {
            statusEventReceived.Set();
        }

        private static void Server_CommunicationsReqReceived(CommunicationsReqResponse reqResponse)
        {
            statusEventReceived.Set();
        }

        private static void Server_GetMerchantDataReqReceived(GetMerchantDataReqResponse reqResponse)
        {
            statusEventReceived.Set();
        }

        private static void Server_SetMerchantDataReqReceived(SetMerchantDataReqResponse reqResponse)
        {
            statusEventReceived.Set();
        }

        private static void Server_ConfigTerminalReqReceived(ConfigTerminalReqResponse reqResponse)
        {
            statusEventReceived.Set();
        }

        #endregion

        #region "Commands"

        /// <summary>
        /// Sends a terminal status request.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task SendTerminalStatusRequest()
        {
            try
            {
                var terminalStatusRequest = new TerminalStatusReq();
                await server.SendMessageToClient(JsonConvert.SerializeObject(terminalStatusRequest));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a reconciliation request.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task SendReconciliationRequest()
        {
            try
            {
                string input = string.Empty;

                System.Console.Write(_MessageEnterIBANOrEmptyToUseLastOne);

                // Read user input synchronously
                input = System.Console.ReadLine();

                var reconciliationReq = new ReconciliationReq() { Iban = input };
                await server.SendMessageToClient(JsonConvert.SerializeObject(reconciliationReq));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a communication request.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task SendCommunicationRequest()
        {
            try
            {
                var communicationsReq = new CommunicationsReq();
                await server.SendMessageToClient(JsonConvert.SerializeObject(communicationsReq));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a get merchant data request.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task SendGetMerchantDataRequest(GetMerchantDataReq getMerchantDataReq)
        {
            try
            {
                await server.SendMessageToClient(JsonConvert.SerializeObject(getMerchantDataReq));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a set merchant data request.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task SendSetMerchantDataRequest(MerchantData merchantData)
        {
            try
            {
                await server.SendMessageToClient(JsonConvert.SerializeObject(new SetMerchantDataReq()
                {
                    AcceptorAddress = merchantData.AcceptorAddress,
                    AcceptorLocation = merchantData.AcceptorLocation,
                    AcceptorName = merchantData.AcceptorName,
                    FiscalNumber = merchantData.FiscalNumber,
                    MerchantName = merchantData.MerchantName,
                    TerminalName = merchantData.TerminalName
                }));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a process payment request.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task SendProcessPaymentRequest()
        {
            try
            {
                double amount = 0;
                var invalidAmount = true;

                while (invalidAmount)
                {
                    System.Console.Write(_MessageEnterAmount);

                    // Read user input synchronously
                    string input = System.Console.ReadLine();

                    if (double.TryParse(input, out amount))
                        invalidAmount = false;

                    if (invalidAmount)
                        System.Console.WriteLine(_MessageInvalidInput);
                }

                var processPaymentRequest = new ProcessPaymentReq { AmountData = new AmountData { Amount = amount } };
                await server.SendMessageToClient(JsonConvert.SerializeObject(processPaymentRequest));

                statusEventReceived.Set();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a refund payment request.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task SendRefundPaymentRequest()
        {
            try
            {
                System.Console.WriteLine(_MessageUsingLastSuccessfullTranscationDataForRefund);

                var processPaymentRequest = new RefundReq {
                    Amount = LastPaymentData.Amount,
                    OriginalTransactionData = new OriginalTransactionData
                    {
                        PaymentType = LastPaymentData.PaymentType,
                        ServerDateTime = LastPaymentData.ServerDateTime,
                        SdkId = LastPaymentData.SdkId,
                        ServerId = LastPaymentData.ServerId,
                        TransactionType = LastPaymentData.TransactionType
                    }
                };
                await server.SendMessageToClient(JsonConvert.SerializeObject(processPaymentRequest));

                statusEventReceived.Set();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a pairing code request.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task SendPairingRequest()
        {
            try
            {
                var pairingReq = new PairingReq();
                await server.SendMessageToClient(JsonConvert.SerializeObject(pairingReq));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a pairing code request cancel
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task SendPairingRequestCancel()
        {
            try
            {
                var pairingReq = new PairingReq() {  PairingStep = PairingStep.CANCEL_PAIRING };
                await server.SendMessageToClient(JsonConvert.SerializeObject(pairingReq));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a pairing code request code to validate
        /// </summary>
        /// <param name="code">The code to validate</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task SendPairingRequestCode(string code)
        {
            try
            {
                var pairingReq = new PairingReq() { PairingCode = code, PairingStep = PairingStep.VALIDATE_PAIRING_CODE };
                await server.SendMessageToClient(JsonConvert.SerializeObject(pairingReq));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a set merchant data request.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task SendConfigTerminalRequest(ConfigTerminalReq configTerminalReq)
        {
            try
            {
                await server.SendMessageToClient(JsonConvert.SerializeObject(configTerminalReq));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
        }

        #endregion
    }
}

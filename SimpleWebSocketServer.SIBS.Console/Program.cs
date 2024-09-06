using System;
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
        private const string _MessageEnterCode = "Enter code: ";
        private const string _MessageInvalidInput = "Invalid input";

        #endregion

        #region "Members"

        private static WebSocketServerSibs server;
        private static readonly ManualResetEvent statusEventReceived = new ManualResetEvent(false);

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
                            break;
                        case TerminalCommandOptions.SendPairingRequest:
                            SendPairingRequest().Wait();
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

                // Read user input synchronously
                string input = System.Console.ReadLine();

                var pairingReq = new PairingReq() { PairingCode = input, PairingStep = Lib.Enums.Enums.PairingStep.VALIDATE_PAIRING_CODE };
                await server.SendMessageToClient(JsonConvert.SerializeObject(pairingReq));

                statusEventReceived.Set();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{_MessageErrorProcessingRequest}: {ex.Message}");
            }
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
        /// Sends a process payment request.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task SendProcessPaymentRequest()
        {
            try
            {
                var processPaymentRequest = new ProcessPaymentReq { AmountData = new AmountData { Amount = 0.01 } };
                await server.SendMessageToClient(JsonConvert.SerializeObject(processPaymentRequest));
                System.Console.WriteLine("Process Payment Request sent.");
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

        #endregion
    }
}

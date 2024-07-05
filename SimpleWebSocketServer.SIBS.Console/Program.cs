using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleWebSocketServer.SIBS.Models;

namespace SimpleWebSocketServer.SIBS.Console
{
    internal static class Program
    {
        #region "Constants"

        private const string _WebSocketServerPrefix = "http://+:10005/";
        private const string _MessageEnterJSONCommand = "Enter JSON command or 'q' to stop:";
        private const string _MessageErrorErrorOccurred = "Error occurred";
        private const string _MessageErrorProcessingJSON = "Error processing JSON";
        private const string _MessagePressAnyKeyToExit = "Press any key to exit...";
        private const string _MessageStoppingTheServer = "Stopping the server...";

        #endregion

        private static WebSocketServerSibs server;

        /// <summary>
        /// Listens for user input and sends the input to the WebSocket server.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task ListenForUserInput()
        {
            while (true)
            {
                // Read the entire line of input asynchronously
                string input = await Task.Run(() => System.Console.ReadLine());

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

                // Process the JSON input
                try
                {
                    await server.SendMessageToClient(input);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"{_MessageErrorProcessingJSON}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        static async Task Main(string[] args)
        {
            // Define the WebSocket server prefix
            string prefix = _WebSocketServerPrefix;

            // Create an instance of WebSocketServer
            server = new WebSocketServerSibs();
            server.ClientConnected += Server_ClientConnected;
            server.TerminalStatusReqResponseReceived += Server_TerminalStatusReqResponseReceived;
            server.SetAuthCredentialsReqReceived += Server_SetAuthCredentialsReqReceived;
            server.EventNotificationReceived += Server_EventNotificationReceived;
            server.ProcessPaymentReqReceived += Server_ProcessPaymentReqReceived;
            server.ErrorNotificationReceived += Server_ErrorNotificationReceived;

            try
            {
                System.Console.WriteLine(_MessageEnterJSONCommand);

                // Start the WebSocket server asynchronously
                server.Start(prefix);

                // Listen for user input asynchronously
                Task userInputTask = ListenForUserInput();

                // Wait for either the user to press 'q' or the WebSocket server to stop
                await Task.WhenAny(userInputTask);
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

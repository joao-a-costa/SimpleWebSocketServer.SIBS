# SimpleWebSocketServer.SIBS

A simple WebSocket server implementation in C# using `SimpleWebSocketServer.SIBS.Models` and `Newtonsoft.Json` for JSON parsing.

## Overview

`SimpleWebSocketServer.SIBS` is a lightweight WebSocket server library designed to handle various types of WebSocket messages, including terminal status responses, authentication credential requests, payment processing requests, event notifications, and error notifications.

## Features

- Handle client connections
- Receive and parse different types of messages
- Send messages to clients
- Event-driven architecture for handling different message types

## Getting Started

### Prerequisites

- .NET Framework (4.6.1 or later) or .NET Core/5.0+
- Newtonsoft.Json package

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/SimpleWebSocketServer.SIBS.git
    ```

2. Navigate to the project directory:
    ```sh
    cd SimpleWebSocketServer.SIBS
    ```

3. Install the required NuGet packages:
    ```sh
    dotnet add package Newtonsoft.Json
    dotnet restore
    ```

### Usage

1. Create an instance of `WebSocketServerSibs` and start the server:
    ```csharp
    using SimpleWebSocketServer.SIBS;

    var webSocketServer = new WebSocketServerSibs();
    webSocketServer.Start("http://localhost:5000/");
    ```

2. Subscribe to the events to handle incoming messages and client connections:
    ```csharp
    webSocketServer.ClientConnected += (sender, e) => 
    {
        Console.WriteLine("Client connected.");
    };

    webSocketServer.TerminalStatusReqResponseReceived += (sender, response) =>
    {
        Console.WriteLine("Received Terminal Status Response.");
    };

    webSocketServer.SetAuthCredentialsReqReceived += (sender, response) =>
    {
        Console.WriteLine("Received Set Auth Credentials Response.");
    };

    webSocketServer.ProcessPaymentReqReceived += (sender, response) =>
    {
        Console.WriteLine("Received Process Payment Response.");
    };

    webSocketServer.EventNotificationReceived += (sender, response) =>
    {
        Console.WriteLine("Received Event Notification.");
    };

    webSocketServer.ErrorNotificationReceived += (sender, response) =>
    {
        Console.WriteLine("Received Error Notification.");
    };
    ```

3. Send a message to the client:
    ```csharp
    await webSocketServer.SendMessageToClient("Hello, client!");
    ```

### Event Handling

The following events are available for handling:

- `ClientConnected`
- `TerminalStatusReqResponseReceived`
- `SetAuthCredentialsReqReceived`
- `ProcessPaymentReqReceived`
- `EventNotificationReceived`
- `ErrorNotificationReceived`

### Example

Here's a complete example to demonstrate how to start the server and handle events:

```csharp
using System;
using SimpleWebSocketServer.SIBS;

namespace WebSocketServerExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var webSocketServer = new WebSocketServerSibs();
            webSocketServer.Start("http://localhost:5000/");

            webSocketServer.ClientConnected += (sender, e) =>
            {
                Console.WriteLine("Client connected.");
            };

            webSocketServer.TerminalStatusReqResponseReceived += (sender, response) =>
            {
                Console.WriteLine("Received Terminal Status Response.");
            };

            webSocketServer.SetAuthCredentialsReqReceived += (sender, response) =>
            {
                Console.WriteLine("Received Set Auth Credentials Response.");
            };

            webSocketServer.ProcessPaymentReqReceived += (sender, response) =>
            {
                Console.WriteLine("Received Process Payment Response.");
            };

            webSocketServer.EventNotificationReceived += (sender, response) =>
            {
                Console.WriteLine("Received Event Notification.");
            };

            webSocketServer.ErrorNotificationReceived += (sender, response) =>
            {
                Console.WriteLine("Received Error Notification.");
            };

            // Send a message to the client
            await webSocketServer.SendMessageToClient("Hello, client!");
        }
    }
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgements

- Special thanks to contributors and maintainers.

## Contributing

Contributions are welcome! Please feel free to submit pull requests or open issues for bug fixes, improvements, or new features.

## Support

For support, questions, or suggestions, please [open an issue](https://github.com/yourusername/SimpleWebSocketServer.SIBS/issues).

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    // User credentials
    // These are the hardcoded credentials for the admin and user
    private static readonly string adminUsername = "admin";
    private static readonly string adminPassword = "adminpass";
    private static readonly string userUsername = "user";
    private static readonly string userPassword = "userpass";

    // LED states
    // This array represents the state of each LED (on/off)
    private static readonly bool[] ledStates = new bool[5];

    // This dictionary stores the connected clients with their unique ID and WebSocket instance
    private static readonly ConcurrentDictionary<string, WebSocket> connectedClients = new ConcurrentDictionary<string, WebSocket>();

    static async Task Main()
    {
        try
        {
            // Add a trace listener to log information to a text file
            Trace.Listeners.Add(new TextWriterTraceListener(new StreamWriter("logfile_server.txt", true)));

            // Create a new HTTP listener
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/ws/");
            listener.Start();

            // Log and display the server start information
            Console.WriteLine("WebSocket server started at ws://localhost:5000/ws");
            Trace.WriteLine("WebSocket server started at ws://localhost:5000/ws");

            // Main loop to handle incoming connections
            while (true)
            {
                // Wait for a client to connect
                HttpListenerContext context = await listener.GetContextAsync();

                // If the request is a WebSocket request, accept it
                if (context.Request.IsWebSocketRequest)
                {
                    // Accept the WebSocket connection
                    HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
                    WebSocket webSocket = wsContext.WebSocket;

                    // Generate a unique ID for the client and add it to the connected clients
                    string clientId = Guid.NewGuid().ToString();
                    connectedClients.TryAdd(clientId, webSocket);

                    // Log the connection
                    Trace.WriteLine($"Client {clientId} connected");

                    // Start handling the WebSocket in a separate task
                    _ = HandleWebSocket(clientId, webSocket);
                }
                else
                {
                    // If the request is not a WebSocket request, return a 400 status code
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }

                // Flush the trace log
                Trace.Flush();
            }
        }
        catch (Exception ex)
        {
            // Log any critical errors
            Trace.WriteLine($"Critical error in Main: {ex.Message}");
        }
    }

    private static async Task HandleWebSocket(string clientId, WebSocket webSocket)
    {
        try
        {
            // Buffer for receiving messages
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;

            // Loop until a successful connection is made
            while (true)
            {
                try
                {
                    // Receive a message from the client
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    // If the connection is closed, throw an exception
                    if (result.CloseStatus.HasValue)
                    {
                        throw new Exception("WebSocket connection closed.");
                    }

                    // Connection successful, exit the loop
                    break;
                }
                catch (Exception ex)
                {
                    // Log the error and try to reconnect after 5 seconds
                    Trace.WriteLine($"Error: {ex.Message}. Attempting to reconnect...");
                    await Task.Delay(5000);
                }
            }

            // Loop until the connection is closed
            while (!result.CloseStatus.HasValue)
            {
                // Decode the received message
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                // Log the received message
                Trace.WriteLine($"Received message from client {clientId}: {message}");

                // Process the message and get a response
                var response = ProcessMessage(message);

                // Encode the response and broadcast it to all connected clients
                var responseBuffer = Encoding.UTF8.GetBytes(response);
                await BroadcastMessage(responseBuffer);

                // Log the sent message
                Trace.WriteLine($"Sent message to client {clientId}: {response}");

                // Wait for the next message
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            // Remove the client from the connected clients and close the WebSocket
            connectedClients.TryRemove(clientId, out _);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

            // Log the disconnection
            Trace.WriteLine($"Client {clientId} disconnected");
        }
        catch (Exception ex)
        {
            // Log any critical errors
            Trace.WriteLine($"Critical error in HandleWebSocket: {ex.Message}");
        }
    }

    private static async Task BroadcastMessage(byte[] message)
    {
        // Loop through all connected clients
        foreach (var client in connectedClients.Values)
        {
            // If the client is still connected, send the message
            if (client.State == WebSocketState.Open)
            {
                await client.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);

                // Log the broadcasted message
                Trace.WriteLine($"Broadcasted message: {Encoding.UTF8.GetString(message)}");
            }
        }
    }

    private static string ProcessMessage(string message)
    {
        // The message format is "username:password:command"
        var parts = message.Split(':');

        // If the message is correctly formatted
        if (parts.Length == 3)
        {
            var username = parts[0];
            var password = parts[1];
            var command = parts[2];

            // If the user is valid
            if (IsValidUser(username, password))
            {
                // If the command is a toggle command
                if (command.StartsWith("toggle"))
                {
                    var commandParts = command.Split(' ');

                    // If the command is correctly formatted
                    if (commandParts.Length < 2)
                    {
                        return "ERR: Invalid toggle command";
                    }

                    // If the user is an admin
                    if (username == adminUsername)
                    {
                        // If the LED index is valid, toggle the LED and return the new status
                        if (int.TryParse(commandParts[1], out int ledIndex) && ledIndex >= 0 && ledIndex < ledStates.Length)
                        {
                            ledStates[ledIndex] = !ledStates[ledIndex];
                            return GetLedStatus();
                        }
                        else
                        {
                            return "ERR: Invalid LED index";
                        }
                    }
                    else
                    {
                        return "ERR: Unauthorized";
                    }
                }
                // If the command is a status command, return the current status
                else if (command == "status")
                {
                    return GetLedStatus();
                }
                else
                {
                    return "ERR: Unknown command";
                }
            }
            else
            {
                return "ERR: Invalid credentials";
            }
        }
        else
        {
            return "ERR: Invalid message format";
        }
    }

    private static bool IsValidUser(string username, string password)
    {
        // Check if the username and password match either the admin or the user
        return (username == adminUsername && password == adminPassword) ||
               (username == userUsername && password == userPassword);
    }

    private static string GetLedStatus()
    {
        // Convert the LED states to a string
        char[] status = new char[ledStates.Length];
        for (int i = 0; i < ledStates.Length; i++)
        {
            status[i] = ledStates[i] ? '1' : '0';
        }
        return new string(status);
    }
}

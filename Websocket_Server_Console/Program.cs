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
    private static readonly string adminUsername = "admin";
    private static readonly string adminPassword = "adminpass";
    private static readonly string userUsername = "user";
    private static readonly string userPassword = "userpass";

    // LED states
    private static readonly bool[] ledStates = new bool[5];
    private static readonly ConcurrentDictionary<string, WebSocket> connectedClients = new ConcurrentDictionary<string, WebSocket>();

    static async Task Main()
    {
        try
        {
            Trace.Listeners.Add(new TextWriterTraceListener(new StreamWriter("logfile_server.txt", true)));
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/ws/");
            listener.Start();
            Console.WriteLine("WebSocket server started at ws://localhost:5000/ws");
            Trace.WriteLine("WebSocket server started at ws://localhost:5000/ws");

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
                    WebSocket webSocket = wsContext.WebSocket;
                    string clientId = Guid.NewGuid().ToString();
                    connectedClients.TryAdd(clientId, webSocket);
                    Trace.WriteLine($"Client {clientId} connected");
                    _ = HandleWebSocket(clientId, webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
                Trace.Flush();
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Critical error in Main: {ex.Message}");
        }

    }

    private static async Task HandleWebSocket(string clientId, WebSocket webSocket)
    {
        try
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;

            while (true)
            {
                try
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.CloseStatus.HasValue)
                    {
                        throw new Exception("WebSocket connection closed.");
                    }
                    break; // Connection successful, exit the loop
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error: {ex.Message}. Attempting to reconnect...");
                    await Task.Delay(5000); // Wait for 5 seconds before trying to reconnect
                }
            }
            while (!result.CloseStatus.HasValue)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Trace.WriteLine($"Received message from client {clientId}: {message}");

                var response = ProcessMessage(message);
                var responseBuffer = Encoding.UTF8.GetBytes(response);
                await BroadcastMessage(responseBuffer);
                Trace.WriteLine($"Sent message to client {clientId}: {response}");

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            connectedClients.TryRemove(clientId, out _);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            Trace.WriteLine($"Client {clientId} disconnected");
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Critical error in HandleWebSocket: {ex.Message}");
        }
    }


    private static async Task BroadcastMessage(byte[] message)
    {
        foreach (var client in connectedClients.Values)
        {
            if (client.State == WebSocketState.Open)
            {
                await client.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);
                Trace.WriteLine($"Broadcasted message: {Encoding.UTF8.GetString(message)}");
            }
        }
    }

    private static string ProcessMessage(string message)
    {
        // Message format: "username:password:command"
        var parts = message.Split(':');
        if (parts.Length == 3)
        {
            var username = parts[0];
            var password = parts[1];
            var command = parts[2];

            if (IsValidUser(username, password))
            {
                if (command.StartsWith("toggle"))
                {
                    var commandParts = command.Split(' ');
                    if (commandParts.Length < 2)
                    {
                        return "ERR: Invalid toggle command";
                    }

                    if (username == adminUsername)
                    {
                        if (int.TryParse(commandParts[1], out int ledIndex) && ledIndex >= 0 && ledIndex < ledStates.Length)
                        {
                            ledStates[ledIndex] = !ledStates[ledIndex];
                            return GetLedStatus(); // Send the updated LED status
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
        return (username == adminUsername && password == adminPassword) ||
               (username == userUsername && password == userPassword);
    }

    private static string GetLedStatus()
    {
        char[] status = new char[ledStates.Length];
        for (int i = 0; i < ledStates.Length; i++)
        {
            status[i] = ledStates[i] ? '1' : '0';
        }
        return new string(status);
    }
}

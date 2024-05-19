using EasyModbus;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;

namespace Websocket_Server
{
    public partial class Subscriber : Form
    {
        // ClientWebSocket instance for connecting to the WebSocket server
        private readonly ClientWebSocket _webSocket;

        // ModbusClient instance for connecting to the Modbus server
        private ModbusClient? modbusClient;

        // Flag to indicate if the Modbus client is connected
        bool connected = true;

        public Subscriber()
        {
            InitializeComponent();

            // Initialize the Modbus checkboxes to unchecked
            modbusCheckBoxes0.Checked = false;
            modbusCheckBoxes1.Checked = false;
            modbusCheckBoxes2.Checked = false;
            modbusCheckBoxes3.Checked = false;
            modbusCheckBoxes4.Checked = false;

            // Disable the LED checkboxes
            chkLED1.Enabled = false;
            chkLED2.Enabled = false;
            chkLED3.Enabled = false;
            chkLED4.Enabled = false;
            chkLED5.Enabled = false;

            // Initialize the WebSocket client
            _webSocket = new ClientWebSocket();

            // Connect to the WebSocket server
            ConnectToWebSocket();

            // Initialize the Modbus client
            InitializeModbusClient();
        }

        private async void ConnectToWebSocket()
        {
            try
            {
                // Connect to the WebSocket server
                await _webSocket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);

                // Log the connection and update the status label
                Trace.WriteLine("WebSocket connected to Subscriber");
                websocketstatus.Text = "WebSocket: Connected";

                // Start receiving messages in a separate task
                _ = Task.Run(ReceiveMessages);
            }
            catch (Exception ex)
            {
                // Log any connection errors and update the status label
                Trace.WriteLine($"WebSocket connection error: {ex.Message}");
                websocketstatus.Text = "WebSocket: Connection Error";
            }
        }

        private async Task ReceiveMessages()
        {
            var buffer = new byte[1024 * 4];

            // Loop while the WebSocket is open
            while (_webSocket.State == WebSocketState.Open)
            {
                try
                {
                    // Receive a message from the server
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    // Decode the message and update the LED states
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    UpdateLEDStates(message);

                    // Log the received message and add it to the list box
                    Trace.WriteLine($"Received message: {message}");
                    Invoke((Action)(() => lstMessages.Items.Add("Received: " + message)));
                }
                catch (Exception ex)
                {
                    // Log any errors and add them to the list box
                    Trace.WriteLine($"WebSocket receive message error: {ex.Message}");
                    Invoke((Action)(() => lstMessages.Items.Add("Error: " + ex.Message)));
                }
            }
        }

        private void UpdateLEDStates(string stateMessage)
        {
            // If the state message is the correct length
            if (stateMessage.Length == 5)
            {
                try
                {
                    // Update the LED checkboxes based on the state message
                    Invoke((Action)(() =>
                    {
                        chkLED1.Checked = stateMessage[0] == '1';
                        chkLED2.Checked = stateMessage[1] == '1';
                        chkLED3.Checked = stateMessage[2] == '1';
                        chkLED4.Checked = stateMessage[3] == '1';
                        chkLED5.Checked = stateMessage[4] == '1';
                    }));
                }
                catch (Exception ex)
                {
                    // Log any errors
                    Trace.WriteLine($"Invalid LED state message: {ex.Message}");
                }
            }
            else
            {
                // Log an error if the state message is the wrong length
                Trace.WriteLine("Invalid LED state message length");
            }
        }

        private void PollModbus()
        {
            try
            {
                // Update the status label and read the coil states from the Modbus server
                modbusConnectionStatusLabel.Text = "MODBUS: Connected and Polling";
                var data = modbusClient?.ReadCoils(0, 5);

                // If the read failed, update the status label and return
                if (data == null)
                {
                    modbusConnectionStatusLabel.Text = "MODBUS: Error";
                    return;
                }

                // Update the Modbus checkboxes based on the coil states
                for (int i = 0; i < 5; i++)
                {
                    modbusCheckBoxes0.Checked = data[i];
                    modbusCheckBoxes1.Checked = data[i];
                    modbusCheckBoxes2.Checked = data[i];
                    modbusCheckBoxes3.Checked = data[i];
                    modbusCheckBoxes4.Checked = data[i];
                }
            }
            catch (Exception ex)
            {
                // Update the status label if there was an error
                modbusConnectionStatusLabel.Text = $"MODBUS: Error - {ex.Message}";
            }
        }

        private void InitializeModbusClient()
        {
            try
            {
                // Initialize the Modbus client and connect to the server
                modbusClient = new ModbusClient("127.0.0.1", 502)
                {
                    ConnectionTimeout = 5000
                }; // Adjust IP and port as necessary
                modbusClient.Connect();

                // Update the status label
                modbusConnectionStatusLabel.Text = "MODBUS: Connected";
            }
            catch (Exception ex)
            {
                // Update the status label if there was an error and try to reconnect
                modbusConnectionStatusLabel.Text = "MODBUS: Disconnected";
                Thread.Sleep(1000);
                modbusConnectionStatusLabel.Text = $"MODBUS: Error -\n{ex.Message}.\nAttempting to reconnect...";
                Thread.Sleep(5000); // Wait for 5 seconds before trying to reconnect
                connected = false;
            }

            // If the Modbus client is connected, start polling
            if (connected)
            {
                System.Windows.Forms.Timer modbusPollTimer = new() { Interval = 1000 };
                modbusPollTimer.Tick += (sender, e) => PollModbus();
                modbusPollTimer.Start();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Close the WebSocket when the form is closing
            _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).Wait();

            base.OnFormClosing(e);
        }
    }
}

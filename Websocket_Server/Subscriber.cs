using EasyModbus;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;

namespace Websocket_Server
{
    public partial class Subscriber : Form
    {
        private readonly ClientWebSocket _webSocket;
        private ModbusClient? modbusClient;
        bool connected = true;
        public Subscriber()
        {
            InitializeComponent();
            modbusCheckBoxes0.Checked = false;
            modbusCheckBoxes1.Checked = false;
            modbusCheckBoxes2.Checked = false;
            modbusCheckBoxes3.Checked = false;
            modbusCheckBoxes4.Checked = false;
            chkLED1.Enabled = false;
            chkLED2.Enabled = false;
            chkLED3.Enabled = false;
            chkLED4.Enabled = false;
            chkLED5.Enabled = false;
            _webSocket = new ClientWebSocket();
            ConnectToWebSocket();
            InitializeModbusClient();
        }

        private async void ConnectToWebSocket()
        {
            try
            {
                await _webSocket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
                Trace.WriteLine("WebSocket connected to Subscriber");
                websocketstatus.Text = "WebSocket: Connected";
                _ = Task.Run(ReceiveMessages);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"WebSocket connection error: {ex.Message}");
                websocketstatus.Text = "WebSocket: Connection Error";
            }
        }

        private async Task ReceiveMessages()
        {
            var buffer = new byte[1024 * 4];
            while (_webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    UpdateLEDStates(message);
                    Trace.WriteLine($"Received message: {message}");
                    Invoke((Action)(() => lstMessages.Items.Add("Received: " + message)));
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"WebSocket receive message error: {ex.Message}");
                    Invoke((Action)(() => lstMessages.Items.Add("Error: " + ex.Message)));
                }
            }
        }

        private void UpdateLEDStates(string stateMessage)
        {
            if (stateMessage.Length == 5)
            {
                try
                {
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
                    Trace.WriteLine($"Invalid LED state message: {ex.Message}");
                }
            }
            else
            {
                Trace.WriteLine("Invalid LED state message length");
            }
        }

        private void PollModbus()
        {
            try
            {
                modbusConnectionStatusLabel.Text = "MODBUS: Connected and Polling";
                var data = modbusClient?.ReadCoils(0, 5);
                if (data == null)
                {
                    modbusConnectionStatusLabel.Text = "MODBUS: Error";
                    return;
                }
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
                modbusConnectionStatusLabel.Text = $"MODBUS: Error - {ex.Message}";
            }
        }
        private void InitializeModbusClient()
        {
            try
            {
                modbusClient = new ModbusClient("127.0.0.1", 502)
                {
                    ConnectionTimeout = 5000
                }; // Adjust IP and port as necessary
                modbusClient.Connect();
                modbusConnectionStatusLabel.Text = "MODBUS: Connected";
            }
            catch (Exception ex)
            {
                modbusConnectionStatusLabel.Text = "MODBUS: Disconnected";
                Thread.Sleep(1000);
                modbusConnectionStatusLabel.Text = $"MODBUS: Error -\n{ex.Message}.\nAttempting to reconnect...";
                Thread.Sleep(5000); // Wait for 5 seconds before trying to reconnect
                connected = false;
            }
            if (connected)
            {
                System.Windows.Forms.Timer modbusPollTimer = new() { Interval = 1000 };
                modbusPollTimer.Tick += (sender, e) => PollModbus();
                modbusPollTimer.Start();
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).Wait();
            base.OnFormClosing(e);
        }
    }
}
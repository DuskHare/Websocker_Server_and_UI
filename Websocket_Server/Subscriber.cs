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
            Task.Run(() => InitializeModbusClient());
        }

        private async void ConnectToWebSocket()
        {
            await _webSocket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            Trace.WriteLine("WebSocket connected to Subscriber");
            websocketstatus.Text = "WebSocket: Connected";
            _ = Task.Run(ReceiveMessages);
        }

        private async Task ReceiveMessages()
        {
            var buffer = new byte[1024 * 4];
            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                UpdateLEDStates(message);
                Trace.WriteLine($"Received message: {message}");
                Invoke((Action)(() => lstMessages.Items.Add("Received: " + message)));
            }
        }

        private void UpdateLEDStates(string stateMessage)
        {
            if (stateMessage.Length == 5)
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
            while (true)
            {
                try
                {
                    modbusClient = new ModbusClient("127.0.0.1", 502)
                    {
                        ConnectionTimeout = 5000
                    }; // Adjust IP and port as necessary
                    modbusClient.Connect();
                    this.Invoke((MethodInvoker)delegate {
                        modbusConnectionStatusLabel.Text = "MODBUS: Connected";
                    });
                    break; // Connection successful, exit the loop
                }
                catch (Exception ex)
                {
                    this.Invoke((MethodInvoker)delegate {
                        modbusConnectionStatusLabel.Text = "MODBUS: Disconnected";
                    });
                    Thread.Sleep(1000);
                    this.Invoke((MethodInvoker)delegate {
                        modbusConnectionStatusLabel.Text = $"MODBUS: Error -\n{ex.Message}.\nAttempting to reconnect...";
                    });
                    Thread.Sleep(5000); // Wait for 5 seconds before trying to reconnect
                }
            }

            System.Windows.Forms.Timer modbusPollTimer = new() { Interval = 1000 };
            modbusPollTimer.Tick += (sender, e) => PollModbus();
            modbusPollTimer.Start();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).Wait();
            base.OnFormClosing(e);
        }
    }
}
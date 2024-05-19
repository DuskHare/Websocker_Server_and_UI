using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Websocket_Server
{
    public partial class Publisher : Form
    {
        private readonly ClientWebSocket _webSocket;

        public Publisher(bool isAdmin)
        {
            InitializeComponent();
            _webSocket = new ClientWebSocket();
            ConnectToWebSocket();
            StartSendingMessages(isAdmin); // Call the method to start sending messages
        }

        private async void ConnectToWebSocket()
        {
            await _webSocket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            Trace.WriteLine("WebSocket connected to Publisher");
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
                Trace.WriteLine($"Received message: {message}");
                Invoke((Action)(() => lstMessages.Items.Add("Received: " + message)));
            }
        }

        private bool GetCheckBoxState(int index)
        {
            return index switch
            {
                0 => chkLED1.Checked,
                1 => chkLED2.Checked,
                2 => chkLED3.Checked,
                3 => chkLED4.Checked,
                4 => chkLED5.Checked,
                _ => false,
            };
        }
        private readonly bool[] ledStates = new bool[5];

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).Wait();
            base.OnFormClosing(e);
        }

        private async void StartSendingMessages(bool isAdmin)
        {
            await Task.Run(async () =>
            {
                while (true) // Run indefinitely
                {
                    if (!isAdmin && (chkLED1.Checked || chkLED2.Checked || chkLED3.Checked || chkLED4.Checked || chkLED5.Checked))
                    {
                        MessageBox.Show("Only admin can control the LEDs", "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Invoke((Action)(() => chkLED1.Checked = chkLED2.Checked = chkLED3.Checked = chkLED4.Checked = chkLED5.Checked = false));
                    }
                    else if (isAdmin)
                    {

                        for (int i = 0; i < 5; i++)
                        {
                            var isChecked = GetCheckBoxState(i);
                            var command = $"admin:adminpass:toggle {i}";
                            if (isChecked != ledStates[i])
                            {
                                ledStates[i] = isChecked;
                                var buffer = Encoding.UTF8.GetBytes(command);
                                await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                                Invoke((Action)(() => lstMessages.Items.Add("Sent: " + command)));
                            }
                        }
                        await Task.Delay(1000); // Delay for 1 second before sending the next set of messages

                    }
                }
            });
        }
    }
}





using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Websocket_Server
{
    public partial class Publisher : Form
    {
        private ClientWebSocket _webSocket;

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
                Invoke((Action)(() => lstMessages.Items.Add("Received: " + message)));
            }
        }

        private bool GetCheckBoxState(int index)
        {
            switch (index)
            {
                case 0: return chkLED1.Checked;
                case 1: return chkLED2.Checked;
                case 2: return chkLED3.Checked;
                case 3: return chkLED4.Checked;
                case 4: return chkLED5.Checked;
                default: return false;
            }
        }
        private bool[] ledStates = new bool[5];

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
            });
        }

    }
}

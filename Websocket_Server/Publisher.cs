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
        // ClientWebSocket instance for connecting to the WebSocket server
        private readonly ClientWebSocket _webSocket;

        public Publisher(bool isAdmin)
        {
            InitializeComponent();

            // Initialize the WebSocket client
            _webSocket = new ClientWebSocket();

            // Connect to the WebSocket server
            ConnectToWebSocket();

            // Start sending messages if the user is an admin
            StartSendingMessages(isAdmin);
        }

        private async void ConnectToWebSocket()
        {
            // Connect to the WebSocket server
            await _webSocket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);

            // Log the connection and update the status label
            Trace.WriteLine("WebSocket connected to Publisher");
            websocketstatus.Text = "WebSocket: Connected";
        }

        private bool GetCheckBoxState(int index)
        {
            // Return the checked state of the checkbox at the given index
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

        // Array to store the state of each LED
        private readonly bool[] ledStates = new bool[5];

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Close the WebSocket when the form is closing
            _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).Wait();

            base.OnFormClosing(e);
        }

        private async void StartSendingMessages(bool isAdmin)
        {
            await Task.Run(async () =>
            {
                // Loop indefinitely
                while (true)
                {
                    // If the user is not an admin and any of the LEDs are checked, show an error message and uncheck the LEDs
                    if (!isAdmin && (chkLED1.Checked || chkLED2.Checked || chkLED3.Checked || chkLED4.Checked || chkLED5.Checked))
                    {
                        MessageBox.Show("Only admin can control the LEDs", "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Invoke((Action)(() => chkLED1.Checked = chkLED2.Checked = chkLED3.Checked = chkLED4.Checked = chkLED5.Checked = false));
                    }
                    else if (isAdmin)
                    {
                        // If the user is an admin, loop through each LED
                        for (int i = 0; i < 5; i++)
                        {
                            // If the state of the LED has changed, send a toggle command to the server
                            var isChecked = GetCheckBoxState(i);
                            var command = $"admin:adminpass:toggle {i}";
                            if (isChecked != ledStates[i])
                            {
                                ledStates[i] = isChecked;
                                var buffer = Encoding.UTF8.GetBytes(command);
                                await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

                                // Add the sent command to the list box
                                Invoke((Action)(() => lstMessages.Items.Add("Sent: " + command)));
                            }
                        }

                        // Delay for 1 second before sending the next set of messages
                        await Task.Delay(1000);
                    }
                }
            });
        }
    }
}


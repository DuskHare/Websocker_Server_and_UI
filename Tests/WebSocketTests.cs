using System.Net.WebSockets;
using System.Text;
using System.Threading;
namespace Tests
{
    [TestFixture]
    public class WebSocketTests
    {
        private ClientWebSocket _clientWebSocket;

        [SetUp]
        public void Setup()
        {
            _clientWebSocket = new ClientWebSocket();
        }

        [Test]
        public async Task TestConnectionToServer()
        {
            await _clientWebSocket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            Assert.That(_clientWebSocket.State, Is.EqualTo(WebSocketState.Open));
        }

        [Test]
        public async Task TestDisconnectionFromServer()
        {
            await _clientWebSocket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal closure", CancellationToken.None);
            Assert.That(_clientWebSocket.State, Is.EqualTo(WebSocketState.Closed));
        }

        [Test]
        public async Task TestSendReceiveMessageFromServer()
        {
            await _clientWebSocket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            var buffer = Encoding.UTF8.GetBytes("admin:adminpass:toggle 0");
            await _clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            buffer = new byte[1024 * 4];
            var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Assert.That(message, Is.Not.Null);
        }

        [Test]
        public async Task TestUnexpectedDisconnectionFromServer()
        {
            await _clientWebSocket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "Endpoint unavailable", CancellationToken.None);
            Assert.That(_clientWebSocket.State, Is.EqualTo(WebSocketState.Closed));
        }

        [Test]
        public async Task TestReconnectionToServer()
        {
            await _clientWebSocket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal closure", CancellationToken.None);
            _clientWebSocket = new ClientWebSocket();
            await _clientWebSocket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            Assert.That(_clientWebSocket.State, Is.EqualTo(WebSocketState.Open));
        }

        [TearDown]
        public void TearDown()
        {
            _clientWebSocket.Dispose();
        }
    }
}
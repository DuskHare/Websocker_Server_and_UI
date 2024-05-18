using System.Net;
using System.Net.WebSockets;
using System.Text;
using Websocket_Server;

namespace Websocker_Server_and_UI
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Thread loginFormThread = new Thread(() =>
            {
                Application.Run(new LoginForm());
            });
            loginFormThread.Start();

            Thread unifiedClientFormThread = new Thread(() =>
            {
                Application.Run(new Subscriber());
            });
            unifiedClientFormThread.Start();
        }
    }
}
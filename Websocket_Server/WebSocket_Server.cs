using Websocket_Server;

namespace Websocker_Server_and_UI
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Thread loginFormThread = new(() =>
            {
                LoginForm loginForm = new LoginForm();
                Application.Run(loginForm);
                if (loginForm.DialogResult == DialogResult.OK)
                {
                    Application.Run(new Publisher(true));
                }
                else if (loginForm.DialogResult == DialogResult.No)
                {
                    Application.Run(new Publisher(false));
                }
            });
            loginFormThread.Start();

            Thread subscriberFormThread = new(() =>
            {
                Application.Run(new Subscriber());
            });
            subscriberFormThread.Start();
        }
    }
}
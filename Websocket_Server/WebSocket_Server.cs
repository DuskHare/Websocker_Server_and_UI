using System.Diagnostics;
using Websocket_Server;

namespace Websocker_Server_and_UI
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            // Add a trace listener to log information to a text file
            Trace.Listeners.Add(new TextWriterTraceListener(new StreamWriter("logfile_ui.txt", true)));

            // Create a new thread for the login form
            Thread loginFormThread = new(() =>
            {
                // Create and display the login form
                LoginForm loginForm = new LoginForm();
                Application.Run(loginForm);

                // If the user clicked OK, run the publisher form as an admin
                if (loginForm.DialogResult == DialogResult.OK)
                {
                    Application.Run(new Publisher(true));
                }
                // If the user clicked No, run the publisher form as a non-admin
                else if (loginForm.DialogResult == DialogResult.No)
                {
                    Application.Run(new Publisher(false));
                }
                // If the user clicked Cancel or closed the form, the application will exit
            });
            // Start the login form thread
            loginFormThread.Start();

            // Create a new thread for the subscriber form
            Thread subscriberFormThread = new(() =>
            {
                // Create and display the subscriber form
                Application.Run(new Subscriber());
            });
            // Start the subscriber form thread
            subscriberFormThread.Start();

            // Flush the trace log
            Trace.Flush();
        }
    }
}

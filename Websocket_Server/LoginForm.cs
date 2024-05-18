using Websocker_Server_and_UI;

namespace Websocket_Server
{
    public partial class LoginForm : Form
    {
        static public TextBox usernameTextBox;
        static public TextBox passwordTextBox;
        bool isAdmin = false;
        private Button loginButton;

        public LoginForm()
        {
            usernameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 10),
                Width = 200
            };
            Controls.Add(usernameTextBox);

            passwordTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 40),
                Width = 200,
                PasswordChar = '*'
            };
            Controls.Add(passwordTextBox);

            loginButton = new Button
            {
                Text = "Login",
                Location = new System.Drawing.Point(10, 70)
            };
            loginButton.Click += LoginButton_Click;
            Controls.Add(loginButton);

            Text = "Login";
            Width = 250;
            Height = 150;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            while (usernameTextBox.Text != "admin" && passwordTextBox.Text != "adminpass")
            {
                DialogResult dialog = MessageBox.Show("You are not authorized to turn off the LEDs", "Unauthorized", MessageBoxButtons.OK);
                return;
            }
            new Publisher(isAdmin).Show();
            Hide();
        }
    }
}
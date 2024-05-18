using Websocker_Server_and_UI;

namespace Websocket_Server
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponents();
            CustomizeLoginForm();
        }

        private void InitializeComponents()
        {
            LoginFormHelpers.usernameLabel = new Label
            {
                Text = "Username",
                Location = new System.Drawing.Point(10, 10),
                Width = 70
            };
            Controls.Add(LoginFormHelpers.usernameLabel);

            LoginFormHelpers.usernameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(90, 10),
                Width = 170
            };
            Controls.Add(LoginFormHelpers.usernameTextBox);

            LoginFormHelpers.passwordLabel = new Label
            {
                Text = "Password",
                Location = new System.Drawing.Point(10, 40),
                Width = 70
            };
            Controls.Add(LoginFormHelpers.passwordLabel);

            LoginFormHelpers.passwordTextBox = new TextBox
            {
                Location = new System.Drawing.Point(90, 40),
                Width = 170,
                PasswordChar = '*'
            };
            Controls.Add(LoginFormHelpers.passwordTextBox);

            LoginFormHelpers.loginButton = new Button
            {
                Text = "Login",
                Location = new System.Drawing.Point(90, 70)
            };
            LoginFormHelpers.loginButton.Click += LoginButton_Click;
            Controls.Add(LoginFormHelpers.loginButton);
        }

        private void CustomizeLoginForm()
        {
            Text = "Login";
            Width = 300;
            Height = 150;
        }

        private void LoginButton_Click(object? sender, EventArgs e)
        {
            while (LoginFormHelpers.usernameTextBox?.Text != "admin" && LoginFormHelpers.passwordTextBox?.Text != "adminpass")
            {
                MessageBox.Show("You are not authorized to turn off the LEDs", "Alert", MessageBoxButtons.OK);
                this.DialogResult = DialogResult.No;
                this.Close();
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
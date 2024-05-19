using Websocker_Server_and_UI;

namespace Websocket_Server
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            // Initialize the components of the form
            InitializeComponents();

            // Customize the form
            CustomizeLoginForm();
        }

        private void InitializeComponents()
        {
            // Create and add the username label to the form
            LoginFormHelpers.usernameLabel = new Label
            {
                Text = "Username",
                Location = new System.Drawing.Point(10, 10),
                Width = 70
            };
            Controls.Add(LoginFormHelpers.usernameLabel);

            // Create and add the username text box to the form
            LoginFormHelpers.usernameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(90, 10),
                Width = 170
            };
            Controls.Add(LoginFormHelpers.usernameTextBox);

            // Create and add the password label to the form
            LoginFormHelpers.passwordLabel = new Label
            {
                Text = "Password",
                Location = new System.Drawing.Point(10, 40),
                Width = 70
            };
            Controls.Add(LoginFormHelpers.passwordLabel);

            // Create and add the password text box to the form
            LoginFormHelpers.passwordTextBox = new TextBox
            {
                Location = new System.Drawing.Point(90, 40),
                Width = 170,
                PasswordChar = '*' // Hide the password characters
            };
            Controls.Add(LoginFormHelpers.passwordTextBox);

            // Create and add the login button to the form
            LoginFormHelpers.loginButton = new Button
            {
                Text = "Login",
                Location = new System.Drawing.Point(90, 70)
            };
            LoginFormHelpers.loginButton.Click += LoginButton_Click; // Add a click event handler to the button
            Controls.Add(LoginFormHelpers.loginButton);
        }

        private void CustomizeLoginForm()
        {
            // Set the title of the form
            Text = "Login";

            // Set the size of the form
            Width = 300;
            Height = 150;
        }

        private void LoginButton_Click(object? sender, EventArgs e)
        {
            // If the username and password are not correct, show an error message and close the form
            while (LoginFormHelpers.usernameTextBox?.Text != "admin" && LoginFormHelpers.passwordTextBox?.Text != "adminpass")
            {
                MessageBox.Show("You are not authorized to turn off the LEDs", "Alert", MessageBoxButtons.OK);
                this.DialogResult = DialogResult.No;
                this.Close();
                return;
            }

            // If the username and password are correct, close the form with a DialogResult of OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

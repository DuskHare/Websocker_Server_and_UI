namespace Websocket_Server
{
    partial class Subscriber
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lstMessages = new ListBox();
            chkLED5 = new CheckBox();
            chkLED4 = new CheckBox();
            chkLED3 = new CheckBox();
            chkLED2 = new CheckBox();
            chkLED1 = new CheckBox();
            modbusCheckBoxes4 = new CheckBox();
            modbusCheckBoxes3 = new CheckBox();
            modbusCheckBoxes2 = new CheckBox();
            modbusCheckBoxes1 = new CheckBox();
            modbusCheckBoxes0 = new CheckBox();
            modbusConnectionStatusLabel = new Label();
            websocketstatus = new Label();
            SuspendLayout();
            // 
            // lstMessages
            // 
            lstMessages.FormattingEnabled = true;
            lstMessages.ItemHeight = 25;
            lstMessages.Location = new Point(341, 61);
            lstMessages.Name = "lstMessages";
            lstMessages.Size = new Size(389, 329);
            lstMessages.TabIndex = 12;
            // 
            // chkLED5
            // 
            chkLED5.AutoSize = true;
            chkLED5.Location = new Point(37, 210);
            chkLED5.Name = "chkLED5";
            chkLED5.Size = new Size(83, 29);
            chkLED5.TabIndex = 11;
            chkLED5.Text = "LED 5";
            chkLED5.UseVisualStyleBackColor = true;
            // 
            // chkLED4
            // 
            chkLED4.AutoSize = true;
            chkLED4.Location = new Point(37, 172);
            chkLED4.Name = "chkLED4";
            chkLED4.Size = new Size(83, 29);
            chkLED4.TabIndex = 10;
            chkLED4.Text = "LED 4";
            chkLED4.UseVisualStyleBackColor = true;
            // 
            // chkLED3
            // 
            chkLED3.AutoSize = true;
            chkLED3.Location = new Point(37, 133);
            chkLED3.Name = "chkLED3";
            chkLED3.Size = new Size(83, 29);
            chkLED3.TabIndex = 9;
            chkLED3.Text = "LED 3";
            chkLED3.UseVisualStyleBackColor = true;
            // 
            // chkLED2
            // 
            chkLED2.AutoSize = true;
            chkLED2.Location = new Point(37, 97);
            chkLED2.Name = "chkLED2";
            chkLED2.Size = new Size(83, 29);
            chkLED2.TabIndex = 8;
            chkLED2.Text = "LED 2";
            chkLED2.UseVisualStyleBackColor = true;
            // 
            // chkLED1
            // 
            chkLED1.AutoSize = true;
            chkLED1.Location = new Point(37, 61);
            chkLED1.Name = "chkLED1";
            chkLED1.Size = new Size(83, 29);
            chkLED1.TabIndex = 7;
            chkLED1.Text = "LED 1";
            chkLED1.UseVisualStyleBackColor = true;
            // 
            // modbusCheckBoxes4
            // 
            modbusCheckBoxes4.AutoSize = true;
            modbusCheckBoxes4.Location = new Point(181, 210);
            modbusCheckBoxes4.Name = "modbusCheckBoxes4";
            modbusCheckBoxes4.Size = new Size(83, 29);
            modbusCheckBoxes4.TabIndex = 17;
            modbusCheckBoxes4.Text = "Coil 5";
            modbusCheckBoxes4.UseVisualStyleBackColor = true;
            // 
            // modbusCheckBoxes3
            // 
            modbusCheckBoxes3.AutoSize = true;
            modbusCheckBoxes3.Location = new Point(181, 172);
            modbusCheckBoxes3.Name = "modbusCheckBoxes3";
            modbusCheckBoxes3.Size = new Size(83, 29);
            modbusCheckBoxes3.TabIndex = 16;
            modbusCheckBoxes3.Text = "Coil 4";
            modbusCheckBoxes3.UseVisualStyleBackColor = true;
            // 
            // modbusCheckBoxes2
            // 
            modbusCheckBoxes2.AutoSize = true;
            modbusCheckBoxes2.Location = new Point(181, 133);
            modbusCheckBoxes2.Name = "modbusCheckBoxes2";
            modbusCheckBoxes2.Size = new Size(83, 29);
            modbusCheckBoxes2.TabIndex = 15;
            modbusCheckBoxes2.Text = "Coil 3";
            modbusCheckBoxes2.UseVisualStyleBackColor = true;
            // 
            // modbusCheckBoxes1
            // 
            modbusCheckBoxes1.AutoSize = true;
            modbusCheckBoxes1.Location = new Point(181, 96);
            modbusCheckBoxes1.Name = "modbusCheckBoxes1";
            modbusCheckBoxes1.Size = new Size(83, 29);
            modbusCheckBoxes1.TabIndex = 14;
            modbusCheckBoxes1.Text = "Coil 2";
            modbusCheckBoxes1.UseVisualStyleBackColor = true;
            // 
            // modbusCheckBoxes0
            // 
            modbusCheckBoxes0.AutoSize = true;
            modbusCheckBoxes0.Location = new Point(181, 61);
            modbusCheckBoxes0.Name = "modbusCheckBoxes0";
            modbusCheckBoxes0.Size = new Size(83, 29);
            modbusCheckBoxes0.TabIndex = 13;
            modbusCheckBoxes0.Text = "Coil 1";
            modbusCheckBoxes0.UseVisualStyleBackColor = true;
            // 
            // modbusConnectionStatusLabel
            // 
            modbusConnectionStatusLabel.AutoSize = true;
            modbusConnectionStatusLabel.Location = new Point(37, 301);
            modbusConnectionStatusLabel.Name = "modbusConnectionStatusLabel";
            modbusConnectionStatusLabel.Size = new Size(59, 25);
            modbusConnectionStatusLabel.TabIndex = 18;
            modbusConnectionStatusLabel.Text = "label1";
            // 
            // websocketstatus
            // 
            websocketstatus.AutoSize = true;
            websocketstatus.Location = new Point(37, 264);
            websocketstatus.Name = "websocketstatus";
            websocketstatus.Size = new Size(59, 25);
            websocketstatus.TabIndex = 19;
            websocketstatus.Text = "label1";
            // 
            // Subscriber
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(websocketstatus);
            Controls.Add(modbusConnectionStatusLabel);
            Controls.Add(modbusCheckBoxes4);
            Controls.Add(modbusCheckBoxes3);
            Controls.Add(modbusCheckBoxes2);
            Controls.Add(modbusCheckBoxes1);
            Controls.Add(modbusCheckBoxes0);
            Controls.Add(lstMessages);
            Controls.Add(chkLED5);
            Controls.Add(chkLED4);
            Controls.Add(chkLED3);
            Controls.Add(chkLED2);
            Controls.Add(chkLED1);
            Name = "Subscriber";
            Text = "Subscriber";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lstMessages;
        private CheckBox chkLED5;
        private CheckBox chkLED4;
        private CheckBox chkLED3;
        private CheckBox chkLED2;
        private CheckBox chkLED1;
        private CheckBox modbusCheckBoxes4;
        private CheckBox modbusCheckBoxes3;
        private CheckBox modbusCheckBoxes2;
        private CheckBox modbusCheckBoxes1;
        private CheckBox modbusCheckBoxes0;
        private Label modbusConnectionStatusLabel;
        private Label websocketstatus;
    }
}
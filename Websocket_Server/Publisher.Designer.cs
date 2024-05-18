namespace Websocket_Server
{
    partial class Publisher
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
            websocketstatus = new Label();
            SuspendLayout();
            // 
            // lstMessages
            // 
            lstMessages.FormattingEnabled = true;
            lstMessages.ItemHeight = 25;
            lstMessages.Location = new Point(318, 59);
            lstMessages.Name = "lstMessages";
            lstMessages.Size = new Size(389, 329);
            lstMessages.TabIndex = 6;
            // 
            // chkLED5
            // 
            chkLED5.AutoSize = true;
            chkLED5.Location = new Point(40, 207);
            chkLED5.Name = "chkLED5";
            chkLED5.Size = new Size(83, 29);
            chkLED5.TabIndex = 16;
            chkLED5.Text = "LED 5";
            chkLED5.UseVisualStyleBackColor = true;
            // 
            // chkLED4
            // 
            chkLED4.AutoSize = true;
            chkLED4.Location = new Point(40, 169);
            chkLED4.Name = "chkLED4";
            chkLED4.Size = new Size(83, 29);
            chkLED4.TabIndex = 15;
            chkLED4.Text = "LED 4";
            chkLED4.UseVisualStyleBackColor = true;
            // 
            // chkLED3
            // 
            chkLED3.AutoSize = true;
            chkLED3.Location = new Point(40, 130);
            chkLED3.Name = "chkLED3";
            chkLED3.Size = new Size(83, 29);
            chkLED3.TabIndex = 14;
            chkLED3.Text = "LED 3";
            chkLED3.UseVisualStyleBackColor = true;
            // 
            // chkLED2
            // 
            chkLED2.AutoSize = true;
            chkLED2.Location = new Point(40, 94);
            chkLED2.Name = "chkLED2";
            chkLED2.Size = new Size(83, 29);
            chkLED2.TabIndex = 13;
            chkLED2.Text = "LED 2";
            chkLED2.UseVisualStyleBackColor = true;
            // 
            // chkLED1
            // 
            chkLED1.AutoSize = true;
            chkLED1.Location = new Point(40, 58);
            chkLED1.Name = "chkLED1";
            chkLED1.Size = new Size(83, 29);
            chkLED1.TabIndex = 12;
            chkLED1.Text = "LED 1";
            chkLED1.UseVisualStyleBackColor = true;
            // 
            // websocketstatus
            // 
            websocketstatus.AutoSize = true;
            websocketstatus.Location = new Point(40, 252);
            websocketstatus.Name = "websocketstatus";
            websocketstatus.Size = new Size(59, 25);
            websocketstatus.TabIndex = 20;
            websocketstatus.Text = "label1";
            // 
            // Publisher
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(websocketstatus);
            Controls.Add(chkLED5);
            Controls.Add(chkLED4);
            Controls.Add(chkLED3);
            Controls.Add(chkLED2);
            Controls.Add(chkLED1);
            Controls.Add(lstMessages);
            Name = "Publisher";
            Text = "Publisher";
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
        private Label websocketstatus;
    }
}
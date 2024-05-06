namespace MektepTagam
{
    partial class loginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            loginTextBox = new TextBox();
            passwordTextBox = new TextBox();
            loginLabel = new Label();
            passwordLabel = new Label();
            loginButton = new Button();
            versionLabel = new Label();
            SuspendLayout();
            // 
            // loginTextBox
            // 
            loginTextBox.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            loginTextBox.Location = new Point(189, 53);
            loginTextBox.MaxLength = 11;
            loginTextBox.Multiline = true;
            loginTextBox.Name = "loginTextBox";
            loginTextBox.Size = new Size(213, 63);
            loginTextBox.TabIndex = 0;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            passwordTextBox.Location = new Point(189, 216);
            passwordTextBox.Multiline = true;
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(213, 63);
            passwordTextBox.TabIndex = 1;
            // 
            // loginLabel
            // 
            loginLabel.AutoSize = true;
            loginLabel.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            loginLabel.Location = new Point(61, 64);
            loginLabel.Name = "loginLabel";
            loginLabel.Size = new Size(77, 31);
            loginLabel.TabIndex = 2;
            loginLabel.Text = "Логин";
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            passwordLabel.Location = new Point(61, 238);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(93, 31);
            passwordLabel.TabIndex = 3;
            passwordLabel.Text = "Пароль";
            // 
            // loginButton
            // 
            loginButton.Location = new Point(235, 352);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(129, 61);
            loginButton.TabIndex = 4;
            loginButton.Text = "Войти";
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += loginButton_Click;
            // 
            // versionLabel
            // 
            versionLabel.AutoSize = true;
            versionLabel.Location = new Point(437, 31);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new Size(0, 20);
            versionLabel.TabIndex = 5;
            // 
            // loginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(591, 449);
            Controls.Add(versionLabel);
            Controls.Add(loginButton);
            Controls.Add(passwordLabel);
            Controls.Add(loginLabel);
            Controls.Add(passwordTextBox);
            Controls.Add(loginTextBox);
            Name = "loginForm";
            Text = "Form1";
            Load += loginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox loginTextBox;
        private TextBox passwordTextBox;
        private Label loginLabel;
        private Label passwordLabel;
        private Button loginButton;
        private Label versionLabel;
    }
}
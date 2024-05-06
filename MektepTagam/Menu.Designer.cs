namespace MektepTagam
{
    partial class Menu
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
            codeTextBox = new TextBox();
            enterTokenButton = new Button();
            menuStrip1 = new MenuStrip();
            файлToolStripMenuItem = new ToolStripMenuItem();
            настройкиToolStripMenuItem = new ToolStripMenuItem();
            reportsToolStripMenuItem = new ToolStripMenuItem();
            cashRegisterReportToolStripMenuItem = new ToolStripMenuItem();
            refreshDataButton = new Button();
            label1 = new Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // codeTextBox
            // 
            codeTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            codeTextBox.Location = new Point(83, 109);
            codeTextBox.Multiline = true;
            codeTextBox.Name = "codeTextBox";
            codeTextBox.Size = new Size(600, 209);
            codeTextBox.TabIndex = 0;
            // 
            // enterTokenButton
            // 
            enterTokenButton.Location = new Point(643, 338);
            enterTokenButton.Name = "enterTokenButton";
            enterTokenButton.Size = new Size(157, 109);
            enterTokenButton.TabIndex = 1;
            enterTokenButton.UseVisualStyleBackColor = true;
            enterTokenButton.Click += enterTokenButton_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { файлToolStripMenuItem, настройкиToolStripMenuItem, reportsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            файлToolStripMenuItem.Size = new Size(59, 24);
            файлToolStripMenuItem.Text = "Файл";
            // 
            // настройкиToolStripMenuItem
            // 
            настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            настройкиToolStripMenuItem.Size = new Size(98, 24);
            настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // reportsToolStripMenuItem
            // 
            reportsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cashRegisterReportToolStripMenuItem });
            reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            reportsToolStripMenuItem.Size = new Size(73, 24);
            reportsToolStripMenuItem.Text = "Отчеты";
            // 
            // cashRegisterReportToolStripMenuItem
            // 
            cashRegisterReportToolStripMenuItem.Name = "cashRegisterReportToolStripMenuItem";
            cashRegisterReportToolStripMenuItem.Size = new Size(209, 26);
            cashRegisterReportToolStripMenuItem.Text = "Кассовые смены";
            cashRegisterReportToolStripMenuItem.Click += cashRegisterReportToolStripMenuItem_Click;
            // 
            // refreshDataButton
            // 
            refreshDataButton.Location = new Point(12, 418);
            refreshDataButton.Name = "refreshDataButton";
            refreshDataButton.Size = new Size(194, 29);
            refreshDataButton.TabIndex = 3;
            refreshDataButton.Text = "Refresh Data";
            refreshDataButton.UseVisualStyleBackColor = true;
            refreshDataButton.Click += refreshDataButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.ForestGreen;
            label1.Location = new Point(69, 55);
            label1.Name = "label1";
            label1.Size = new Size(614, 38);
            label1.TabIndex = 4;
            label1.Text = "Карта ученика (QR код или пластиковая карта)";
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(refreshDataButton);
            Controls.Add(enterTokenButton);
            Controls.Add(codeTextBox);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Menu";
            Text = "Menu";
            Load += Menu_Load_1;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox codeTextBox;
        private Button enterTokenButton;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem файлToolStripMenuItem;
        private ToolStripMenuItem настройкиToolStripMenuItem;
        private Button refreshDataButton;
        private ToolStripMenuItem reportsToolStripMenuItem;
        private ToolStripMenuItem cashRegisterReportToolStripMenuItem;
        private Label label1;
    }
}
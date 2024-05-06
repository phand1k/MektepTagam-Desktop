namespace MektepTagam
{
    partial class DishSelectionForm
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
            buttonConfirm = new Button();
            labelTotalSum = new Label();
            balanceInfoLabel = new Label();
            dataGridViewDishes = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDishes).BeginInit();
            SuspendLayout();
            // 
            // buttonConfirm
            // 
            buttonConfirm.Location = new Point(535, 519);
            buttonConfirm.Name = "buttonConfirm";
            buttonConfirm.Size = new Size(258, 75);
            buttonConfirm.TabIndex = 1;
            buttonConfirm.Text = "Оплата";
            buttonConfirm.UseVisualStyleBackColor = true;
            buttonConfirm.Click += buttonConfirm_Click;
            // 
            // labelTotalSum
            // 
            labelTotalSum.AutoSize = true;
            labelTotalSum.BackColor = SystemColors.Highlight;
            labelTotalSum.Location = new Point(75, 156);
            labelTotalSum.Name = "labelTotalSum";
            labelTotalSum.Size = new Size(125, 20);
            labelTotalSum.TabIndex = 2;
            labelTotalSum.Text = "Итоговая сумма:";
            // 
            // balanceInfoLabel
            // 
            balanceInfoLabel.AutoSize = true;
            balanceInfoLabel.BackColor = SystemColors.MenuHighlight;
            balanceInfoLabel.ForeColor = SystemColors.Desktop;
            balanceInfoLabel.Location = new Point(75, 120);
            balanceInfoLabel.Name = "balanceInfoLabel";
            balanceInfoLabel.Size = new Size(61, 20);
            balanceInfoLabel.TabIndex = 3;
            balanceInfoLabel.Text = "Баланс:";
            // 
            // dataGridViewDishes
            // 
            dataGridViewDishes.BackgroundColor = SystemColors.ActiveCaption;
            dataGridViewDishes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDishes.Location = new Point(75, 179);
            dataGridViewDishes.Name = "dataGridViewDishes";
            dataGridViewDishes.RowHeadersWidth = 51;
            dataGridViewDishes.RowTemplate.Height = 29;
            dataGridViewDishes.Size = new Size(1007, 334);
            dataGridViewDishes.TabIndex = 4;
            // 
            // DishSelectionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1762, 766);
            Controls.Add(dataGridViewDishes);
            Controls.Add(balanceInfoLabel);
            Controls.Add(labelTotalSum);
            Controls.Add(buttonConfirm);
            Name = "DishSelectionForm";
            Text = "DishSelectionForm";
            Load += DishSelectionForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewDishes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button buttonConfirm;
        private Label labelTotalSum;
        private Label balanceInfoLabel;
        private DataGridView dataGridViewDishes;
    }
}
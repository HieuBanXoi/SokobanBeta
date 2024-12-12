namespace StatisticsForm
{
    partial class StatisticsForm2
    {
        // Các control
        private System.Windows.Forms.Label lblSteps;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblMoveHistory;

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
            this.lblSteps = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblMoveHistory = new System.Windows.Forms.Label();

            // 
            // lblSteps
            // 
            this.lblSteps.AutoSize = true;
            this.lblSteps.Location = new System.Drawing.Point(12, 20);
            this.lblSteps.Name = "lblSteps";
            this.lblSteps.Size = new System.Drawing.Size(74, 15);
            this.lblSteps.TabIndex = 0;
            this.lblSteps.Text = "Số bước: 0";

            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(12, 50);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(75, 15);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "Thời gian: 0 giây";

            // 
            // lblMoveHistory
            // 
            this.lblMoveHistory.AutoSize = true;
            this.lblMoveHistory.Location = new System.Drawing.Point(12, 80);
            this.lblMoveHistory.Name = "lblMoveHistory";
            this.lblMoveHistory.Size = new System.Drawing.Size(89, 15);
            this.lblMoveHistory.TabIndex = 2;
            this.lblMoveHistory.Text = "Lịch sử di chuyển";

            // 
            // StatisticsForm2
            // 
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.lblSteps);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblMoveHistory);
            this.Name = "StatisticsForm2";
            this.Text = "Thông Tin Thống Kê";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}


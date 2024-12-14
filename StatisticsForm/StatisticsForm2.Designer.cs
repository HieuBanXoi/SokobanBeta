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
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSteps
            // 
            this.lblSteps.AutoSize = true;
            this.lblSteps.BackColor = System.Drawing.Color.Transparent;
            this.lblSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteps.Location = new System.Drawing.Point(12, 86);
            this.lblSteps.Name = "lblSteps";
            this.lblSteps.Size = new System.Drawing.Size(137, 29);
            this.lblSteps.TabIndex = 0;
            this.lblSteps.Text = "Số bước: 0";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(12, 126);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(206, 29);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "Thời gian: 0 giây";
            // 
            // lblMoveHistory
            // 
            this.lblMoveHistory.AutoSize = true;
            this.lblMoveHistory.BackColor = System.Drawing.Color.Transparent;
            this.lblMoveHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoveHistory.Location = new System.Drawing.Point(12, 165);
            this.lblMoveHistory.Name = "lblMoveHistory";
            this.lblMoveHistory.Size = new System.Drawing.Size(213, 29);
            this.lblMoveHistory.TabIndex = 2;
            this.lblMoveHistory.Text = "Lịch sử di chuyển";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Mistral", 36F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(88, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(340, 71);
            this.label1.TabIndex = 3;
            this.label1.Text = "Level Completed";
            // 
            // StatisticsForm2
            // 
            this.AutoScroll = true;
            this.BackgroundImage = global::StatisticsForm.Properties.Resources.StatisticsFormBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(483, 413);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSteps);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblMoveHistory);
            this.DoubleBuffered = true;
            this.Name = "StatisticsForm2";
            this.Text = "StatisticsForm2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
    }
}


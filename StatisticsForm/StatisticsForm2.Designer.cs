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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsForm2));
            this.lblSteps = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblMoveHistory = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNextLevel = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.panelHistory = new System.Windows.Forms.Panel();
            this.panelHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSteps
            // 
            this.lblSteps.AutoSize = true;
            this.lblSteps.BackColor = System.Drawing.Color.Beige;
            this.lblSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteps.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSteps.Location = new System.Drawing.Point(12, 99);
            this.lblSteps.Name = "lblSteps";
            this.lblSteps.Size = new System.Drawing.Size(159, 32);
            this.lblSteps.TabIndex = 0;
            this.lblSteps.Text = "Steps: 0";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.Beige;
            this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTime.Location = new System.Drawing.Point(12, 139);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(238, 34);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "Time: 0 seconds";
            // 
            // lblMoveHistory
            // 
            this.lblMoveHistory.AutoSize = true;
            this.lblMoveHistory.BackColor = System.Drawing.Color.Transparent;
            this.lblMoveHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoveHistory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMoveHistory.Location = new System.Drawing.Point(3, 9);
            this.lblMoveHistory.Name = "lblMoveHistory";
            this.lblMoveHistory.Size = new System.Drawing.Size(215, 32);
            this.lblMoveHistory.TabIndex = 2;
            this.lblMoveHistory.Text = "Moving History";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Mistral", 36F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(88, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(340, 71);
            this.label1.TabIndex = 3;
            this.label1.Text = "Level Completed";
            // 
            // btnNextLevel
            // 
            this.btnNextLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextLevel.BackColor = System.Drawing.Color.Beige;
            this.btnNextLevel.Location = new System.Drawing.Point(351, 295);
            this.btnNextLevel.Name = "btnNextLevel";
            this.btnNextLevel.Size = new System.Drawing.Size(117, 36);
            this.btnNextLevel.TabIndex = 4;
            this.btnNextLevel.Text = "Next Level";
            this.btnNextLevel.UseVisualStyleBackColor = false;
            this.btnNextLevel.Click += new System.EventHandler(this.btnNextLevel_Click);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.BackColor = System.Drawing.Color.Beige;
            this.btnBack.Location = new System.Drawing.Point(351, 348);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(117, 36);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "Back to menu";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // panelHistory
            // 
            this.panelHistory.AutoScroll = true;
            this.panelHistory.BackColor = System.Drawing.Color.Beige;
            this.panelHistory.Controls.Add(this.lblMoveHistory);
            this.panelHistory.Location = new System.Drawing.Point(12, 176);
            this.panelHistory.Name = "panelHistory";
            this.panelHistory.Size = new System.Drawing.Size(333, 208);
            this.panelHistory.TabIndex = 6;
            // 
            // StatisticsForm2
            // 
            this.AutoScroll = true;
            this.BackgroundImage = global::StatisticsForm.Properties.Resources.StatisticsFormBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(483, 413);
            this.Controls.Add(this.panelHistory);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnNextLevel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSteps);
            this.Controls.Add(this.lblTime);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StatisticsForm2";
            this.Text = "StatisticsForm2";
            this.panelHistory.ResumeLayout(false);
            this.panelHistory.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNextLevel;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel panelHistory;
    }
}


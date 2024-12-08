﻿namespace SokobanBeta
{
    partial class MenuForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.btnInstructions = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(200, 150);  // Điều chỉnh vị trí nút theo form lớn hơn
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(200, 50);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start Game";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);

            // 
            // btnInstructions
            // 
            this.btnInstructions.Location = new System.Drawing.Point(200, 230);  // Điều chỉnh vị trí nút theo form lớn hơn
            this.btnInstructions.Name = "btnInstructions";
            this.btnInstructions.Size = new System.Drawing.Size(200, 50);
            this.btnInstructions.TabIndex = 1;
            this.btnInstructions.Text = "Instructions";
            this.btnInstructions.UseVisualStyleBackColor = true;
            this.btnInstructions.Click += new System.EventHandler(this.BtnInstructions_Click);

            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(200, 310);  // Điều chỉnh vị trí nút theo form lớn hơn
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(200, 50);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);

            // 
            // MenuForm
            // 
            this.ClientSize = new System.Drawing.Size(600, 450);  // Phóng to 1.5 lần (600x450)
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnInstructions);
            this.Controls.Add(this.btnStart);
            this.Name = "MenuForm";
            this.Text = "Sokoban Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnInstructions;
        private System.Windows.Forms.Button btnExit;
    }
}

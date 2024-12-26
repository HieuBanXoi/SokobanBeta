// LogIn.Designer.cs
using SokobanBeta.Properties;

namespace SokobanBeta
{
    partial class LogIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelSelectionForm));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.textBoxPlayerName = new System.Windows.Forms.TextBox();
            this.continueButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxPlayerName
            // 
            this.textBoxPlayerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayerName.Location = new System.Drawing.Point(241, 232);
            this.textBoxPlayerName.Name = "textBoxPlayerName";
            this.textBoxPlayerName.Size = new System.Drawing.Size(176, 30);
            this.textBoxPlayerName.TabIndex = 0;
            // 
            // continueButton
            // 
            this.continueButton.BackColor = System.Drawing.Color.Transparent;
            this.continueButton.CausesValidation = false;
            this.continueButton.FlatAppearance.BorderSize = 0;
            this.continueButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.continueButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.continueButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.continueButton.ForeColor = System.Drawing.Color.Transparent;
            this.continueButton.Location = new System.Drawing.Point(474, 367);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(114, 40);
            this.continueButton.TabIndex = 1;
            this.continueButton.TabStop = false;
            this.continueButton.UseVisualStyleBackColor = false;
            this.continueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // LogIn
            // 
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.textBoxPlayerName);
            this.Controls.Add(this.continueButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LogIn";
            this.Text = "Sokoban";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPlayerName;
        private System.Windows.Forms.Button continueButton;
    }
}



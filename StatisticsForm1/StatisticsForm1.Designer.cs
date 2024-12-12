using System.Windows.Forms;

namespace StatisticsForm
{
    partial class StatisticsForm1
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

        
        private DataGridView dataGridView1;
        private Button btnLoadStatistics;
        private Control lblTitle;

        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnLoadStatistics = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 42);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 346);
            this.dataGridView1.TabIndex = 0;

            // 
            // btnLoadStatistics
            // 
            this.btnLoadStatistics.Location = new System.Drawing.Point(12, 394);
            this.btnLoadStatistics.Name = "btnLoadStatistics";
            this.btnLoadStatistics.Size = new System.Drawing.Size(120, 40);
            this.btnLoadStatistics.TabIndex = 1;
            this.btnLoadStatistics.Text = "Load Statistics";
            this.btnLoadStatistics.UseVisualStyleBackColor = true;
            this.btnLoadStatistics.Click += new System.EventHandler(this.btnLoadStatistics_Click);

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 22);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "High Score Statistics";

            // 
            // StatisticsForm1
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnLoadStatistics);
            this.Controls.Add(this.dataGridView1);
            this.Name = "StatisticsForm1";
            this.Text = "Statistics Form";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

            #endregion
        }
}


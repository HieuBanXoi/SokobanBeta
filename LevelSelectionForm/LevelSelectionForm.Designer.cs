using System;

namespace SokobanBeta
{
    partial class LevelSelectionForm
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">True if managed resources should be disposed; otherwise, false.</param>
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
        /// the content of this method with the code editor.
        /// </summary>
        private void InitializeComponent(Button btnBack)
        {
            this.SuspendLayout();
            // 
            // LevelSelectionForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 500);
            this.Name = "LevelSelectionForm";
            this.Text = "Select Level";
            this.Load += new System.EventHandler(this.LevelSelectionForm_Load);
            this.ResumeLayout(false);

            // Thêm các nút chọn màn chơi
            for (int i = 1; i <= 5; i++)
            {
                Button btnLevel = new Button();
                btnLevel.Text = "Level " + i;
                btnLevel.Size = new System.Drawing.Size(200, 50);
                btnLevel.Location = new System.Drawing.Point(100, 50 + (i - 1) * 60); // Sắp xếp các nút theo chiều dọc
                btnLevel.Click += (sender, e) => BtnLevel_Click(sender, e, i);  // Truyền tham số i vào sự kiện
                this.Controls.Add(btnLevel);
            }

            // Thêm nút "Back"
            Button btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Size = new System.Drawing.Size(200, 50);
            btnBack.Location = new System.Drawing.Point(100, 380); // Đặt vị trí nút "Back" ở phía dưới cùng
            btnBack.Click += BtnBack_Click;
            this.Controls.Add(btnBack);
        }

        #endregion

        private void LevelSelectionForm_Load(object sender, EventArgs e)
        {
            // Các thao tác nếu cần khi form được tải
        }
    }
}

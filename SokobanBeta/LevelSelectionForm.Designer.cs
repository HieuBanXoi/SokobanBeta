using System;
using System.Windows.Forms;

namespace SokobanBeta
{
    partial class LevelSelectionForm
    {
        private System.ComponentModel.IContainer components = null;

        // Dọn dẹp tài nguyên đang được sử dụng
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Phương thức khởi tạo giao diện form
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Thêm các nút chọn màn chơi
            for (int i = 1; i <= 5; i++)
            {
                int level = i; // Biến tạm để giữ giá trị chính xác cho mỗi nút
                Button btnLevel = new Button
                {
                    Text = $"Level {level}",
                    Size = new System.Drawing.Size(200, 50),
                    Location = new System.Drawing.Point(100, 50 + (level - 1) * 60)
                };
                btnLevel.Click += (sender, e) => BtnLevel_Click(sender, e, level);
                this.Controls.Add(btnLevel);
            }

            // Thêm nút "Back"
            Button btnBack = new Button
            {
                Text = "Back",
                Size = new System.Drawing.Size(200, 50),
                Location = new System.Drawing.Point(100, 380)
            };
            btnBack.Click += BtnBack_Click;
            this.Controls.Add(btnBack);

            // Thuộc tính form
            this.ClientSize = new System.Drawing.Size(400, 500);
            this.Name = "LevelSelectionForm";
            this.Text = "Select Level";
            //this.Load += new System.EventHandler(this.LevelSelectionForm_Load);

            this.ResumeLayout(false);
        }

        #endregion
    }
}

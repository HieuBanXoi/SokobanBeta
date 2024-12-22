// LogIn.cs
using System;
using System.Windows.Forms;
using System.Drawing;

namespace SokobanBeta
{
    public partial class LogIn : Form
    {
        private Image backgroundImage;
        public string playerName { get; set; }
        public LogIn()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadBackgroundImage();
        }

        // Tải hình ảnh vào bộ nhớ
        private void LoadBackgroundImage()
        {
            backgroundImage = Properties.Resources.Log_In;
        }

        // Vẽ hình ảnh trong phương thức Paint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (backgroundImage != null)
            {
                // Vẽ hình ảnh lên form, phù hợp với kích thước form
                e.Graphics.DrawImage(backgroundImage, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
            }
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            string playerName = textBoxPlayerName.Text.Trim();

            if (string.IsNullOrEmpty(playerName))
            {
                playerName = textBoxPlayerName.Text.Trim();
                MessageBox.Show("Please enter your name to continue.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lưu tên người chơi (tạm thời lưu trong một biến toàn cục hoặc cấu hình nếu cần)
            Properties.Settings.Default.PlayerName = playerName;
            Properties.Settings.Default.Save();

            // Mở form MenuForm
            MenuForm menuForm = new MenuForm();
            menuForm.Show();
            this.Hide();
        }
        // Đảm bảo tài nguyên hình ảnh được giải phóng khi form đóng
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (backgroundImage != null)
            {
                backgroundImage.Dispose(); // Giải phóng tài nguyên hình ảnh
            }
        }
        public static class NavigationHelper
        {
            public static Form PreviousForm { get; set; }
        }
    }
}



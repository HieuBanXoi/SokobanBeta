using System;
using System.Windows.Forms;
using System.Drawing;
using StatisticsForm;

namespace SokobanBeta
{
    public partial class MenuForm : Form
    {
        private Image backgroundImage;

        public MenuForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadBackgroundImage();
        }

        // Tải hình ảnh vào bộ nhớ
        private void LoadBackgroundImage()
        {
            backgroundImage = Properties.Resources.SOKOBAN;
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

        // Sự kiện khi nhấn nút "Start Game"
        private void BtnStart_Click(object sender, EventArgs e)
        {
            // Mở form LevelSelectionForm khi nhấn Start Game
            NavigationHelper.PreviousForm = this;
            LevelSelectionForm levelSelectionForm = new LevelSelectionForm();
            levelSelectionForm.Show();  // Hiển thị form chọn màn chơi
            this.Hide();  // Ẩn MenuForm
        }

        // Sự kiện khi nhấn nút "Instructions"
        private void BtnInstructions_Click(object sender, EventArgs e)
        {
            // Mở form InstructionsForm
            NavigationHelper.PreviousForm = this;
            Instructions instructionsForm = new Instructions(); 
            instructionsForm.Show();
            this.Hide();
        }
        // Sự kiện khi nhấn nút "Exit"

        private void BtnExit_Click(object sender, EventArgs e)

        {

            Application.Exit(); // Thoát ứng dụng

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

        private void btnHighScores_Click(object sender, EventArgs e)
        {
            NavigationHelper.PreviousForm = this;
            StatisticsForm1 statisticsForm1 = new StatisticsForm1();
            statisticsForm1.Show();
            this.Hide();
        }
    }
}

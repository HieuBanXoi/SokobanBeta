using System;
using System.Windows.Forms;
using Level_1;
using static SokobanBeta.MenuForm;

namespace SokobanBeta
{
    public partial class LevelSelectionForm : Form
    {
        public LevelSelectionForm()
        {
            InitializeComponent(); // Gọi phương thức InitializeComponent từ file Designer
            SetWindowSize();       // Gọi phương thức để thiết lập kích thước cửa sổ
        }

        // Phương thức thiết lập kích thước cửa sổ
        private void SetWindowSize()
        {
            var screenWidth = Screen.PrimaryScreen.Bounds.Width;
            var screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int windowWidth = (int)(screenWidth * 0.8);  // 80% chiều rộng màn hình
            int windowHeight = (int)(screenHeight * 0.6); // 60% chiều cao màn hình

            this.Width = windowWidth;
            this.Height = windowHeight;
            this.StartPosition = FormStartPosition.CenterScreen; // Căn giữa cửa sổ
        }

        private void BtnLevel_Click(object sender, EventArgs e, int level)
        {
            // Tạo một form mới để chơi level tương ứng
            Main gameForm = new Main();
            gameForm.LoadSpecificLevel(level); // Tải level tương ứng
            gameForm.Show();

            // Ẩn menu chính để tập trung vào trò chơi
            this.Hide();

            // Khi form game đóng, hiển thị lại menu chính
            gameForm.FormClosed += (s, args) => this.Show();
        }


        // Xử lý sự kiện khi nhấn nút "Back"
        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (NavigationHelper.PreviousForm != null)
            {
                NavigationHelper.PreviousForm.Show(); // Hiển thị Form trước đó
                this.Close();
            }
        }

    }
}

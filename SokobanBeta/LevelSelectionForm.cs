using System;
using System.Windows.Forms;

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

        // Xử lý sự kiện khi nhấn vào một nút chọn màn chơi
        private void BtnLevel_Click(object sender, EventArgs e, int level)
        {
            MessageBox.Show($"You selected Level {level}", $"Level {level}");
            // Thêm logic chuyển sang màn chơi ở đây nếu cần
            this.Close();
        }

        // Xử lý sự kiện khi nhấn nút "Back"
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form hiện tại để quay lại menu chính
        }

        //private void LevelSelectionForm_Load(object sender, EventArgs e)
        //{
        //    // Xử lý logic khi form được tải (nếu cần)
        //}
    }
}

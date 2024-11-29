using System;
using System.Windows.Forms;

namespace SokobanBeta
{
    public partial class LevelSelectionForm : Form
    {
        public LevelSelectionForm()
        {
            InitializeComponent(btnBack);
            SetWindowSize();  // Gọi phương thức để thiết lập kích thước cửa sổ
        }

        // Phương thức thiết lập kích thước cửa sổ theo tỷ lệ
        private void SetWindowSize()
        {
            // Lấy kích thước màn hình chính
            var screenWidth = Screen.PrimaryScreen.Bounds.Width;
            var screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Tính toán kích thước cửa sổ theo tỷ lệ
            int windowWidth = (int)(screenWidth * 0.8); // 80% chiều rộng màn hình
            int windowHeight = (int)(screenHeight * 0.6); // 60% chiều cao màn hình

            // Thiết lập kích thước cửa sổ
            this.Width = windowWidth;
            this.Height = windowHeight;

            // Đảm bảo cửa sổ luôn ở giữa màn hình
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Sự kiện khi nhấn vào một nút chọn màn chơi
        private void BtnLevel_Click(object sender, EventArgs e, int level)
        {
            // Chuyển đến màn chơi tương ứng. Ví dụ:
            MessageBox.Show("You selected Level " + level, "Level " + level);

            // Tại đây, bạn có thể chuyển đến form của từng màn chơi cụ thể
            // Ví dụ, nếu bạn có form cho mỗi màn chơi, có thể gọi:
            // new GameForm(level).Show();
            // Hoặc, nếu chỉ có một form chơi chung cho tất cả các màn, bạn có thể truyền tham số `level` vào form đó
            // Ví dụ: new GameForm(level).Show(); 

            this.Close(); // Đóng form chọn màn chơi
        }

        // Sự kiện khi nhấn nút "Back"
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form chọn màn chơi và quay lại menu chính
        }
    }
}

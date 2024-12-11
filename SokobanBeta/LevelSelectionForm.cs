using System;
using System.Windows.Forms;
using System.Drawing;
using MainSys;
using static SokobanBeta.MenuForm;

namespace SokobanBeta
{
    public partial class LevelSelectionForm : Form
    {
        private Image backgroundImage; // Thuộc tính để chứa ảnh nền

        public LevelSelectionForm()
        {
            InitializeComponent(); // Gọi phương thức InitializeComponent từ file Designer
            SetWindowSize();       // Thiết lập kích thước cửa sổ
            LoadBackgroundImage(); // Tải hình ảnh nền
        }

        // Phương thức thiết lập kích thước cửa sổ
        private void SetWindowSize()
        {
            this.Width = 1000;  // Đặt chiều rộng cửa sổ nhỏ hơn
            this.Height = 600;  // Đặt chiều cao cửa sổ
            this.StartPosition = FormStartPosition.CenterScreen; // Căn giữa cửa sổ
        }

        private void LoadBackgroundImage()
        {
            backgroundImage = Properties.Resources.SOKOBAN__Slection_Level;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (backgroundImage != null)
            {
                backgroundImage.Dispose(); // Giải phóng tài nguyên hình ảnh
            }
        }

        // Vẽ hình ảnh nền trong phương thức Paint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (backgroundImage != null)
            {
                e.Graphics.DrawImage(backgroundImage, 0, 0, this.ClientSize.Width, this.ClientSize.Height); // Vẽ hình ảnh lên form
            }
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

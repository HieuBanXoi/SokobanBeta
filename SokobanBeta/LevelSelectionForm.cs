using System;
using System.Windows.Forms;
using System.Drawing;
using SokobanBeta;
using static SokobanBeta.MenuForm;

namespace SokobanBeta
{
    /// <summary>
    /// Lớp LevelSelectionForm quản lý giao diện chọn level trong trò chơi Sokoban.
    /// </summary>
    public partial class LevelSelectionForm : Form
    {
        private Image _backgroundImage; // Thuộc tính để chứa ảnh nền

        /// <summary>
        /// Constructor khởi tạo LevelSelectionForm.
        /// </summary>
        public LevelSelectionForm()
        {
            InitializeComponent(); // Gọi phương thức InitializeComponent từ file Designer
            SetWindowSize();       // Thiết lập kích thước cửa sổ
            LoadBackgroundImage(); // Tải hình ảnh nền
        }

        /// <summary>
        /// Thiết lập kích thước cửa sổ.
        /// </summary>
        private void SetWindowSize()
        {
            this.Width = 1000;  // Đặt chiều rộng cửa sổ nhỏ hơn
            this.Height = 600;  // Đặt chiều cao cửa sổ
            this.MaximumSize = new Size(1000, 600);
            this.MinimumSize = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen; // Căn giữa cửa sổ
        }

        /// <summary>
        /// Tải hình ảnh nền từ tài nguyên.
        /// </summary>
        private void LoadBackgroundImage()
        {
            try
            {
                _backgroundImage = Properties.Resources.SOKOBAN_Slection_Level;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải hình ảnh nền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi form bị đóng.
        /// </summary>
        /// <param name="e">Thông tin sự kiện.</param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (_backgroundImage != null)
            {
                _backgroundImage.Dispose(); // Giải phóng tài nguyên hình ảnh
            }
        }

        /// <summary>
        /// Vẽ hình ảnh nền trong phương thức Paint.
        /// </summary>
        /// <param name="e">Thông tin PaintEventArgs.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {
                if (_backgroundImage != null)
                {
                    e.Graphics.DrawImage(_backgroundImage, 0, 0, this.ClientSize.Width, this.ClientSize.Height); // Vẽ hình ảnh lên form
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi vẽ hình nền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút chọn level.
        /// </summary>
        /// <param name="sender">Nguồn sự kiện.</param>
        /// <param name="e">Thông tin sự kiện.</param>
        /// <param name="level">Số thứ tự level cần chơi.</param>
        private void BtnLevelClick(object sender, EventArgs e, int level)
        {
            try
            {
                // Tạo một form mới để chơi level tương ứng
                MainGame gameForm = new MainGame
                {
                    PlayerName = Properties.Settings.Default.PlayerName
                };
                gameForm.LoadSpecificLevel(level); // Tải level tương ứng
                gameForm.Show();

                // Ẩn menu chính để tập trung vào trò chơi
                this.Hide();

                // Khi form game đóng, hiển thị lại menu chính
                gameForm.FormClosed += (s, args) => this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở level: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút "Back".
        /// </summary>
        /// <param name="sender">Nguồn sự kiện.</param>
        /// <param name="e">Thông tin sự kiện.</param>
        private void BtnBackClick(object sender, EventArgs e)
        {
            try
            {
                if (NavigationHelper.PreviousForm != null)
                {
                    NavigationHelper.PreviousForm.Show(); // Hiển thị Form trước đó
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy form trước đó để quay lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi quay lại menu trước: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

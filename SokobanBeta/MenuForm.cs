using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace SokobanBeta
{
    /// <summary>
    /// Form chính của ứng dụng Sokoban.
    /// </summary>
    public partial class MenuForm : Form
    {
        /// <summary>
        /// Hình nền của <see cref="MenuForm"/>.
        /// </summary>
        private Image _backgroundImage;

        /// <summary>
        /// Tên người chơi.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Khởi tạo một thể hiện của <see cref="MenuForm"/>.
        /// </summary>
        public MenuForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadBackgroundImage();
        }

        /// <summary>
        /// Tải hình ảnh nền vào bộ nhớ.
        /// </summary>
        private void LoadBackgroundImage()
        {
            _backgroundImage = Properties.Resources.SOKOBAN;
        }

        /// <summary>
        /// Vẽ hình ảnh nền lên form khi sự kiện Paint được gọi.
        /// </summary>
        /// <param name="e">Thông tin về sự kiện Paint.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_backgroundImage != null)
            {
                // Vẽ hình ảnh lên form, phù hợp với kích thước form
                e.Graphics.DrawImage(_backgroundImage, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút "Start Game".
        /// </summary>
        /// <param name="sender">Nguồn gửi sự kiện.</param>
        /// <param name="e">Thông tin về sự kiện.</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                // Xóa các tệp lưu game trước đó
                File.Delete("save_game1.txt");
                File.Delete("save_game2.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa tệp lưu game: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Mở form LogIn
            NavigationHelper.PreviousForm = this;
            LogIn logInForm = new LogIn();
            logInForm.Show();
            this.Hide();  // Ẩn MenuForm
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút "Instructions".
        /// </summary>
        /// <param name="sender">Nguồn gửi sự kiện.</param>
        /// <param name="e">Thông tin về sự kiện.</param>
        private void BtnInstructions_Click(object sender, EventArgs e)
        {
            // Mở form Instructions
            NavigationHelper.PreviousForm = this;
            Instructions instructionsForm = new Instructions();
            instructionsForm.Show();
            this.Hide();
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút "Exit".
        /// </summary>
        /// <param name="sender">Nguồn gửi sự kiện.</param>
        /// <param name="e">Thông tin về sự kiện.</param>
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Thoát ứng dụng
        }

        /// <summary>
        /// Đảm bảo tài nguyên hình ảnh được giải phóng khi form đóng.
        /// </summary>
        /// <param name="e">Thông tin về sự kiện đóng form.</param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (_backgroundImage != null)
            {
                _backgroundImage.Dispose(); // Giải phóng tài nguyên hình ảnh
            }
        }

        /// <summary>
        /// Lớp hỗ trợ điều hướng giữa các form.
        /// </summary>
        public static class NavigationHelper
        {
            /// <summary>
            /// Form trước đó.
            /// </summary>
            public static Form PreviousForm { get; set; }
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút "High Scores".
        /// </summary>
        /// <param name="sender">Nguồn gửi sự kiện.</param>
        /// <param name="e">Thông tin về sự kiện.</param>
        private void BtnHighScores_Click(object sender, EventArgs e)
        {
            // Mở form StatisticsForm1
            NavigationHelper.PreviousForm = this;
            StatisticsForm1 statisticsForm1 = new StatisticsForm1();
            statisticsForm1.Show();
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút "Continue".
        /// </summary>
        /// <param name="sender">Nguồn gửi sự kiện.</param>
        /// <param name="e">Thông tin về sự kiện.</param>
        private void BtnContinue_Click(object sender, EventArgs e)
        {
            NavigationHelper.PreviousForm = this;

            if (File.Exists("save_game1.txt"))
            {
                try
                {
                    using (StreamReader reader = new StreamReader("save_game1.txt"))
                    {
                        PlayerName = reader.ReadLine();
                    }

                    LevelSelectionForm levelSelectionForm = new LevelSelectionForm();
                    levelSelectionForm.Show();
                    this.Hide();  // Ẩn MenuForm
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải save_game1.txt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (File.Exists("save_game2.txt"))
            {
                try
                {
                    using (StreamReader reader = new StreamReader("save_game2.txt"))
                    {
                        PlayerName = reader.ReadLine();
                    }

                    LevelSelectionForm levelSelectionForm = new LevelSelectionForm();
                    levelSelectionForm.Show();
                    this.Hide();  // Ẩn MenuForm
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải save_game2.txt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu.", "Yêu cầu nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

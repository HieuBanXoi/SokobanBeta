using System;
using System.Windows.Forms;

namespace SokobanBeta
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Sự kiện khi nhấn nút "Start Game"
        private void BtnStart_Click(object sender, EventArgs e)
        {
            // Mở form LevelSelectionForm khi nhấn Start Game
            LevelSelectionForm levelSelectionForm = new LevelSelectionForm();
            levelSelectionForm.Show();  // Hiển thị form chọn màn chơi
            this.Hide();  // Ẩn MenuForm
        }

        // Sự kiện khi nhấn nút "Instructions"
        private void BtnInstructions_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Use the arrow keys to move the player and push boxes to the goals.", "Instructions");
        }

        // Sự kiện khi nhấn nút "Exit"
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Thoát ứng dụng
        }
    }
}

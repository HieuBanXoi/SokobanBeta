using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatisticsForm
{
    public partial class StatisticsForm1 : Form
    {
        
        private Dictionary<int, (string playerName, int steps)> highScores = new Dictionary<int, (string playerName, int steps)>();

        public StatisticsForm1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadHighScores(); // Tải điểm cao khi form được khởi tạo
        }

        private void LoadHighScores()
        {
            // Tải điểm cao từ file hoặc nguồn dữ liệu
            if (System.IO.File.Exists("high_scores.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines("high_scores.txt");
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split(':');
                    if (parts.Length == 3)
                    {
                        int level = int.Parse(parts[0].Trim());
                        string playerName = parts[1].Trim();
                        int score = int.Parse(parts[2].Trim());
                        highScores[level] = (playerName, score);
                    }
                }
            }

            // Hiển thị điểm cao trên DataGridView
            DisplayStatistics();
        }

        private void DisplayStatistics()
        {
            // Clear DataGridView before adding data
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // Thiết lập cột cho DataGridView
            dataGridView1.Columns.Add("Level", "Level");
            dataGridView1.Columns.Add("PlayerName", "Player Name"); // Cột tên người chơi
            dataGridView1.Columns.Add("HighScore", "High Score");

            // Thêm dữ liệu vào DataGridView
            foreach (var score in highScores)
            {
                dataGridView1.Rows.Add(score.Key, score.Value.Item1, score.Value.Item2); // Gán level, tên, điểm
            }
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form thống kê
        }

    }
}

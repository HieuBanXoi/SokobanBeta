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
    public partial class StatisticsForm2 : Form
    {
        private int steps;
        private int timeTaken; // Thời gian hoàn thành (tính bằng giây)
        private List<string> moveHistory;

        // Hàm khởi tạo nhận các tham số để hiển thị
        public StatisticsForm2(int steps, int timeTaken, List<string> moveHistory)
        {
            InitializeComponent();
            this.steps = steps;
            this.timeTaken = timeTaken;
            this.moveHistory = moveHistory;

            // Hiển thị thông tin lên form
            DisplayStatistics();
        }

        // Hàm hiển thị thông tin thống kê lên form
        private void DisplayStatistics()
        {
            // Hiển thị số bước đã đi
            lblSteps.Text = $"Số bước: {steps}";

            // Hiển thị thời gian hoàn thành
            lblTime.Text = $"Thời gian: {timeTaken} giây";

            // Hiển thị lịch sử di chuyển
            string historyText = "Lịch sử di chuyển:\n";
            foreach (string move in moveHistory)
            {
                historyText += move + "\n";
            }

            lblMoveHistory.Text = historyText;
        }
    }
}

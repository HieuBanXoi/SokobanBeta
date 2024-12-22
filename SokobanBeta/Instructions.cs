using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SokobanBeta.MenuForm;

namespace SokobanBeta
{
    public partial class Instructions : Form
    {
        private Image backgroundImage;

        public Instructions()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadBackgroundImage();
        }

        // Tải hình ảnh vào bộ nhớ
        private void LoadBackgroundImage()
        {
            backgroundImage = Properties.Resources.Instructions;
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

        // Xử lí nút Back
        private void Btn_Back_Click(object sender, EventArgs e)
        {
            if (NavigationHelper.PreviousForm != null)
            {
                NavigationHelper.PreviousForm.Show(); // Hiển thị Form trước đó
                this.Close();
            }
        }
    }
}


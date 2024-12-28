using System.Windows.Forms;

namespace SokobanBeta
{
    partial class LevelSelectionForm
    {
        private System.ComponentModel.IContainer components = null;

        // Dọn dẹp tài nguyên đang được sử dụng
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Phương thức khởi tạo giao diện form
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelSelectionForm));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.SuspendLayout();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Khóa kích thước form
            this.MaximizeBox = false;
            // Kích thước các nút
            int buttonWidth = 200;
            int buttonHeight = 50;
            int verticalSpacing = 20; // Khoảng cách giữa các nút theo chiều dọc
            int moveUpPixels = 38; // Di chuyển các nút lên trên 0.5cm (khoảng 38 pixels)
            int moveDownPixels = 75; // Di chuyển nút "Back" xuống dưới 1cm (khoảng 75 pixels)

            // Vị trí cố định của các nút (dễ dàng thay đổi nếu cần)
            int startX = 400; // Vị trí bắt đầu của các nút từ trái sang phải
            int startY = 150 + moveUpPixels; // Nút Level 1 và Level 2 sẽ được di chuyển lên trên 0.5cm (38 pixels)

            // Nút Level 1
            Button btnLevel1 = new Button
            {
                Text = "Level 1",
                Size = new System.Drawing.Size(buttonWidth, buttonHeight),
                Location = new System.Drawing.Point(startX, startY), // Vị trí cố định
                Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))
            };
            btnLevel1.Click += (sender, e) => BtnLevelClick(sender, e, 1);
            this.Controls.Add(btnLevel1);
           
            // Nút Level 2
            Button btnLevel2 = new Button
            {
                Text = "Level 2",
                Size = new System.Drawing.Size(buttonWidth, buttonHeight),
                Location = new System.Drawing.Point(startX, startY + buttonHeight + verticalSpacing), // Cộng thêm chiều cao của nút và khoảng cách
                Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))
            };
            btnLevel2.Click += (sender, e) => BtnLevelClick(sender, e, 2);
            this.Controls.Add(btnLevel2);

            // Nút "Back"
            Button btnBack = new Button
            {
                Text = "Back",
                Size = new System.Drawing.Size(buttonWidth, buttonHeight),
                Location = new System.Drawing.Point(startX, startY + (2 * buttonHeight) + (2 * verticalSpacing) + moveDownPixels), // Nút "Back" xuống dưới 1cm (75 pixels)
                Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))
            };
            btnBack.Click += BtnBackClick;
            this.Controls.Add(btnBack);

            // Thuộc tính form
            this.ClientSize = new System.Drawing.Size(1000, 600); // Kích thước cửa sổ
            this.Name = "LevelSelectionForm";
            this.Text = "Select Level";

            this.ResumeLayout(false);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Map;

namespace Level_1
{
    public partial class Main : Form
    {
        private GameMapManager mapManager;
        private char[,] map;
        private int playerX , playerY ; // Vị trí bắt đầu của người chơi
        private int cellSize = 64;           // Kích thước mỗi ô
        private Image wallImage;
        private Image boxImage;
        private Image goalImage;
        private Image playerImage;
        private Image placedBoxImage;
        private int steps; // Số bước đã đi
        private Stack<char[,]> mapHistory; // Lịch sử map
        private Stack<(int playerX, int playerY)> playerHistory; // Lịch sử vị trí người chơi
        enum TrangThai { OnGoal, OutGoal };

        TrangThai p_TrangThai = TrangThai.OutGoal; // Trạng thái của Player
        TrangThai b_TrangThai = TrangThai.OutGoal; // Trạng thái của Box

        public Main()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            steps = 0;
            mapHistory = new Stack<char[,]>();
            playerHistory = new Stack<(int, int)>();
            playerImage = Image.FromFile("C:\\Users\\Truong Minh Hoang\\source\\repos\\SokobanBeta\\Level 1\\Resources\\player.png");
            goalImage = Image.FromFile("C:\\Users\\Truong Minh Hoang\\source\\repos\\SokobanBeta\\Level 1\\Resources\\goal.png");
            boxImage = Image.FromFile("C:\\Users\\Truong Minh Hoang\\source\\repos\\SokobanBeta\\Level 1\\Resources\\Box.png");
            wallImage = Image.FromFile("C:\\Users\\Truong Minh Hoang\\source\\repos\\SokobanBeta\\Level 1\\Resources\\wall.png");
            placedBoxImage = Image.FromFile("C:\\Users\\Truong Minh Hoang\\source\\repos\\SokobanBeta\\Level 1\\Resources\\placedBox.png");
            LoadMaps();
            LoadCurrentMap();

            //InitializeUI();
            this.KeyDown += SokobanForm_KeyDown;
            this.Paint += SokobanForm_Paint;


        }

        public void LoadSpecificLevel(int level)
        {
            mapManager.SetCurrentMap(level - 1); // Chuyển đến level tương ứng (index bắt đầu từ 0)
            LoadCurrentMap(); // Tải map của level đó
        }


        private void LoadMaps()
        {
            mapManager = new GameMapManager();
            mapManager.AddMap(new GameMap("Level 1", new char[,] {
                { '#', '#', '#', '#', '#' },
                { '#', 'P', ' ', ' ', '#' },
                { '#', ' ', 'B', 'G', '#' },
                { '#', '#', '#', '#', '#' }
            }, 1, 1));

            mapManager.AddMap(new GameMap("Level 2", new char[,] {
                { '#', '#', '#', '#', '#', '#', '#' },
                { '#', ' ', ' ', ' ', ' ', ' ', '#' },
                { '#', 'P', 'B', ' ', 'B', ' ', '#' },
                { '#', ' ', 'G', '#', 'G', ' ', '#' },
                { '#', ' ', ' ', '#', ' ', ' ', '#' },
                { '#', ' ', ' ', '#', ' ', ' ', '#' },
                { '#', '#', '#', '#', '#', '#', '#' }
            }, 2, 1));

            mapManager.AddMap(new GameMap("Level 3", new char[,] {
                { ' ', ' ', '#', '#', '#', '#', '#', ' ' },
                { '#', '#', '#', ' ', ' ', ' ', '#', ' ' },
                { '#', 'G', 'P', 'B', ' ', ' ', '#', ' ' },
                { '#', '#', '#', ' ', 'B', 'G', '#', ' ' },
                { '#', 'G', '#', '#', 'B', ' ', '#', ' ' },
                { '#', ' ', '#', ' ', 'G', ' ', '#', '#' },
                { '#', 'B', ' ', 'A', 'B', 'B', 'G', '#' },
                { '#', ' ', ' ', ' ', 'G', ' ', ' ', '#' },
                { '#', '#', '#', '#', '#', '#', '#', '#' }
            }, 2, 2));

            mapManager.AddMap(new GameMap("Level 4", new char[,] {
                { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', ' ' },
                { '#', 'G', 'G', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', '#', '#', '#' },
                { '#', 'G', 'G', ' ', ' ', '#', ' ', 'B', ' ', ' ', 'B', ' ', ' ', '#' },
                { '#', 'G', 'G', ' ', ' ', '#', 'B', '#', '#', '#', '#', ' ', ' ', '#' },
                { '#', 'G', 'G', ' ', ' ', ' ', ' ', 'P', ' ', '#', '#', ' ', ' ', '#' },
                { '#', 'G', 'G', ' ', ' ', '#', ' ', '#', ' ', ' ', 'B', ' ', '#', '#' },
                { '#', '#', '#', '#', '#', '#', ' ', '#', '#', 'B', ' ', 'B', ' ', '#' },
                { ' ', ' ', '#', ' ', 'B', ' ', ' ', 'B', ' ', 'B', ' ', 'B', ' ', '#' },
                { ' ', ' ', '#', ' ', ' ', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', '#' },
                { ' ', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#' }
            }, 4, 7));

            mapManager.AddMap(new GameMap("Level 5", new char[,] {
                { ' ', '#', '#', '#', '#', '#', ' ', ' ' },
                { ' ', '#', ' ', 'P', ' ', '#', '#', '#' },
                { '#', '#', ' ', '#', 'B', ' ', ' ', '#' },
                { '#', ' ', 'A', 'G', ' ', 'G', ' ', '#' },
                { '#', ' ', ' ', 'B', 'B', ' ', '#', '#' },
                { '#', '#', '#', ' ', '#', 'G', '#', ' ' },
                { ' ', ' ', '#', ' ', ' ', ' ', '#', ' ' },
                { ' ', ' ', '#', '#', '#', '#', '#', ' ' }
            }, 1, 3));

        }
        
        private void LoadCurrentMap()
        {
            GameMap currentMap = mapManager.GetCurrentMap();
            if (currentMap != null)
            {
                map = currentMap.MapData;
                playerX = currentMap.PlayerStartX;
                playerY = currentMap.PlayerStartY;
                this.StartPosition = FormStartPosition.WindowsDefaultLocation;
                this.Text = currentMap.Name;
                UpdateFormSize();
                this.Invalidate(); // Vẽ lại màn hình
            }
        }

        private void UpdateFormSize()
        {
            if (map != null)
            {
                this.Width = map.GetLength(1) * cellSize + 16; // Độ rộng form
                this.Height = map.GetLength(0) * cellSize + 39; // Chiều cao form
            }
        }
        private void SokobanForm_KeyDown(object sender, KeyEventArgs e)
        {
            int dx = 0, dy = 0;
            if (e.KeyCode == Keys.Up) dx = -1;
            else if (e.KeyCode == Keys.Down) dx = 1;
            else if (e.KeyCode == Keys.Left) dy = -1;
            else if (e.KeyCode == Keys.Right) dy = 1;
            else if (e.KeyCode == Keys.Z && mapHistory.Count > 0) // Lùi lại (phím Z)
            {
                UndoLastMove();
                return;
            }

            // Lưu trạng thái trước khi di chuyển
            SaveCurrentState();
            // Xử lý di chuyển người chơi
            int newX = playerX + dx, newY = playerY + dy;
            if (ProcessMove(newX, newY, dx, dy))
            {
                steps++; // Tăng số bước khi di chuyển hợp lệ
            }

            
            // Kiểm tra hoàn thành level
            CheckWinCondition();

            this.Invalidate(); // Vẽ lại màn hình
        }
        private void SaveCurrentState()
        {
            // Lưu map hiện tại
            char[,] currentMapState = (char[,])map.Clone();
            mapHistory.Push(currentMapState);

            // Lưu vị trí người chơi hiện tại
            playerHistory.Push((playerX, playerY));
        }

        private void UndoLastMove()
        {
            // Phục hồi map từ lịch sử
            map = mapHistory.Pop();

            // Phục hồi vị trí người chơi từ lịch sử
            (playerX, playerY) = playerHistory.Pop();

            // Giảm số bước (nếu cần)
            if (steps > 0) steps--;

            this.Invalidate(); // Vẽ lại màn hình
        }
        private bool ProcessMove(int newX, int newY, int dx, int dy)
        {
            // Kiểm tra di chuyển hợp lệ
            if (map[newX, newY] == 'G')
            {
                if (p_TrangThai == TrangThai.OnGoal)
                {
                    map[playerX, playerY] = 'G';
                }
                else map[playerX, playerY] = ' ';
                playerX = newX;
                playerY = newY;
                map[playerX, playerY] = 'P';
                p_TrangThai = TrangThai.OnGoal;
            }
            if (map[newX, newY] == ' ')
            {
                switch (p_TrangThai)
                {
                    case TrangThai.OutGoal:
                        map[playerX, playerY] = ' ';
                        break;
                    case TrangThai.OnGoal:
                        map[playerX, playerY] = 'G';
                        p_TrangThai = TrangThai.OutGoal;
                        break;
                    default:
                        break;
                }
                playerX = newX;
                playerY = newY;
                map[playerX, playerY] = 'P';
            }
            else if (map[newX, newY] == 'B' || map[newX, newY] == 'A')
            {
                if (map[newX, newY] == 'A')
                {
                    b_TrangThai = TrangThai.OnGoal;
                }
                // Kiểm tra nếu hộp có thể được đẩy
                int boxNewX = newX + dx, boxNewY = newY + dy;
                if (map[boxNewX, boxNewY] == 'G')
                {
                    if (b_TrangThai == TrangThai.OnGoal)
                    {
                        if (p_TrangThai == TrangThai.OnGoal)
                        {
                            map[playerX, playerY] = 'G';
                        }
                        else
                        {
                            map[playerX, playerY] = ' ';
                        }
                        p_TrangThai = TrangThai.OnGoal;
                    }
                    else
                    {
                        if (p_TrangThai == TrangThai.OnGoal)
                        {
                            map[playerX, playerY] = 'G';
                        }
                        else
                        {
                            map[playerX, playerY] = ' ';
                        }
                        p_TrangThai = TrangThai.OutGoal;
                    }
                    map[newX, newY] = 'P';            // Vị trí mới của người chơi
                    map[boxNewX, boxNewY] = 'A';      // Vị trí mới của hộp
                    playerX = newX;
                    playerY = newY;
                    b_TrangThai = TrangThai.OutGoal;
                }
                if (map[boxNewX, boxNewY] == ' ')
                {
                    if (b_TrangThai == TrangThai.OnGoal)
                    {
                        if (p_TrangThai == TrangThai.OnGoal)
                        {
                            map[playerX, playerY] = 'G';
                        }
                        else
                        {
                            map[playerX, playerY] = ' ';
                        }
                        p_TrangThai = TrangThai.OnGoal;
                    }
                    else
                    {
                        if (p_TrangThai == TrangThai.OnGoal)
                        {
                            map[playerX, playerY] = 'G';
                        }
                        else
                        {
                            map[playerX, playerY] = ' ';
                        }
                        p_TrangThai = TrangThai.OutGoal;
                    }
                    map[newX, newY] = 'P';            // Vị trí mới của người chơi
                    map[boxNewX, boxNewY] = 'B';      // Vị trí mới của hộp
                    playerX = newX;
                    playerY = newY;
                    b_TrangThai = TrangThai.OutGoal;
                }
            }

            return true; // Trả về true nếu di chuyển hợp lệ
        }
        // Hàm kiểm tra điều kiện chiến thắng
        private void CheckWinCondition()
        {
                for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == 'G'||p_TrangThai==TrangThai.OnGoal ) // Nếu còn mục tiêu chưa có hộp
                    {
                        return; // Chưa hoàn thành
                    }
                }
            }
            this.Invalidate();
            // Nếu tất cả mục tiêu đã được lấp đầy
            MessageBox.Show("You Win!!!", "Level Completed");
            b_TrangThai = TrangThai.OutGoal;
            NextLevel();
        }
        private void NextLevel()
        {
            if (mapManager.MoveToNextMap())
            {
                LoadCurrentMap();
            }
            else
            {
                MessageBox.Show("All levels completed!", "Congratulations");
                Application.Exit();
            }
        }


        // Sự kiện vẽ giao diện trò chơi
        private void SokobanForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Rectangle rect = new Rectangle(y * cellSize, x * cellSize, cellSize, cellSize);
                    char cell = map[x, y];

                    if (cell == '#')
                        g.DrawImage(wallImage, rect);  // Vẽ tường
                    else if (cell == 'P')
                        g.DrawImage(playerImage, rect);  // Vẽ người chơi
                    else if (cell == 'B')
                        g.DrawImage(boxImage, rect);  // Vẽ hộp
                    else if (cell == 'G')
                        g.DrawImage(goalImage, rect);  // Vẽ mục tiêu
                    else if (cell == 'A')
                        g.DrawImage(placedBoxImage, rect);  // Vẽ hộp trên đích
                    else
                        g.FillRectangle(Brushes.White, rect); // Ô trống

                    g.DrawRectangle(Pens.Gray, rect); // Viền ô
                }
            }
            // Hiển thị số bước đã đi
            g.DrawString($"Steps: {steps}", new Font("Arial", 14), Brushes.Black, new PointF(10, 10));
        }

        


    }
}

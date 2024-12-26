using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Map;
using StatisticsForm;
using NAudio.Wave; // Thêm namespace NAudio

namespace MainSys
{
    public partial class Main : Form
    {
        private GameMapManager mapManager;
        private char[,] map; // Khai báo map
        private int playerX, playerY; // Vị trí bắt đầu của người chơi
        private int cellSize = 64; // Kích thước mỗi ô
        private int currentLevel;
        private Image wallImage;
        private Image boxImage;
        private Image goalImage;
        private Image playerImage;
        private Image placedBoxImage;
        private int steps; // Số bước đã đi
        private Stack<char[,]> mapHistory; // Lịch sử map
        private Stack<(int playerX, int playerY)> playerHistory; // Lịch sử vị trí người chơi
        private Stack<TrangThai> playerStateHistory; // Lịch sử trạng thái người chơi
        private char[,] initialMap; // Lưu bản đồ gốc
        private int initialPlayerX, initialPlayerY; // Lưu vị trí ban đầu của người chơi
        private TrangThai initialPlayerState; // Lưu trạng thái ban đầu của người chơi
        private TrangThai initialBoxState; // Lưu trạng thái ban đầu của box
        private bool result;// Lưu kết quả khi qua màn
        public string saveFile; // File lưu trạng thái game

        enum TrangThai { OnGoal, OutGoal };
        private TrangThai p_TrangThai = TrangThai.OutGoal; // Trạng thái của Player
        private TrangThai b_TrangThai = TrangThai.OutGoal; // Trạng thái của Box
        public string playerName; // Tên player
        private string highScoreFile = "high_scores.txt"; // File lưu điểm cao
        private Dictionary<int, (string playerName, int steps)> highScores = new Dictionary<int, (string playerName, int steps)>();
        private List<string> moveHistory = new List<string>();
        private DateTime startTime;

        // Thêm biến cho NAudio
        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;

        public Main()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Màn hình không bị chớp mỗi khi di chuyển nhân vật

            // Khởi tạo 
            steps = 0;
            mapHistory = new Stack<char[,]>();
            playerHistory = new Stack<(int, int)>();
            playerStateHistory = new Stack<TrangThai>();
            // Load ảnh từ Resource
            playerImage = Properties.Resources.player;
            goalImage = Properties.Resources.goal;
            boxImage = Properties.Resources.box;
            wallImage = Properties.Resources.wall;
            placedBoxImage = Properties.Resources.placedBox;
            // Khởi tạo 
            LoadMaps();
            LoadHighScores();

            this.KeyDown += SokobanForm_KeyDown;
            this.Paint += SokobanForm_Paint;
        }

        // Hàm phát âm thanh bắt đầu màn chơi
        private void PlayLevelStartSound()
        {
            try
            {
                // Giải phóng tài nguyên âm thanh hiện tại nếu có
                if (waveOutDevice != null)
                {
                    waveOutDevice.Stop();
                    waveOutDevice.Dispose();
                    waveOutDevice = null;
                }
                if (audioFileReader != null)
                {
                    audioFileReader.Dispose();
                    audioFileReader = null;
                }

                // Đường dẫn tệp âm thanh
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "GameSound.mp3");

                // Kiểm tra sự tồn tại của tệp âm thanh
                if (!File.Exists(audioPath))
                {
                    MessageBox.Show($"Không tìm thấy tệp âm thanh: {audioPath}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Khởi tạo trình phát âm thanh
                waveOutDevice = new WaveOutEvent();
                audioFileReader = new AudioFileReader(audioPath);
                waveOutDevice.Init(audioFileReader);
                waveOutDevice.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể phát âm thanh bắt đầu màn chơi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tải level cụ thể
        public void LoadSpecificLevel(int level)
        {
            currentLevel = level;
            saveFile = "save_game" + level + ".txt";
            if (System.IO.File.Exists("save_game" + level + ".txt"))
            {
                LoadGame(level);
            }
            else
            {
                mapManager.SetCurrentMap(level - 1); // Chuyển đến level tương ứng (index bắt đầu từ 0)
                LoadCurrentMap(); // Tải map của level đó
            }
        }

        // Sử dụng hàm LoadMapsFromFile trong LoadMaps
        private void LoadMaps()
        {
            mapManager = new GameMapManager();

            // Tải bản đồ từ file
            mapManager.LoadMapsFromFile("Resources\\Level1.txt");
            mapManager.LoadMapsFromFile("Resources\\Level2.txt");
            //LoadMapsFromFile("Resources\\Level3.txt");  // Dễ dàng mở rộng các map mới
            //LoadMapsFromFile("Resources\\Level4.txt");
            //LoadMapsFromFile("Resources\\Level5.txt");
        }

        // Tải map hiện tại
        private void LoadCurrentMap()
        {
            GameMap currentMap = mapManager.GetCurrentMap();
            if (currentMap != null)
            {
                map = currentMap.MapData;
                playerX = currentMap.PlayerStartX;
                playerY = currentMap.PlayerStartY;
                UpdateFormSize();
                this.Text = currentMap.Name;
                // Lưu trạng thái ban đầu
                initialMap = (char[,])map.Clone();
                initialPlayerX = playerX;
                initialPlayerY = playerY;
                initialPlayerState = p_TrangThai;
                initialBoxState = b_TrangThai;

                // Phát âm thanh bắt đầu màn chơi
                PlayLevelStartSound();
            }
        }

        // Căn chỉnh form
        private void UpdateFormSize()
        {
            if (map != null)
            {
                this.Text = "Level: " + currentLevel;
                this.Icon = Properties.Resources.Icon; // Icon form
                this.Width = map.GetLength(1) * cellSize + 16; // Độ rộng form
                this.Height = map.GetLength(0) * cellSize + 39; // Chiều cao form
                this.FormBorderStyle = FormBorderStyle.FixedSingle; // Khóa kích thước form
                this.MaximizeBox = false; // Khóa kích thước form
            }
        }

        // Nhận dữ liệu từ bàn phím
        private void SokobanForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (steps == 0)
            {
                startTime = DateTime.Now; // Đánh dấu thời gian bắt đầu
            }
            int dx = 0, dy = 0;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W) dx = -1;
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S) dx = 1;
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) dy = -1;
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) dy = 1;
            else if (e.KeyCode == Keys.Z && mapHistory.Count > 0) // Lùi lại (phím Z)
            {
                UndoLastMove(); // Gọi hàm UndoLastMove
                return;
            }
            else if (e.KeyCode == Keys.R) // Restart (phím R)
            {
                RestartGame(); // Gọi hàm restart
                return;
            }
            else
            {
                return; // Bỏ qua các phím khác
            }
            // Lưu trạng thái trước khi di chuyển
            SaveCurrentState();
            // Xử lý di chuyển người chơi
            int newX = playerX + dx, newY = playerY + dy;
            if (ProcessMove(newX, newY, dx, dy)) // Nếu di chuyển hợp lệ
            {
                steps++; // Tăng số bước khi di chuyển hợp lệ
                string move = $"Bước {steps}: {e.KeyCode}";
                moveHistory.Add(move); // Lưu lịch sử di chuyển
                // Nếu bạn muốn thêm âm thanh di chuyển ở đây, bạn có thể làm như đã hướng dẫn trước đó
            }
            else // Nếu di chuyển không hợp lệ
            {
                // Xóa lịch sử gần nhất của map, player, trạng thái
                mapHistory.Pop();
                playerHistory.Pop();
                playerStateHistory.Pop();
            }

            CheckWinCondition(); // Kiểm tra hoàn thành level
            this.Invalidate(); // Vẽ lại màn hình
        }

        // Lưu vị trí, map, trạng thái hiện tại
        private void SaveCurrentState()
        {
            // Lưu map hiện tại
            char[,] currentMapState = (char[,])map.Clone();
            mapHistory.Push(currentMapState);

            // Lưu vị trí người chơi hiện tại
            playerHistory.Push((playerX, playerY));

            // Lưu trạng thái người chơi hiện tại
            playerStateHistory.Push(p_TrangThai);
        }

        //Quay lại bước trước đó
        private void UndoLastMove()
        {

            // Phục hồi map từ lịch sử
            map = mapHistory.Pop();

            // Phục hồi vị trí người chơi từ lịch sử
            (playerX, playerY) = playerHistory.Pop();

            // Phục hồi trạng thái người chơi từ lịch sử
            p_TrangThai = playerStateHistory.Pop();

            // Giảm số bước (nếu cần)
            if (steps > 0) steps--;

            // Xóa bước di chuyển cuối cùng khỏi moveHistory
            if (moveHistory.Count > 0)
            {
                moveHistory.RemoveAt(moveHistory.Count - 1);
            }
            this.Invalidate(); // Vẽ lại màn hình
        }

        // Chơi lại màn
        private void RestartGame()
        {
            // Khôi phục trạng thái ban đầu
            map = (char[,])initialMap.Clone();
            playerX = initialPlayerX;
            playerY = initialPlayerY;
            p_TrangThai = initialPlayerState;
            b_TrangThai = initialBoxState;

            mapHistory.Clear();
            playerHistory.Clear();
            playerStateHistory.Clear();
            moveHistory.Clear();
            steps = 0;

            this.Invalidate();
        }

        // Thực hiện di chuyển nhân vật theo logic game
        private bool ProcessMove(int newX, int newY, int dx, int dy)
        {
            if (map[newX, newY] == '#')
            {
                return false; // Không thực hiện di chuyển
            }
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
                // Kiểm tra nếu hộp có thể được đẩy
                int boxNewX = newX + dx, boxNewY = newY + dy;
                if (map[boxNewX, boxNewY] != '#')
                {
                    if (map[newX, newY] == 'A')
                    {
                        b_TrangThai = TrangThai.OnGoal;
                    }
                }
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
                    return true;
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
                    return true;
                }
                if (map[boxNewX, boxNewY] == '#' || map[boxNewX, boxNewY] == 'B' || map[boxNewX, boxNewY] == 'A')
                {
                    return false;
                }
            }

            return true; // Trả về true nếu di chuyển hợp lệ
        }

        // Kiểm tra điều kiện chiến thắng
        private void CheckWinCondition()
        {
            // Kiểm tra xem có còn mục tiêu nào chưa hoàn thành không
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == 'G' || p_TrangThai == TrangThai.OnGoal) // Nếu còn mục tiêu chưa có hộp hoặc người chơi đang đứng trên đích.
                    {
                        return; // Chưa hoàn thành
                    }
                }
            }

            // Khi đã hoàn thành màn chơi
            UpdateHighScore(mapManager.GetCurrentLevel(), steps); // Lưu số bước của màn chơi vào highscore nếu có
            this.Invalidate(); // Vẽ lại màn hình
            int timeTaken = (int)(DateTime.Now - startTime).TotalSeconds; // Tính thời gian hoàn thành màn chơi

            // Mở form StatisticsForm2 để hiển thị thống kê
            StatisticsForm2 statsForm = new StatisticsForm2(steps, timeTaken, moveHistory);
            statsForm.ShowDialog();
            result = statsForm.result; // Lấy kết quả từ button trong StatisticsForm2

            if (result)
            {
                // Chơi tiếp màn kế tiếp
                b_TrangThai = TrangThai.OutGoal;  // Đặt lại trạng thái hộp
                steps = 0;  // Đặt lại số bước về 0
                NextLevel(currentLevel + 1); // Tải màn kế tiếp
            }
            else
            {
                // Nếu không chơi tiếp, quay lại LevelSelectionForm
                this.Close(); // Đóng form game hiện tại
            }
            if (System.IO.File.Exists(saveFile))
            {
                System.IO.File.Delete(saveFile);
            }
        }

        // Chuyển sang level kế tiếp
        private void NextLevel(int level)
        {
            if (File.Exists("save_game" + level + ".txt"))
            {
                mapHistory.Clear();
                playerHistory.Clear();
                playerStateHistory.Clear();
                moveHistory.Clear();
                mapManager.MoveToNextMap();
                LoadSpecificLevel(level);
            }
            else
            {
                if (mapManager.MoveToNextMap())
                {
                    LoadCurrentMap(); // Tải map của màn tiếp theo
                }
                else
                {
                    this.Close();
                    MessageBox.Show("All levels completed!", "Congratulations");
                }
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
            if (highScores.ContainsKey(currentLevel))
            {
                // Hiển thị HighScore
                g.DrawString($"High Score: {highScores[currentLevel].steps} by {highScores[currentLevel].playerName}", new Font("Arial", 14), Brushes.Black, new PointF(10, 30));
            }

        }

        // Tải điểm cao từ file
        private void LoadHighScores()
        {
            if (System.IO.File.Exists(highScoreFile))
            {
                string[] lines = System.IO.File.ReadAllLines(highScoreFile);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split(':');
                    if (parts.Length == 3) // Đảm bảo có đủ thông tin (Level, PlayerName, Steps)
                    {
                        int level = int.Parse(parts[0].Trim());
                        string playerName = parts[1].Trim();
                        int steps = int.Parse(parts[2].Trim());
                        highScores[level] = (playerName, steps);
                    }
                }
            }
        }

        // Lưu điểm cao vào file
        private void SaveHighScores()
        {
            var lines = highScores.Select(kvp => $"{kvp.Key}: {kvp.Value.playerName}: {kvp.Value.steps}");
            System.IO.File.WriteAllLines(highScoreFile, lines);
        }

        // Cập nhật điểm cao cho level hiện tại
        private void UpdateHighScore(int level, int steps)
        {
            if (!highScores.ContainsKey(level) || steps < highScores[level].steps)
            {
                highScores[level] = (playerName, steps); // Cập nhật điểm cao mới
                SaveHighScores();
            }
        }

        private void SaveGame()
        {
            using (StreamWriter writer = new StreamWriter(saveFile))
            {
                writer.WriteLine(playerName);
                // Lưu map
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        writer.Write(map[x, y]);
                    }
                    writer.WriteLine();
                }
                writer.WriteLine("");
                // Lưu số bước
                writer.WriteLine(steps);

                // Lưu thời gian chơi
                int timeTaken = (int)(DateTime.Now - startTime).TotalSeconds;
                writer.WriteLine(timeTaken);

                // Lưu vị trí người chơi
                writer.WriteLine(playerX);
                writer.WriteLine(playerY);

                // Lưu trạng thái người chơi
                writer.WriteLine(p_TrangThai);

                // Lưu lịch sử di chuyển
                foreach (var move in moveHistory)
                {
                    writer.WriteLine(move);
                }
            }
        }

        public void LoadGame(int level)
        {
            using (StreamReader reader = new StreamReader(saveFile))
            {
                reader.ReadLine();
                // Tải map
                List<List<char>> loadedMap = new List<List<char>>();
                string line;
                while ((line = reader.ReadLine()) != null && !string.IsNullOrEmpty(line))
                {
                    List<char> row = line.ToList();
                    loadedMap.Add(row);
                }
                map = new char[loadedMap.Count, loadedMap[0].Count];
                for (int i = 0; i < loadedMap.Count; i++)
                {
                    for (int j = 0; j < loadedMap[i].Count; j++)
                    {
                        map[i, j] = loadedMap[i][j];
                    }
                }

                // Tải số bước
                steps = int.Parse(reader.ReadLine());

                // Tải thời gian chơi
                int timeTaken = int.Parse(reader.ReadLine());
                startTime = DateTime.Now.AddSeconds(-timeTaken);

                // Tải vị trí người chơi
                playerX = int.Parse(reader.ReadLine());
                playerY = int.Parse(reader.ReadLine());

                // Tải trạng thái người chơi
                p_TrangThai = (TrangThai)Enum.Parse(typeof(TrangThai), reader.ReadLine());

                // Tải lịch sử di chuyển
                moveHistory.Clear();
                while ((line = reader.ReadLine()) != null)
                {
                    moveHistory.Add(line);
                }

                // Cập nhật số bước
                this.Text = "Steps: " + steps;
                this.Invalidate(); // Vẽ lại game
            }
            UpdateFormSize();

            // Phát âm thanh bắt đầu màn chơi khi tải game
            PlayLevelStartSound();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            SaveGame(); // Lưu trạng thái game khi thoát

            // Giải phóng tài nguyên âm thanh
            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }
            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }
        }
    }
}

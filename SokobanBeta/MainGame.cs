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
using NAudio.Wave; // Thêm namespace NAudio

namespace SokobanBeta
{
    /// <summary>
    /// Lớp chính quản lý trò chơi Sokoban.
    /// </summary>
    public partial class MainGame : Form
    {
        private GameMapManager _mapManager;
        private char[,] _map; // Khai báo map
        private int _playerX, _playerY; // Vị trí bắt đầu của người chơi
        private int _cellSize = 64; // Kích thước mỗi ô
        private int _currentLevel;
        private Image _wallImage;
        private Image _boxImage;
        private Image _goalImage;
        private Image _playerImage;
        private Image _placedBoxImage;
        private int _steps; // Số bước đã đi
        private Stack<char[,]> _mapHistory; // Lịch sử map
        private Stack<(int playerX, int playerY)> _playerHistory; // Lịch sử vị trí người chơi
        private Stack<Status> _playerStateHistory; // Lịch sử trạng thái người chơi
        private char[,] _initialMap; // Lưu bản đồ gốc
        private int _initialPlayerX, _initialPlayerY; // Lưu vị trí ban đầu của người chơi
        private Status _initialPlayerState; // Lưu trạng thái ban đầu của người chơi
        private Status _initialBoxState; // Lưu trạng thái ban đầu của box
        private bool _result; // Lưu kết quả khi qua màn
        public string SaveFile { get; set; } // File lưu trạng thái game

        /// <summary>
        /// Enum trạng thái của Player và Box.
        /// </summary>
        public enum Status { OnGoal, OutGoal }

        private Status _playerStatus = Status.OutGoal; // Trạng thái của Player
        private Status _boxStatus = Status.OutGoal; // Trạng thái của Box
        public string PlayerName { get; set; } // Tên player
        private string _highScoreFile = "high_scores.txt"; // File lưu điểm cao
        private Dictionary<int, (string PlayerName, int Steps)> _highScores = new Dictionary<int, (string PlayerName, int Steps)>();
        private List<string> _moveHistory = new List<string>();
        private DateTime _startTime;

        // Thêm biến cho NAudio
        private IWavePlayer _waveOutDevice;
        private AudioFileReader _audioFileReader;

        /// <summary>
        /// Constructor khởi tạo MainGame.
        /// </summary>
        public MainGame()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Màn hình không bị chớp mỗi khi di chuyển nhân vật

            // Khởi tạo 
            _steps = 0;
            _mapHistory = new Stack<char[,]>();
            _playerHistory = new Stack<(int, int)>();
            _playerStateHistory = new Stack<Status>();

            // Load ảnh từ Resource
            _playerImage = Properties.Resources.player;
            _goalImage = Properties.Resources.goal;
            _boxImage = Properties.Resources.box;
            _wallImage = Properties.Resources.wall;
            _placedBoxImage = Properties.Resources.placedBox;

            // Khởi tạo 
            LoadMaps();
            LoadHighScores();

            this.KeyDown += OnKeyDownHandler;
            this.Paint += OnPaintHandler;
        }

        /// <summary>
        /// Hàm phát âm thanh bắt đầu màn chơi.
        /// </summary>
        private void PlayLevelStartSound()
        {
            try
            {
                // Giải phóng tài nguyên âm thanh hiện tại nếu có
                if (_waveOutDevice != null)
                {
                    _waveOutDevice.Stop();
                    _waveOutDevice.Dispose();
                    _waveOutDevice = null;
                }
                if (_audioFileReader != null)
                {
                    _audioFileReader.Dispose();
                    _audioFileReader = null;
                }

                // Lấy tệp âm thanh từ tài nguyên nhúng
                var soundStream = new MemoryStream();
                Properties.Resources.GameSound.CopyTo(soundStream); // "GameSound" là tên tệp âm thanh nhúng
                soundStream.Position = 0; // Đặt vị trí đọc lại từ đầu

                // Sử dụng WaveFileReader để đọc từ MemoryStream
                var waveReader = new WaveFileReader(soundStream);
                _waveOutDevice = new WaveOutEvent();
                _waveOutDevice.Init(waveReader);
                _waveOutDevice.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể phát âm thanh bắt đầu màn chơi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tải level cụ thể.
        /// </summary>
        /// <param name="level">Số thứ tự level cần tải.</param>
        public void LoadSpecificLevel(int level)
        {
            _currentLevel = level;
            SaveFile = $"save_game{level}.txt";
            if (File.Exists(SaveFile))
            {
                LoadGame(level);
            }
            else
            {
                _mapManager.SetCurrentMap(level - 1); // Chuyển đến level tương ứng (index bắt đầu từ 0)
                LoadCurrentMap(); // Tải map của level đó
            }
        }

        /// <summary>
        /// Tải các bản đồ từ file.
        /// </summary>
        private void LoadMaps()
        {
            _mapManager = new GameMapManager();

            // Tải bản đồ từ file
            _mapManager.LoadMapsFromFile("Resources\\Level1.txt");
            _mapManager.LoadMapsFromFile("Resources\\Level2.txt");
            // _mapManager.LoadMapsFromFile("Resources\\Level3.txt");  // Dễ dàng mở rộng các map mới
            // _mapManager.LoadMapsFromFile("Resources\\Level4.txt");
            // _mapManager.LoadMapsFromFile("Resources\\Level5.txt");
        }

        /// <summary>
        /// Tải map hiện tại.
        /// </summary>
        private void LoadCurrentMap()
        {
            GameMap currentMap = _mapManager.GetCurrentMap();
            if (currentMap != null)
            {
                _map = currentMap.MapData;
                _playerX = currentMap.PlayerStartX;
                _playerY = currentMap.PlayerStartY;
                UpdateFormSize();
                this.Text = currentMap.Name;

                // Lưu trạng thái ban đầu
                _initialMap = (char[,])_map.Clone();
                _initialPlayerX = _playerX;
                _initialPlayerY = _playerY;
                _initialPlayerState = _playerStatus;
                _initialBoxState = _boxStatus;

                // Phát âm thanh bắt đầu màn chơi
                PlayLevelStartSound();
            }
        }

        /// <summary>
        /// Căn chỉnh kích thước form dựa trên map.
        /// </summary>
        private void UpdateFormSize()
        {
            if (_map != null)
            {
                this.Text = $"Level: {_currentLevel}";
                this.Icon = Properties.Resources.Icon; // Icon form
                this.Width = _map.GetLength(1) * _cellSize + 16; // Độ rộng form
                this.Height = _map.GetLength(0) * _cellSize + 39; // Chiều cao form
                this.FormBorderStyle = FormBorderStyle.FixedSingle; // Khóa kích thước form
                this.MaximizeBox = false; // Khóa kích thước form
            }
        }

        /// <summary>
        /// Xử lý sự kiện nhấn phím.
        /// </summary>
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (_steps == 0)
            {
                _startTime = DateTime.Now; // Đánh dấu thời gian bắt đầu
            }

            int dx = 0, dy = 0;
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    dx = -1;
                    break;
                case Keys.Down:
                case Keys.S:
                    dx = 1;
                    break;
                case Keys.Left:
                case Keys.A:
                    dy = -1;
                    break;
                case Keys.Right:
                case Keys.D:
                    dy = 1;
                    break;
                case Keys.Z:
                    if (_mapHistory.Count > 0)
                    {
                        UndoLastMove(); // Gọi hàm UndoLastMove
                    }
                    return;
                case Keys.R:
                    RestartGame(); // Gọi hàm RestartGame
                    return;
                default:
                    return; // Bỏ qua các phím khác
            }

            // Lưu trạng thái trước khi di chuyển
            SaveCurrentState();

            // Xử lý di chuyển người chơi
            int newX = _playerX + dx, newY = _playerY + dy;
            if (ProcessMove(newX, newY, dx, dy)) // Nếu di chuyển hợp lệ
            {
                _steps++; // Tăng số bước khi di chuyển hợp lệ
                string move = $"Bước {_steps}: {e.KeyCode}";
                _moveHistory.Add(move); // Lưu lịch sử di chuyển
                // Nếu bạn muốn thêm âm thanh di chuyển ở đây, bạn có thể làm như đã hướng dẫn trước đó
            }
            else // Nếu di chuyển không hợp lệ
            {
                // Xóa lịch sử gần nhất của map, player, trạng thái
                _mapHistory.Pop();
                _playerHistory.Pop();
                _playerStateHistory.Pop();
            }

            CheckWinCondition(); // Kiểm tra hoàn thành level
            this.Invalidate(); // Vẽ lại màn hình
        }

        /// <summary>
        /// Lưu trạng thái hiện tại của game.
        /// </summary>
        private void SaveCurrentState()
        {
            // Lưu map hiện tại
            char[,] currentMapState = (char[,])_map.Clone();
            _mapHistory.Push(currentMapState);

            // Lưu vị trí người chơi hiện tại
            _playerHistory.Push((_playerX, _playerY));

            // Lưu trạng thái người chơi hiện tại
            _playerStateHistory.Push(_playerStatus);
        }

        /// <summary>
        /// Quay lại bước di chuyển trước đó.
        /// </summary>
        private void UndoLastMove()
        {
            // Phục hồi map từ lịch sử
            _map = _mapHistory.Pop();

            // Phục hồi vị trí người chơi từ lịch sử
            (_playerX, _playerY) = _playerHistory.Pop();

            // Phục hồi trạng thái người chơi từ lịch sử
            _playerStatus = _playerStateHistory.Pop();

            // Giảm số bước (nếu cần)
            if (_steps > 0) _steps--;

            // Xóa bước di chuyển cuối cùng khỏi _moveHistory
            if (_moveHistory.Count > 0)
            {
                _moveHistory.RemoveAt(_moveHistory.Count - 1);
            }

            this.Invalidate(); // Vẽ lại màn hình
        }

        /// <summary>
        /// Khởi động lại trò chơi.
        /// </summary>
        private void RestartGame()
        {
            // Khôi phục trạng thái ban đầu
            _map = (char[,])_initialMap.Clone();
            _playerX = _initialPlayerX;
            _playerY = _initialPlayerY;
            _playerStatus = _initialPlayerState;
            _boxStatus = _initialBoxState;

            _mapHistory.Clear();
            _playerHistory.Clear();
            _playerStateHistory.Clear();
            _moveHistory.Clear();
            _steps = 0;

            this.Invalidate();
        }

        /// <summary>
        /// Thực hiện di chuyển nhân vật theo logic game.
        /// </summary>
        /// <param name="newX">Vị trí X mới.</param>
        /// <param name="newY">Vị trí Y mới.</param>
        /// <param name="dx">Sự thay đổi về X.</param>
        /// <param name="dy">Sự thay đổi về Y.</param>
        /// <returns>Trả về true nếu di chuyển hợp lệ, ngược lại trả về false.</returns>
        private bool ProcessMove(int newX, int newY, int dx, int dy)
        {
            if (_map[newX, newY] == '#')
            {
                return false; // Không thực hiện di chuyển
            }

            // Kiểm tra di chuyển hợp lệ
            if (_map[newX, newY] == 'G')
            {
                if (_playerStatus == Status.OnGoal)
                {
                    _map[_playerX, _playerY] = 'G';
                }
                else
                {
                    _map[_playerX, _playerY] = ' ';
                }
                _playerX = newX;
                _playerY = newY;
                _map[_playerX, _playerY] = 'P';
                _playerStatus = Status.OnGoal;
            }
            if (_map[newX, newY] == ' ')
            {
                switch (_playerStatus)
                {
                    case Status.OutGoal:
                        _map[_playerX, _playerY] = ' ';
                        break;
                    case Status.OnGoal:
                        _map[_playerX, _playerY] = 'G';
                        _playerStatus = Status.OutGoal;
                        break;
                    default:
                        break;
                }
                _playerX = newX;
                _playerY = newY;
                _map[_playerX, _playerY] = 'P';
            }
            else if (_map[newX, newY] == 'B' || _map[newX, newY] == 'A')
            {
                // Kiểm tra nếu hộp có thể được đẩy
                int boxNewX = newX + dx, boxNewY = newY + dy;
                if (_map[boxNewX, boxNewY] != '#')
                {
                    if (_map[newX, newY] == 'A')
                    {
                        _boxStatus = Status.OnGoal;
                    }
                }
                if (_map[boxNewX, boxNewY] == 'G')
                {
                    if (_boxStatus == Status.OnGoal)
                    {
                        if (_playerStatus == Status.OnGoal)
                        {
                            _map[_playerX, _playerY] = 'G';
                        }
                        else
                        {
                            _map[_playerX, _playerY] = ' ';
                        }
                        _playerStatus = Status.OnGoal;
                    }
                    else
                    {
                        if (_playerStatus == Status.OnGoal)
                        {
                            _map[_playerX, _playerY] = 'G';
                        }
                        else
                        {
                            _map[_playerX, _playerY] = ' ';
                        }
                        _playerStatus = Status.OutGoal;
                    }
                    _map[newX, newY] = 'P';            // Vị trí mới của người chơi
                    _map[boxNewX, boxNewY] = 'A';      // Vị trí mới của hộp
                    _playerX = newX;
                    _playerY = newY;
                    _boxStatus = Status.OutGoal;
                    return true;
                }
                if (_map[boxNewX, boxNewY] == ' ')
                {
                    if (_boxStatus == Status.OnGoal)
                    {
                        if (_playerStatus == Status.OnGoal)
                        {
                            _map[_playerX, _playerY] = 'G';
                        }
                        else
                        {
                            _map[_playerX, _playerY] = ' ';
                        }
                        _playerStatus = Status.OnGoal;
                    }
                    else
                    {
                        if (_playerStatus == Status.OnGoal)
                        {
                            _map[_playerX, _playerY] = 'G';
                        }
                        else
                        {
                            _map[_playerX, _playerY] = ' ';
                        }
                        _playerStatus = Status.OutGoal;
                    }
                    _map[newX, newY] = 'P';            // Vị trí mới của người chơi
                    _map[boxNewX, boxNewY] = 'B';      // Vị trí mới của hộp
                    _playerX = newX;
                    _playerY = newY;
                    _boxStatus = Status.OutGoal;
                    return true;
                }
                if (_map[boxNewX, boxNewY] == '#' || _map[boxNewX, boxNewY] == 'B' || _map[boxNewX, boxNewY] == 'A')
                {
                    return false;
                }
            }

            return true; // Trả về true nếu di chuyển hợp lệ
        }

        /// <summary>
        /// Kiểm tra điều kiện chiến thắng.
        /// </summary>
        private void CheckWinCondition()
        {
            // Kiểm tra xem có còn mục tiêu nào chưa hoàn thành không
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    if (_map[x, y] == 'G' || _playerStatus == Status.OnGoal) // Nếu còn mục tiêu chưa có hộp hoặc người chơi đang đứng trên đích.
                    {
                        return; // Chưa hoàn thành
                    }
                }
            }

            // Khi đã hoàn thành màn chơi
            UpdateHighScore(_mapManager.GetCurrentLevel(), _steps); // Lưu số bước của màn chơi vào highscore nếu có
            this.Invalidate(); // Vẽ lại màn hình
            int timeTaken = (int)(DateTime.Now - _startTime).TotalSeconds; // Tính thời gian hoàn thành màn chơi

            // Mở form StatisticsForm2 để hiển thị thống kê
            StatisticsForm2 statsForm = new StatisticsForm2(_steps, timeTaken, _moveHistory);
            statsForm.ShowDialog();
            _result = statsForm.Result; // Lấy kết quả từ button trong StatisticsForm2

            if (_result)
            {
                // Chơi tiếp màn kế tiếp
                _boxStatus = Status.OutGoal;  // Đặt lại trạng thái hộp
                _steps = 0;  // Đặt lại số bước về 0
                NextLevel(++_currentLevel); // Tải màn kế tiếp
            }
            else
            {
                // Nếu không chơi tiếp, quay lại LevelSelectionForm
                this.Close(); // Đóng form game hiện tại
            }
            if (File.Exists(SaveFile))
            {
                File.Delete(SaveFile);
            }
        }

        /// <summary>
        /// Chuyển sang level kế tiếp.
        /// </summary>
        /// <param name="level">Số thứ tự level kế tiếp.</param>
        private void NextLevel(int level)
        {
            if (File.Exists($"save_game{level}.txt"))
            {
                _mapHistory.Clear();
                _playerHistory.Clear();
                _playerStateHistory.Clear();
                _moveHistory.Clear();
                _mapManager.MoveToNextMap();
                LoadSpecificLevel(level);
            }
            else
            {
                if (_mapManager.MoveToNextMap())
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

        /// <summary>
        /// Sự kiện vẽ giao diện trò chơi.
        /// </summary>
        private void OnPaintHandler(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    Rectangle rect = new Rectangle(y * _cellSize, x * _cellSize, _cellSize, _cellSize);
                    char cell = _map[x, y];

                    switch (cell)
                    {
                        case '#':
                            g.DrawImage(_wallImage, rect); // Vẽ tường
                            break;
                        case 'P':
                            g.DrawImage(_playerImage, rect); // Vẽ người chơi
                            break;
                        case 'B':
                            g.DrawImage(_boxImage, rect); // Vẽ hộp
                            break;
                        case 'G':
                            g.DrawImage(_goalImage, rect); // Vẽ mục tiêu
                            break;
                        case 'A':
                            g.DrawImage(_placedBoxImage, rect); // Vẽ hộp trên đích
                            break;
                        default:
                            g.FillRectangle(Brushes.White, rect); // Ô trống
                            break;
                    }

                    g.DrawRectangle(Pens.Gray, rect); // Viền ô
                }
            }

            // Hiển thị số bước đã đi
            g.DrawString($"Steps: {_steps}", new Font("Arial", 14), Brushes.Black, new PointF(10, 10));
            if (_highScores.ContainsKey(_currentLevel))
            {
                // Hiển thị HighScore
                g.DrawString($"High Score: {_highScores[_currentLevel].Steps} by {_highScores[_currentLevel].PlayerName}", new Font("Arial", 14), Brushes.Black, new PointF(10, 30));
            }
        }

        /// <summary>
        /// Tải điểm cao từ file.
        /// </summary>
        private void LoadHighScores()
        {
            if (File.Exists(_highScoreFile))
            {
                string[] lines = File.ReadAllLines(_highScoreFile);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split(':');
                    if (parts.Length == 3) // Đảm bảo có đủ thông tin (Level, PlayerName, Steps)
                    {
                        if (int.TryParse(parts[0].Trim(), out int level) &&
                            !string.IsNullOrEmpty(parts[1].Trim()) &&
                            int.TryParse(parts[2].Trim(), out int steps))
                        {
                            string playerName = parts[1].Trim();
                            _highScores[level] = (playerName, steps);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Lưu điểm cao vào file.
        /// </summary>
        private void SaveHighScores()
        {
            var lines = _highScores.Select(kvp => $"{kvp.Key}: {kvp.Value.PlayerName}: {kvp.Value.Steps}");
            File.WriteAllLines(_highScoreFile, lines);
        }

        /// <summary>
        /// Cập nhật điểm cao cho level hiện tại.
        /// </summary>
        /// <param name="level">Level hiện tại.</param>
        /// <param name="steps">Số bước đã đi.</param>
        private void UpdateHighScore(int level, int steps)
        {
            if (!_highScores.ContainsKey(level) || steps < _highScores[level].Steps)
            {
                _highScores[level] = (PlayerName, steps); // Cập nhật điểm cao mới
                SaveHighScores();
            }
        }

        /// <summary>
        /// Lưu trạng thái game vào file.
        /// </summary>
        private void SaveGame()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(SaveFile))
                {
                    writer.WriteLine(PlayerName);
                    // Lưu map
                    for (int x = 0; x < _map.GetLength(0); x++)
                    {
                        for (int y = 0; y < _map.GetLength(1); y++)
                        {
                            writer.Write(_map[x, y]);
                        }
                        writer.WriteLine();
                    }
                    writer.WriteLine("");
                    // Lưu số bước
                    writer.WriteLine(_steps);

                    // Lưu thời gian chơi
                    int timeTaken = (int)(DateTime.Now - _startTime).TotalSeconds;
                    writer.WriteLine(timeTaken);

                    // Lưu vị trí người chơi
                    writer.WriteLine(_playerX);
                    writer.WriteLine(_playerY);

                    // Lưu trạng thái người chơi
                    writer.WriteLine(_playerStatus);

                    // Lưu lịch sử di chuyển
                    foreach (var move in _moveHistory)
                    {
                        writer.WriteLine(move);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu game: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tải trạng thái game từ file.
        /// </summary>
        /// <param name="level">Level cần tải.</param>
        public void LoadGame(int level)
        {
            try
            {
                using (StreamReader reader = new StreamReader(SaveFile))
                {
                    PlayerName = reader.ReadLine();
                    // Tải map
                    List<List<char>> loadedMap = new List<List<char>>();
                    string line;
                    while ((line = reader.ReadLine()) != null && !string.IsNullOrEmpty(line))
                    {
                        List<char> row = line.ToList();
                        loadedMap.Add(row);
                    }
                    _map = new char[loadedMap.Count, loadedMap[0].Count];
                    for (int i = 0; i < loadedMap.Count; i++)
                    {
                        for (int j = 0; j < loadedMap[i].Count; j++)
                        {
                            _map[i, j] = loadedMap[i][j];
                        }
                    }

                    // Tải số bước
                    if (int.TryParse(reader.ReadLine(), out int steps))
                    {
                        _steps = steps;
                    }

                    // Tải thời gian chơi
                    if (int.TryParse(reader.ReadLine(), out int timeTaken))
                    {
                        _startTime = DateTime.Now.AddSeconds(-timeTaken);
                    }

                    // Tải vị trí người chơi
                    if (int.TryParse(reader.ReadLine(), out int playerX))
                    {
                        _playerX = playerX;
                    }
                    if (int.TryParse(reader.ReadLine(), out int playerY))
                    {
                        _playerY = playerY;
                    }

                    // Tải trạng thái người chơi
                    string statusLine = reader.ReadLine();
                    if (Enum.TryParse(statusLine, out Status status))
                    {
                        _playerStatus = status;
                    }

                    // Tải lịch sử di chuyển
                    _moveHistory.Clear();
                    while ((line = reader.ReadLine()) != null)
                    {
                        _moveHistory.Add(line);
                    }

                    // Cập nhật số bước
                    this.Text = $"Steps: {_steps}";
                    this.Invalidate(); // Vẽ lại game
                }
                UpdateFormSize();

                // Phát âm thanh bắt đầu màn chơi khi tải game
                PlayLevelStartSound();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải game: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện đóng form.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            SaveGame(); // Lưu trạng thái game khi thoát

            // Giải phóng tài nguyên âm thanh
            if (_waveOutDevice != null)
            {
                _waveOutDevice.Stop();
                _waveOutDevice.Dispose();
                _waveOutDevice = null;
            }
            if (_audioFileReader != null)
            {
                _audioFileReader.Dispose();
                _audioFileReader = null;
            }
        }
    }
}

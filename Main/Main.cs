using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Map;
using NAudio.Wave;
using StatisticsForm;

namespace MainSys
{
    /// <summary>
    /// Represents the main form of the Sokoban game.
    /// </summary>
    public partial class Main : Form
    {
        private GameMapManager _mapManager;
        private char[,] _map; // The game map
        private int _playerX, _playerY; // Player's current position
        private const int CellSize = 64; // Size of each cell
        private int _currentLevel;
        private Image _wallImage;
        private Image _boxImage;
        private Image _goalImage;
        private Image _playerImage;
        private Image _placedBoxImage;
        private int _steps; // Number of steps taken
        private Stack<char[,]> _mapHistory; // History of map states
        private Stack<(int playerX, int playerY)> _playerHistory; // History of player positions
        private Stack<TrangThai> _playerStateHistory; // History of player states
        private char[,] _initialMap; // Initial map state
        private int _initialPlayerX, _initialPlayerY; // Initial player position
        private TrangThai _initialPlayerState; // Initial player state
        private TrangThai _initialBoxState; // Initial box state
        private bool _result; // Result after completing a level
        public string SaveFile { get; set; } // File to save game state

        /// <summary>
        /// Represents the state of an entity (Player or Box).
        /// </summary>
        private enum TrangThai
        {
            OnGoal,
            OutGoal
        }

        private TrangThai _playerStatus = TrangThai.OutGoal; // Player's current status
        private TrangThai _boxStatus = TrangThai.OutGoal; // Box's current status
        public string PlayerName { get; set; } // Player's name
        private const string HighScoreFile = "high_scores.txt"; // File to store high scores
        private Dictionary<int, (string PlayerName, int Steps)> _highScores = new Dictionary<int, (string, int)>();
        private List<string> _moveHistory = new List<string>();
        private DateTime _startTime;

        // NAudio variables
        private IWavePlayer _waveOutDevice;
        private AudioFileReader _audioFileReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Prevent flickering

            // Initialize variables
            _steps = 0;
            _mapHistory = new Stack<char[,]>();
            _playerHistory = new Stack<(int, int)>();
            _playerStateHistory = new Stack<TrangThai>();

            // Load images from resources
            _playerImage = Properties.Resources.player;
            _goalImage = Properties.Resources.goal;
            _boxImage = Properties.Resources.box;
            _wallImage = Properties.Resources.wall;
            _placedBoxImage = Properties.Resources.placedBox;

            // Load maps and high scores
            LoadMaps();
            LoadHighScores();

            // Subscribe to events
            this.KeyDown += OnKeyDown;
            this.Paint += OnPaint;
        }

        /// <summary>
        /// Plays the sound at the start of a level.
        /// </summary>
        private void PlayLevelStartSound()
        {
            try
            {
                // Dispose existing audio resources if any
                _waveOutDevice?.Stop();
                _waveOutDevice?.Dispose();
                _waveOutDevice = null;

                _audioFileReader?.Dispose();
                _audioFileReader = null;

                // Load sound from embedded resources
                using (var soundStream = new MemoryStream())
                {
                    Properties.Resources.GameSound.CopyTo(soundStream);
                    soundStream.Position = 0;

                    // Initialize audio playback
                    var waveReader = new WaveFileReader(soundStream);
                    _waveOutDevice = new WaveOutEvent();
                    _waveOutDevice.Init(waveReader);
                    _waveOutDevice.Play();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot play level start sound: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads a specific level.
        /// </summary>
        /// <param name="level">The level number to load.</param>
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
                _mapManager.SetCurrentMap(level - 1); // Levels are 1-indexed
                LoadCurrentMap();
            }
        }

        /// <summary>
        /// Loads all maps from files.
        /// </summary>
        private void LoadMaps()
        {
            _mapManager = new GameMapManager();

            // Load maps from files
            _mapManager.LoadMapsFromFile("Resources\\Level1.txt");
            _mapManager.LoadMapsFromFile("Resources\\Level2.txt");
            // Uncomment to add more levels
            // _mapManager.LoadMapsFromFile("Resources\\Level3.txt");
            // _mapManager.LoadMapsFromFile("Resources\\Level4.txt");
            // _mapManager.LoadMapsFromFile("Resources\\Level5.txt");
        }

        /// <summary>
        /// Loads the current map based on the current level.
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

                // Save initial state
                _initialMap = (char[,])_map.Clone();
                _initialPlayerX = _playerX;
                _initialPlayerY = _playerY;
                _initialPlayerState = _playerStatus;
                _initialBoxState = _boxStatus;

                // Play level start sound
                PlayLevelStartSound();
            }
        }

        /// <summary>
        /// Updates the form size based on the map dimensions.
        /// </summary>
        private void UpdateFormSize()
        {
            if (_map != null)
            {
                this.Text = $"Level: {_currentLevel}";
                this.Icon = Properties.Resources.Icon;
                this.Width = _map.GetLength(1) * CellSize + 16;
                this.Height = _map.GetLength(0) * CellSize + 39;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
            }
        }

        /// <summary>
        /// Handles key down events for player movement and actions.
        /// </summary>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_steps == 0)
            {
                _startTime = DateTime.Now; // Start timer
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
                        UndoLastMove();
                    }
                    return;
                case Keys.R:
                    RestartGame();
                    return;
                default:
                    return; // Ignore other keys
            }

            // Save current state before moving
            SaveCurrentState();

            int newX = _playerX + dx;
            int newY = _playerY + dy;

            if (ProcessMove(newX, newY, dx, dy)) // If move is valid
            {
                _steps++; // Increment step count
                string move = $"Step {_steps}: {e.KeyCode}";
                _moveHistory.Add(move);
                // Optionally, play move sound here
            }
            else // If move is invalid, revert state
            {
                _mapHistory.Pop();
                _playerHistory.Pop();
                _playerStateHistory.Pop();
            }

            CheckWinCondition(); // Check if level is completed
            this.Invalidate(); // Redraw the form
        }

        /// <summary>
        /// Saves the current state of the game for undo functionality.
        /// </summary>
        private void SaveCurrentState()
        {
            // Save current map state
            char[,] currentMapState = (char[,])_map.Clone();
            _mapHistory.Push(currentMapState);

            // Save current player position
            _playerHistory.Push((_playerX, _playerY));

            // Save current player status
            _playerStateHistory.Push(_playerStatus);
        }

        /// <summary>
        /// Undoes the last move made by the player.
        /// </summary>
        private void UndoLastMove()
        {
            // Restore map from history
            _map = _mapHistory.Pop();

            // Restore player position from history
            (_playerX, _playerY) = _playerHistory.Pop();

            // Restore player status from history
            _playerStatus = _playerStateHistory.Pop();

            // Decrement step count if possible
            if (_steps > 0)
            {
                _steps--;
            }

            // Remove last move from history
            if (_moveHistory.Count > 0)
            {
                _moveHistory.RemoveAt(_moveHistory.Count - 1);
            }

            this.Invalidate(); // Redraw the form
        }

        /// <summary>
        /// Restarts the current game level.
        /// </summary>
        private void RestartGame()
        {
            // Restore initial state
            _map = (char[,])_initialMap.Clone();
            _playerX = _initialPlayerX;
            _playerY = _initialPlayerY;
            _playerStatus = _initialPlayerState;
            _boxStatus = _initialBoxState;

            // Clear histories
            _mapHistory.Clear();
            _playerHistory.Clear();
            _playerStateHistory.Clear();
            _moveHistory.Clear();
            _steps = 0;

            this.Invalidate(); // Redraw the form
        }

        /// <summary>
        /// Processes the player's movement based on the input.
        /// </summary>
        /// <param name="newX">The new X position.</param>
        /// <param name="newY">The new Y position.</param>
        /// <param name="dx">The change in X direction.</param>
        /// <param name="dy">The change in Y direction.</param>
        /// <returns>True if the move is valid; otherwise, false.</returns>
        private bool ProcessMove(int newX, int newY, int dx, int dy)
        {
            if (_map[newX, newY] == '#')
            {
                return false; // Wall encountered
            }

            if (_map[newX, newY] == 'G')
            {
                UpdatePlayerPosition(newX, newY, TrangThai.OnGoal);
            }
            else if (_map[newX, newY] == ' ')
            {
                UpdatePlayerPosition(newX, newY, TrangThai.OutGoal);
            }
            else if (_map[newX, newY] == 'B' || _map[newX, newY] == 'A')
            {
                return HandleBoxMovement(newX, newY, dx, dy);
            }

            return true;
        }

        /// <summary>
        /// Updates the player's position and status on the map.
        /// </summary>
        private void UpdatePlayerPosition(int newX, int newY, TrangThai newStatus)
        {
            // Update previous position
            _map[_playerX, _playerY] = _playerStatus == TrangThai.OnGoal ? 'G' : ' ';

            // Move player
            _playerX = newX;
            _playerY = newY;
            _map[_playerX, _playerY] = 'P';
            _playerStatus = newStatus;
        }

        /// <summary>
        /// Handles the movement of a box when the player pushes it.
        /// </summary>
        /// <returns>True if the box was moved successfully; otherwise, false.</returns>
        private bool HandleBoxMovement(int boxX, int boxY, int dx, int dy)
        {
            int boxNewX = boxX + dx;
            int boxNewY = boxY + dy;

            if (_map[boxNewX, boxNewY] == '#' || _map[boxNewX, boxNewY] == 'B' || _map[boxNewX, boxNewY] == 'A')
            {
                return false; // Cannot push the box
            }

            if (_map[boxNewX, boxNewY] == 'G')
            {
                _map[boxNewX, boxNewY] = 'A'; // Box on goal
                _boxStatus = TrangThai.OnGoal;
            }
            else if (_map[boxNewX, boxNewY] == ' ')
            {
                _map[boxNewX, boxNewY] = 'B'; // Box moved to empty space
                _boxStatus = TrangThai.OutGoal;
            }

            // Update box's previous position
            _map[boxX, boxY] = _playerStatus == TrangThai.OnGoal ? 'G' : ' ';

            // Move player
            _playerX += dx;
            _playerY += dy;
            _map[_playerX, _playerY] = 'P';
            _playerStatus = (_map[_playerX, _playerY] == 'G') ? TrangThai.OnGoal : TrangThai.OutGoal;

            return true;
        }

        /// <summary>
        /// Checks if the current level has been completed.
        /// </summary>
        private void CheckWinCondition()
        {
            // Check for any remaining goals without boxes
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    if (_map[x, y] == 'G' || _playerStatus == TrangThai.OnGoal)
                    {
                        return; // Level not yet completed
                    }
                }
            }

            // Level completed
            UpdateHighScore(_mapManager.GetCurrentLevel(), _steps);
            this.Invalidate(); // Redraw the form

            int timeTaken = (int)(DateTime.Now - _startTime).TotalSeconds;

            // Show statistics form
            StatisticsForm2 statsForm = new StatisticsForm2(_steps, timeTaken, _moveHistory);
            statsForm.ShowDialog();
            _result = statsForm.Result; // Get result from statistics form

            if (_result)
            {
                // Proceed to next level
                _boxStatus = TrangThai.OutGoal;
                _steps = 0;
                NextLevel(_currentLevel + 1);
            }
            else
            {
                // Close the game and return to level selection
                this.Close();
            }

            // Delete save file if it exists
            if (File.Exists(SaveFile))
            {
                File.Delete(SaveFile);
            }
        }

        /// <summary>
        /// Loads high scores from the high score file.
        /// </summary>
        private void LoadHighScores()
        {
            if (File.Exists(HighScoreFile))
            {
                string[] lines = File.ReadAllLines(HighScoreFile);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(':');
                    if (parts.Length == 3) // Ensure correct format
                    {
                        if (int.TryParse(parts[0].Trim(), out int level) &&
                            !string.IsNullOrWhiteSpace(parts[1].Trim()) &&
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
        /// Saves high scores to the high score file.
        /// </summary>
        private void SaveHighScores()
        {
            var lines = _highScores.Select(kvp => $"{kvp.Key}: {kvp.Value.PlayerName}: {kvp.Value.Steps}");
            File.WriteAllLines(HighScoreFile, lines);
        }

        /// <summary>
        /// Updates the high score for the current level if the new score is better.
        /// </summary>
        /// <param name="level">The current level.</param>
        /// <param name="steps">The number of steps taken.</param>
        private void UpdateHighScore(int level, int steps)
        {
            if (!_highScores.ContainsKey(level) || steps < _highScores[level].Steps)
            {
                _highScores[level] = (PlayerName, steps);
                SaveHighScores();
            }
        }

        /// <summary>
        /// Saves the current game state to a file.
        /// </summary>
        private void SaveGame()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(SaveFile))
                {
                    writer.WriteLine(PlayerName);

                    // Save map
                    for (int x = 0; x < _map.GetLength(0); x++)
                    {
                        for (int y = 0; y < _map.GetLength(1); y++)
                        {
                            writer.Write(_map[x, y]);
                        }
                        writer.WriteLine();
                    }
                    writer.WriteLine();

                    // Save steps
                    writer.WriteLine(_steps);

                    // Save time taken
                    int timeTaken = (int)(DateTime.Now - _startTime).TotalSeconds;
                    writer.WriteLine(timeTaken);

                    // Save player position
                    writer.WriteLine(_playerX);
                    writer.WriteLine(_playerY);

                    // Save player status
                    writer.WriteLine(_playerStatus);

                    // Save move history
                    foreach (var move in _moveHistory)
                    {
                        writer.WriteLine(move);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads the game state from a file.
        /// </summary>
        /// <param name="level">The level to load.</param>
        public void LoadGame(int level)
        {
            try
            {
                using (StreamReader reader = new StreamReader(SaveFile))
                {
                    PlayerName = reader.ReadLine();

                    // Load map
                    List<List<char>> loadedMap = new List<List<char>>();
                    string line;
                    while ((line = reader.ReadLine()) != null && !string.IsNullOrEmpty(line))
                    {
                        loadedMap.Add(line.ToList());
                    }

                    _map = new char[loadedMap.Count, loadedMap[0].Count];
                    for (int i = 0; i < loadedMap.Count; i++)
                    {
                        for (int j = 0; j < loadedMap[i].Count; j++)
                        {
                            _map[i, j] = loadedMap[i][j];
                        }
                    }

                    // Load steps
                    if (!int.TryParse(reader.ReadLine(), out _steps))
                    {
                        _steps = 0;
                    }

                    // Load time taken
                    if (int.TryParse(reader.ReadLine(), out int timeTaken))
                    {
                        _startTime = DateTime.Now.AddSeconds(-timeTaken);
                    }
                    else
                    {
                        _startTime = DateTime.Now;
                    }

                    // Load player position
                    if (!int.TryParse(reader.ReadLine(), out _playerX))
                    {
                        _playerX = 0;
                    }
                    if (!int.TryParse(reader.ReadLine(), out _playerY))
                    {
                        _playerY = 0;
                    }

                    // Load player status
                    string statusLine = reader.ReadLine();
                    if (Enum.TryParse(statusLine, out TrangThai status))
                    {
                        _playerStatus = status;
                    }
                    else
                    {
                        _playerStatus = TrangThai.OutGoal;
                    }

                    // Load move history
                    _moveHistory.Clear();
                    while ((line = reader.ReadLine()) != null)
                    {
                        _moveHistory.Add(line);
                    }

                    this.Text = $"Steps: {_steps}";
                    this.Invalidate(); // Redraw the form
                }

                UpdateFormSize();

                // Play level start sound upon loading
                PlayLevelStartSound();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the painting of the game map and UI elements.
        /// </summary>
        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    Rectangle rect = new Rectangle(y * CellSize, x * CellSize, CellSize, CellSize);
                    char cell = _map[x, y];

                    switch (cell)
                    {
                        case '#':
                            g.DrawImage(_wallImage, rect); // Draw wall
                            break;
                        case 'P':
                            g.DrawImage(_playerImage, rect); // Draw player
                            break;
                        case 'B':
                            g.DrawImage(_boxImage, rect); // Draw box
                            break;
                        case 'G':
                            g.DrawImage(_goalImage, rect); // Draw goal
                            break;
                        case 'A':
                            g.DrawImage(_placedBoxImage, rect); // Draw placed box
                            break;
                        default:
                            g.FillRectangle(Brushes.White, rect); // Empty cell
                            break;
                    }

                    g.DrawRectangle(Pens.Gray, rect); // Draw cell border
                }
            }

            // Display step count
            g.DrawString($"Steps: {_steps}", new Font("Arial", 14), Brushes.Black, new PointF(10, 10));

            // Display high score if available
            if (_highScores.ContainsKey(_currentLevel))
            {
                g.DrawString($"High Score: {_highScores[_currentLevel].Steps} by {_highScores[_currentLevel].PlayerName}",
                             new Font("Arial", 14), Brushes.Black, new PointF(10, 30));
            }
        }

        /// <summary>
        /// Updates to the next level after completing the current one.
        /// </summary>
        /// <param name="level">The next level number.</param>
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
                    LoadCurrentMap(); // Load next map
                }
                else
                {
                    this.Close();
                    MessageBox.Show("All levels completed!", "Congratulations");
                }
            }
        }

        /// <summary>
        /// Handles the form closing event to save the game state and dispose resources.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            SaveGame(); // Save game state upon closing

            // Dispose audio resources
            _waveOutDevice?.Stop();
            _waveOutDevice?.Dispose();
            _waveOutDevice = null;

            _audioFileReader?.Dispose();
            _audioFileReader = null;
        }
    }
}

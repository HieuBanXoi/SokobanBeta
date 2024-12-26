using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Map
{
    public class GameMap
    {
        public string Name { get; set; } // Tên của map
        public char[,] MapData { get; set; } // Dữ liệu của map
        public int PlayerStartX { get; set; } // Vị trí bắt đầu của người chơi (X)
        public int PlayerStartY { get; set; } // Vị trí bắt đầu của người chơi (Y)

        public GameMap(string name, char[,] mapData, int playerStartX, int playerStartY)
        {
            Name = name;
            MapData = mapData;
            PlayerStartX = playerStartX;
            PlayerStartY = playerStartY;
        }
    }

    public class GameMapManager
    {
        private List<GameMap> maps = new List<GameMap>();
        public int currentMapIndex = 0;

        public GameMapManager() { }

        public void AddMap(GameMap map)
        {
            maps.Add(map);
        }

        public GameMap GetCurrentMap()
        {
            if (currentMapIndex < maps.Count)
                return maps[currentMapIndex];
            return null;
        }
        public int GetCurrentLevel()
        {
            // Trả về màn chơi hiện tại (bắt đầu từ 1)
            return currentMapIndex + 1;
        }
        public bool MoveToNextMap()
        {
            if (currentMapIndex + 1 < maps.Count)
            {
                currentMapIndex++;
                return true;
            }
            return false;
        }

        public void SetCurrentMap(int index)
        {
            if (index >= 0 && index < maps.Count)
            {
                currentMapIndex = index;
            }
        }

        // Load a map from a TXT file
        public void LoadMapsFromFile(string filePath)
        {
            try
            {
                // Đọc tất cả dòng từ file
                string[] lines = System.IO.File.ReadAllLines(filePath);

                // Tìm tên map (dòng đầu tiên)
                string mapName = lines[0].Replace("# Map ", "").Trim();

                // Parse bản đồ
                List<char[]> mapDataList = new List<char[]>();
                int playerX = 0, playerY = 0;
                bool foundPlayerStart = false;

                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    // Nếu là tọa độ bắt đầu
                    if (line.Contains(","))
                    {
                        string[] startCoords = line.Split(',');
                        playerX = int.Parse(startCoords[0]);
                        playerY = int.Parse(startCoords[1]);
                        foundPlayerStart = true;
                        break;
                    }

                    mapDataList.Add(line.ToCharArray());
                }

                if (!foundPlayerStart)
                {
                    throw new Exception("Player start position not defined in map file.");
                }

                // Chuyển List<char[]> thành mảng 2 chiều
                int rows = mapDataList.Count;
                int cols = mapDataList[0].Length;
                char[,] mapData = new char[rows, cols];
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        mapData[r, c] = mapDataList[r][c];
                    }
                }

                // Thêm bản đồ vào GameMapManager
                AddMap(new GameMap(mapName, mapData, playerX, playerY));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading map from file: {ex.Message}");
            }
        }
    }
}

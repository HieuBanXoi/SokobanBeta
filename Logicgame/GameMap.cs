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

    //public class GameMapManager
    //{
    //    private List<GameMap> maps = new List<GameMap>();
    //    private int currentMapIndex = 0;

    //    public GameMapManager() { }

    //    public void AddMap(GameMap map)
    //    {
    //        maps.Add(map);
    //    }

    //    public GameMap GetCurrentMap()
    //    {
    //        if (currentMapIndex < maps.Count)
    //            return maps[currentMapIndex];
    //        return null;
    //    }

    //    public bool MoveToNextMap()
    //    {
    //        if (currentMapIndex + 1 < maps.Count)
    //        {
    //            currentMapIndex++;
    //            return true;
    //        }
    //        return false;
    //    }

        
    //    public void SetCurrentMap(int index)
    //    {
    //        if (index >= 0 && index < maps.Count)
    //        {
    //            currentMapIndex = index;
    //        }
    //    }

    //    public bool HasMoreMaps()
    //    {
    //        return currentMapIndex + 1 < maps.Count;
    //    }
    //}


        public class GameMapManager
        {
            private List<GameMap> maps = new List<GameMap>();
            private int currentMapIndex = 0;

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

            public bool HasMoreMaps()
            {
                return currentMapIndex + 1 < maps.Count;
            }

            // Load a map from a TXT file
            public void LoadMap(string filePath)
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string name = reader.ReadLine(); // Read map name

                    List<string> mapData = new List<string>();
                    string line;
                    while ((line = reader.ReadLine()) != null && !line.Contains(","))
                    {
                        mapData.Add(line); // Read map data
                    }

                    int rows = mapData.Count;
                    int cols = mapData[0].Length;
                    char[,] mapArray = new char[rows, cols];

                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            mapArray[i, j] = mapData[i][j]; // Populate map data
                        }
                    }

                    string[] startCoordinates = line.Split(','); // Read start coordinates
                    int playerStartX = int.Parse(startCoordinates[0]);
                    int playerStartY = int.Parse(startCoordinates[1]);

                    GameMap loadedMap = new GameMap(name, mapArray, playerStartX, playerStartY);
                    AddMap(loadedMap); // Add the map to the collection
                }
            }
        }
    }

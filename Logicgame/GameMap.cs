using System;
using System.Collections.Generic;
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

        public bool HasMoreMaps()
        {
            return currentMapIndex + 1 < maps.Count;
        }
    }
}

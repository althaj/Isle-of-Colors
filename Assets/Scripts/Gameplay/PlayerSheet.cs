using System.Collections.Generic;
using System.Linq;

namespace PSG.IsleOfColors.Gameplay
{
    public class PlayerSheet
    {
        public PlayerSheetSpace[][] Spaces { get; private set; }
        
        public void GenerateMap(Map map)
        {
            Spaces = new PlayerSheetSpace[map.rows.Count][];
            for (int y = 0; y < Spaces.Length; y++)
            {
                Spaces[y] = new PlayerSheetSpace[map.rows.Max(x => x.Length)];

                for (int x = 0; x < Spaces[y].Length; x++)
                {
                    if (map.rows[y].Length <= x || map.rows[y][x] == 'x')
                        Spaces[y][x] = null;
                    else
                        Spaces[y][x] = new PlayerSheetSpace();
                }
            }
        }

        internal void UpdateAvailableMoves(bool isColoring, int currentMoveIndex)
        {
            if (isColoring)
            {
                if(currentMoveIndex == 0)
                {
                    foreach (var spaceY in Spaces)
                    {
                        foreach (var space in spaceY)
                        {
                            if(space != null)
                                space.IsEnabled = space.Color == null;
                        }
                    }
                }
                else
                {
                    List<PlayerSheetSpace> spacesToEnable = new();
                    for (int y = 0; y < Spaces.Length; y++)
                    {
                        for (int x = 0; x < Spaces[y].Length; x++)
                        {
                            if (Spaces[y][x] == null)
                                continue;

                            Spaces[y][x].IsEnabled = false;

                            if (Spaces[y][x].IsNew)
                            {
                                spacesToEnable.AddRange(GetAllAvailableNeighbours(x, y));
                            }
                        }
                    }

                    foreach(var space in spacesToEnable.Distinct())
                    {
                        space.IsEnabled = true;
                    }
                }
            }
            else
            {
                foreach (var spaceY in Spaces)
                {
                    foreach (var space in spaceY)
                    {
                        if(space != null)
                            space.IsEnabled = false;
                    }
                }
            }
        }

        private List<PlayerSheetSpace> GetAllAvailableNeighbours(int x, int y)
        {
            bool isEven = y % 2 == 0;

            List<PlayerSheetSpace> result = new();
            AddHexIfAvailible(result, isEven ? x - 1 : x, y - 1);
            AddHexIfAvailible(result, isEven ? x : x + 1, y - 1);
            AddHexIfAvailible(result, x - 1, y);
            AddHexIfAvailible(result, x + 1, y);
            AddHexIfAvailible(result, isEven ? x - 1 : x, y + 1);
            AddHexIfAvailible(result, isEven ? x : x + 1, y + 1);

            return result;
        }

        private bool IsAvailableHex(int x, int y)
        {
            if (y < 0 || y >= Spaces.Length || x < 0 || x >= Spaces[y].Length || Spaces[y][x] == null)
                return false;
            return Spaces[y][x].Color == null;
        }

        private void AddHexIfAvailible(List<PlayerSheetSpace> list, int x, int y)
        {
            if (IsAvailableHex(x, y))
                list.Add(Spaces[y][x]);
        }
    }
}

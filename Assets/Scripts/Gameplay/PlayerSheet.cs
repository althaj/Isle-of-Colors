using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

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
                        Spaces[y][x] = new PlayerSheetSpace(x, y);
                }
            }
        }

        internal void UpdateAvailableMoves(bool isColoring, int currentMoveIndex, int maxMoves)
        {
            if (isColoring)
            {
                if (currentMoveIndex == 0 && maxMoves != 0)
                {
                    foreach (var spaceY in Spaces)
                    {
                        foreach (var space in spaceY)
                        {
                            if (space != null)
                                space.IsEnabled = space.Color == null;
                        }
                    }
                }
                else
                {
                    if (currentMoveIndex >= maxMoves)
                    {
                        ClearAllSpaces();
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

                        foreach (var space in spacesToEnable.Distinct())
                        {
                            space.IsEnabled = true;
                        }
                    }
                }
            }
            else
            {
                ClearAllSpaces();
            }
        }

        public List<List<PlayerSheetSpace>> GetAllGroups(PencilColor color)
        {
            List<List<PlayerSheetSpace>> result = new();

            foreach (var spaceY in Spaces)
            {
                foreach (var space in spaceY)
                {
                    if (space == null || result.Any(x => x.Any(y => y.X == space.X && y.Y == space.Y)) || space.Color != color)
                        continue;

                    result.Add(new());
                    var group = result.Last();
                    AddToGroup(group, space);
                }
            }

            return result;
        }

        public List<PlayerSheetSpace> GetEdgeSpaces()
        {
            List<PlayerSheetSpace> result = new();
            foreach (var spaceY in Spaces)
            {
                foreach (var space in spaceY)
                {
                    if (space != null && GetAllNeighbours(space.X, space.Y).Count < 6)
                        result.Add(space);
                }
            }
            return result;
        }

        public List<PlayerSheetSpace> GetAllSpacesOfColor(PencilColor color)
        {
            List<PlayerSheetSpace> result = new();
            foreach (var spaceY in Spaces)
            {
                foreach (var space in spaceY)
                {
                    if (space != null && space.Color == color)
                        result.Add(space);
                }
            }
            return result;
        }

        private void AddToGroup(List<PlayerSheetSpace> group, PlayerSheetSpace space)
        {
            if (!group.Contains(space))
            {
                group.Add(space);

                foreach (var adjescentSpace in GetAllNeighboursOfColor(space.X, space.Y, space.Color))
                {
                    AddToGroup(group, adjescentSpace);
                }
            }
        }

        private void ClearAllSpaces()
        {
            foreach (var spaceY in Spaces)
            {
                foreach (var space in spaceY)
                {
                    if (space != null)
                        space.IsEnabled = false;
                }
            }
        }

        public List<PlayerSheetSpace> GetAllNeighboursOfColor(int x, int y, PencilColor color)
        {
            bool isEven = y % 2 == 0;

            List<PlayerSheetSpace> result = new();
            AddHexIfColor(result, isEven ? x - 1 : x, y - 1, color);
            AddHexIfColor(result, isEven ? x : x + 1, y - 1, color);
            AddHexIfColor(result, x - 1, y, color);
            AddHexIfColor(result, x + 1, y, color);
            AddHexIfColor(result, isEven ? x - 1 : x, y + 1, color);
            AddHexIfColor(result, isEven ? x : x + 1, y + 1, color);

            return result;
        }

        public List<List<PlayerSheetSpace>> GetAllRivers(PencilColor color)
        {
            List<List<PlayerSheetSpace>> result = new();
            var groups = GetAllGroups(color);

            foreach (var group in groups)
            {
                bool isRiver = true;
                foreach (var space in group)
                {
                    var neighbours = GetAllNeighboursOfColor(space.X, space.Y, color);
                    if(neighbours.Count > 2 || neighbours.Count == 0)
                    {
                        isRiver = false;
                        break;
                    }
                }

                if(isRiver)
                    result.Add(group);
            }

            return result;
        }

        private void AddHexIfColor(List<PlayerSheetSpace> list, int x, int y, PencilColor color)
        {
            if (!DoesSpaceExist(x, y))
                return;

            if (Spaces[y][x].Color == color)
                list.Add(Spaces[y][x]);
        }

        private void AddHexIfExists(List<PlayerSheetSpace> list, int x, int y)
        {
            if (DoesSpaceExist(x, y))
                list.Add(Spaces[y][x]);
        }

        public List<PlayerSheetSpace> GetAllNeighbours(int x, int y)
        {
            bool isEven = y % 2 == 0;

            List<PlayerSheetSpace> result = new();
            AddHexIfExists(result, isEven ? x - 1 : x, y - 1);
            AddHexIfExists(result, isEven ? x : x + 1, y - 1);
            AddHexIfExists(result, x - 1, y);
            AddHexIfExists(result, x + 1, y);
            AddHexIfExists(result, isEven ? x - 1 : x, y + 1);
            AddHexIfExists(result, isEven ? x : x + 1, y + 1);

            return result;
        }


        private List<PlayerSheetSpace> GetAllAvailableNeighbours(int x, int y)
        {
            bool isEven = y % 2 == 0;

            List<PlayerSheetSpace> result = new();
            AddHexIfColor(result, isEven ? x - 1 : x, y - 1, null);
            AddHexIfColor(result, isEven ? x : x + 1, y - 1, null);
            AddHexIfColor(result, x - 1, y, null);
            AddHexIfColor(result, x + 1, y, null);
            AddHexIfColor(result, isEven ? x - 1 : x, y + 1, null);
            AddHexIfColor(result, isEven ? x : x + 1, y + 1, null);

            return result;
        }

        private bool DoesSpaceExist(int x, int y)
        {
            if (y < 0 || y >= Spaces.Length || x < 0 || x >= Spaces[y].Length || Spaces[y][x] == null)
                return false;

            return true;
        }
    }
}

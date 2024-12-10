using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay
{
    public class PlayerSheet
    {
        private enum LineDirection { Horizontal, Left, Right }

        public PlayerSheetSpace[][] Spaces { get; set; }

        private Map map;

        public void GenerateMap(Map map)
        {
            this.map = map;

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
                    if (neighbours.Count > 2 || neighbours.Count == 0)
                    {
                        isRiver = false;
                        break;
                    }
                }

                if (isRiver)
                    result.Add(group);
            }

            return result;
        }

        public List<PlayerSheetSpace> GetLongestLine(PlayerSheetSpace space)
        {
            List<PlayerSheetSpace> lineHorizontal = GetLine(space, LineDirection.Horizontal);
            List<PlayerSheetSpace> lineLeft = GetLine(space, LineDirection.Left);
            List<PlayerSheetSpace> lineRight = GetLine(space, LineDirection.Right);

            if (lineHorizontal.Count > lineLeft.Count && lineHorizontal.Count > lineRight.Count)
                return lineHorizontal;
            else if (lineLeft.Count > lineRight.Count)
                return lineLeft;
            else
                return lineRight;
        }

        public PlayerSheetSpace GetSpace(int x, int y) => DoesSpaceExist(x, y) ? Spaces[y][x] : null;

        public int GetDistanceBetweenGroups(List<PlayerSheetSpace> group1, List<PlayerSheetSpace> group2)
        {
            if (group1 == null || group1.Count == 0 || group2 == null || group2.Count == 0)
                return 0;

            int distance = int.MaxValue;

            foreach (var space1 in group1)
            {
                foreach (var space2 in group2)
                {
                    var dist = GetDistance(space1, space2);
                    if (dist < distance)
                        distance = dist;
                }
            }

            return distance;
        }

        // https://www.redblobgames.com/grids/hexagons/#distances-axial
        public int GetDistance(PlayerSheetSpace space1, PlayerSheetSpace space2)
        {
            return (Mathf.Abs(space1.Q - space2.Q) + Mathf.Abs(space1.Q + space1.Y - space2.Q - space2.Y) + Mathf.Abs(space1.Y - space2.Y)) / 2;
        }

        public void FillInRandomGroup(PencilColor color, int groupSize)
        {
            if (groupSize <= 0)
                return;

            var validGroups = GetAllGroups(null).Where(x => x.Count >= groupSize).ToList();

            if (validGroups.Count == 0)
                return;

            var selectedGroup = RNGManager.RNGManager.Manager["Game"].NextElement(validGroups);
            List<PlayerSheetSpace> selectedSpaces = new();

            var spaceToAdd = RNGManager.RNGManager.Manager["Game"].NextElement(selectedGroup);
            selectedSpaces.Add(spaceToAdd);
            selectedGroup.Remove(spaceToAdd);

            while (selectedSpaces.Count < groupSize)
            {
                var startingSpace = RNGManager.RNGManager.Manager["Game"].NextElement(selectedSpaces);
                var validNeighbours = GetAllNeighboursOfColor(startingSpace.X, startingSpace.Y, null).Where(x => !selectedSpaces.Contains(x));
                if (validNeighbours.Count() > 0)
                {
                    spaceToAdd = RNGManager.RNGManager.Manager["Game"].NextElement(validNeighbours);
                    selectedSpaces.Add(spaceToAdd);
                    selectedGroup.Remove(spaceToAdd);
                }
            }

            for (int i = 0; i < selectedSpaces.Count; i++)
            {
                Spaces[selectedSpaces[i].Y][selectedSpaces[i].X].SetColor(color, i + 1);
            }
        }

        public List<PlayerSheetSpace> GetNewSpaces()
        {
            List<PlayerSheetSpace> result = new();
            foreach (var spaceY in Spaces)
            {
                foreach (var space in spaceY)
                {
                    if (space != null && space.IsNew)
                        result.Add(space);
                }
            }
            return result;
        }

        private List<PlayerSheetSpace> GetLine(PlayerSheetSpace space, LineDirection direction)
        {
            List<PlayerSheetSpace> result = new();
            PlayerSheetSpace currentSpace = space;
            bool flippedDirection = false;

            while (currentSpace != null && currentSpace.Color == space.Color)
            {
                result.Add(currentSpace);
                switch (direction)
                {
                    case LineDirection.Horizontal:
                        if (!flippedDirection)
                        {
                            currentSpace = GetSpace(currentSpace.X + 1, currentSpace.Y);
                            if (currentSpace == null || currentSpace.Color != space.Color)
                            {
                                flippedDirection = true;
                                currentSpace = GetSpace(space.X - 1, space.Y);
                            }
                        }
                        else
                        {
                            currentSpace = GetSpace(currentSpace.X - 1, currentSpace.Y);
                        }
                        break;
                    case LineDirection.Left:
                        if (!flippedDirection)
                        {
                            currentSpace = GetSpace(currentSpace.X - (currentSpace.Y % 2 == 0 ? 1 : 0), currentSpace.Y + 1);
                            if (currentSpace == null || currentSpace.Color != space.Color)
                            {
                                flippedDirection = true;
                                currentSpace = GetSpace(space.X + (space.Y % 2 == 0 ? 0 : 1), space.Y - 1);
                            }
                        }
                        else
                        {
                            currentSpace = GetSpace(currentSpace.X + (currentSpace.Y % 2 == 0 ? 0 : 1), currentSpace.Y - 1);
                        }
                        break;
                    case LineDirection.Right:
                        if (!flippedDirection)
                        {
                            currentSpace = GetSpace(currentSpace.X + (currentSpace.Y % 2 == 0 ? 0 : 1), currentSpace.Y + 1);
                            if (currentSpace == null || currentSpace.Color != space.Color)
                            {
                                flippedDirection = true;
                                currentSpace = GetSpace(space.X - (space.Y % 2 == 0 ? 1 : 0), space.Y - 1);
                            }
                        }
                        else
                        {
                            currentSpace = GetSpace(currentSpace.X - (currentSpace.Y % 2 == 0 ? 1 : 0), currentSpace.Y - 1);
                        }
                        break;
                }
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

        public PlayerSheet GetCopy()
        {
            PlayerSheet sheet = new();
            sheet.GenerateMap(map);

            for (int y = 0; y < Spaces.Length; y++)
            {
                for (int x = 0; x < Spaces[y].Length; x++)
                {
                    if (Spaces[y][x] == null)
                    {
                        sheet.Spaces[y][x] = null;
                    }
                    else
                    {
                        sheet.Spaces[y][x] = new PlayerSheetSpace(x, y)
                        {
                            Color = Spaces[y][x].Color,
                            IsEnabled = Spaces[y][x].IsEnabled,
                            IsNew = Spaces[y][x].IsNew,
                            MoveIndex = Spaces[y][x].MoveIndex
                        };
                    }
                }
            }

            return sheet;
        }

        public void Reset()
        {
            foreach (var spaceY in Spaces)
            {
                foreach (var space in spaceY)
                {
                    if (space != null)
                    {
                        space.Undo();
                    }
                }
            }
        }
    }
}

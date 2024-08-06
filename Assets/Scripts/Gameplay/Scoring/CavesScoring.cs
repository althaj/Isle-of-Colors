using System.Collections.Generic;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class CavesScoring : IScoring
    {
        private PencilColor color;

        public CavesScoring(PencilColor color) => this.color = color;

        public PencilColor GetColor() => color;

        public string GetDescription() => "3 points per space in your longest straight line.";

        public string GetName() => "Caves";

        public int GetScore(PlayerSheet playerSheet)
        {
            var spaces = playerSheet.GetAllSpacesOfColor(color);
            List<PlayerSheetSpace> longestLine = new();

            foreach(var space in spaces)
            {
                var line = playerSheet.GetLongestLine(space);
                if(line.Count > longestLine.Count)
                    longestLine = line;
            }

            return longestLine.Count * 3;
        }
    }
}

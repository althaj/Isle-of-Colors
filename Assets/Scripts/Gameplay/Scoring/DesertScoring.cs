using System.Linq;
using Unity.Collections;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class DesertScoring : IScoring
    {
        private PencilColor color;
        private PencilColor neighbouringColor;

        public DesertScoring(PencilColor color, PencilColor neighbouringColor)
        {
            this.color = color;
            this.neighbouringColor = neighbouringColor;
        }

        public PencilColor GetColor() => color;

        public string GetDescription() => $"4 points per group next to no {neighbouringColor.Name}.";

        public string GetName() => "Desert";

        public int GetScore(PlayerSheet playerSheet)
        {
            var groups = playerSheet.GetAllGroups(color);
            int result = 0;

            foreach (var group in groups)
            {
                bool isDesert = true;
                foreach (var space in group)
                {
                    if (playerSheet.GetAllNeighboursOfColor(space.X, space.Y, neighbouringColor).Any())
                    {
                        isDesert = false;
                        break;
                    }
                }

                if(isDesert)
                    result += 4;
            }

            return result;
        }
    }
}

using System.Linq;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class HamletScoring : IScoring
    {
        private PencilColor color;

        public HamletScoring(PencilColor color) => this.color = color;

        public PencilColor GetColor() => color;

        public string GetDescription() => "3 points per space next to an empty space.";

        public string GetName() => "Hamlet";

        public int GetScore(PlayerSheet playerSheet)
        {
            int result = 0;

            foreach(var space in playerSheet.GetAllSpacesOfColor(color))
            {
                var neighbours = playerSheet.GetAllNeighbours(space.X, space.Y);
                if(neighbours.Any(x => x.Color == null))
                    result += 3;
            }

            return result;
        }
    }
}
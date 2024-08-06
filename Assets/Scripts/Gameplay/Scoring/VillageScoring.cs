using System.Linq;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class VillageScoring : IScoring
    {
        private PencilColor color;
        private PencilColor neighbouringColor1;
        private PencilColor neighbouringColor2;
        private PencilColor neighbouringColor3;

        public VillageScoring(PencilColor color, PencilColor neighbouringColor1, PencilColor neighbouringColor2, PencilColor neighbouringColor3)
        {
            this.color = color;
            this.neighbouringColor1 = neighbouringColor1;
            this.neighbouringColor2 = neighbouringColor2;
            this.neighbouringColor3 = neighbouringColor3;
        }

        public PencilColor GetColor() => color;

        public string GetDescription() => $"3 points per space next to {neighbouringColor1.Name}, {neighbouringColor2.Name} and {neighbouringColor3.Name}.";

        public string GetName() => "Village";

        public int GetScore(PlayerSheet playerSheet)
        {
            int count = 0;
            foreach (var space in playerSheet.GetAllSpacesOfColor(color))
                if (
                    playerSheet.GetAllNeighboursOfColor(space.X, space.Y, neighbouringColor1).Any()
                    && playerSheet.GetAllNeighboursOfColor(space.X, space.Y, neighbouringColor2).Any()
                    && playerSheet.GetAllNeighboursOfColor(space.X, space.Y, neighbouringColor3).Any()
                )
                    count++;

            return count * 3;
        }
    }
}

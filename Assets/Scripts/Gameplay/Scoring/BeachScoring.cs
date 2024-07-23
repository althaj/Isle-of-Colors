using System.Linq;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class BeachScoring : IScoring
    {
        private PencilColor color;
        private PencilColor neighbouringColor1;
        private PencilColor neighbouringColor2;

        public BeachScoring(PencilColor color, PencilColor neighbouringColor1, PencilColor neighbouringColor2)
        {
            this.color = color;
            this.neighbouringColor1 = neighbouringColor1;
            this.neighbouringColor2 = neighbouringColor2;
        }

        public PencilColor GetColor() => color;

        public string GetDescription() => $"2 points per space next to {neighbouringColor1.Name} and {neighbouringColor2.Name}.";

        public string GetName() => "Beach";

        public int GetScore(PlayerSheet playerSheet)
        {
            int result = 0;

            foreach(var space in playerSheet.GetAllSpacesOfColor(color))
            {
                var neighbours = playerSheet.GetAllNeighbours(space.X, space.Y);
                if(neighbours.Any(x => x.Color == neighbouringColor1) && neighbours.Any(x => x.Color == neighbouringColor2))
                    result += 2;
            }

            return result;
        }
    }
}
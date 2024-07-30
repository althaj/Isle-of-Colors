using System.Linq;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class ForestScoring : IScoring
    {
        private PencilColor color;
        private PencilColor neighbouringColor;

        public ForestScoring(PencilColor color, PencilColor neighbouringColor)
        {
            this.color = color;
            this.neighbouringColor = neighbouringColor;
        }

        public PencilColor GetColor() => color;

        public string GetDescription() => $"1 point per space that is next to {neighbouringColor.Name}.";

        public string GetName() => "Forest";

        public int GetScore(PlayerSheet playerSheet) => playerSheet.GetAllSpacesOfColor(color).Count(x => playerSheet.GetAllNeighboursOfColor(x.X, x.Y, neighbouringColor).Any());
    }

}

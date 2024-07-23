using System.Linq;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class ShoreScoring : IScoring
    {
        private PencilColor color;

        public ShoreScoring(PencilColor color) => this.color = color;

        public PencilColor GetColor() => color;

        public string GetDescription() => "1 point per space on the edge of the island.";

        public string GetName() => "Shore";

        public int GetScore(PlayerSheet playerSheet) => playerSheet.GetEdgeSpaces().Where(x => x.Color == color).Count() * 1;
    }
}

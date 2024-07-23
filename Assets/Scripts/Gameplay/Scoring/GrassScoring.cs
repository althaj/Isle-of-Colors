using System.Linq;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class GrassScoring : IScoring
    {
        private PencilColor color;

        public GrassScoring(PencilColor color) => this.color = color;

        public PencilColor GetColor() => color;

        public string GetDescription() => "6 points per group of at least 5 spaces.";

        public string GetName() => "Grass";

        public int GetScore(PlayerSheet playerSheet) => playerSheet.GetAllGroups(color).Where(x => x.Count >= 5).Count() * 6;
    }
}

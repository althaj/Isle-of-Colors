using System.Linq;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class FjordsScoring : IScoring
    {
        private PencilColor color;

        public FjordsScoring(PencilColor color) => this.color = color;

        public PencilColor GetColor() => color;

        public string GetDescription() => "4 points per group of 3 or 4 spaces.";

        public string GetName() => "Fjords";

        public int GetScore(PlayerSheet playerSheet) => playerSheet.GetAllGroups(color).Where(x => x.Count == 3 || x.Count == 4).Count() * 4;
    }
}

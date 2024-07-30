using System.Linq;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class SwampScoring : IScoring
    {
        private PencilColor color;

        public SwampScoring(PencilColor color) => this.color = color;

        public PencilColor GetColor() => color;

        public string GetDescription() => "2 points per space in your second largest group.";

        public string GetName() => "Swamp";

        public int GetScore(PlayerSheet playerSheet)
        {
            var swamps = playerSheet.GetAllGroups(color);
            if(swamps.Count > 1)
                return swamps.OrderByDescending(x => x.Count).ToArray()[1].Count * 2;

            return 0;
        }
    }
}


using System.Linq;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class TownScoring : IScoring
    {
        private PencilColor color;

        public TownScoring(PencilColor color) => this.color = color;

        public PencilColor GetColor() => color;

        public string GetDescription() => $"4 points per group at least 2 spaces away from all other {color.Name}.";

        public string GetName() => "Town";

        public int GetScore(PlayerSheet playerSheet)
        {
            var groups = playerSheet.GetAllGroups(color);

            if (groups.Count < 2)
                return 0;

            int count = 0;
            foreach (var group1 in groups)
            {
                int distance = int.MaxValue;
                foreach (var group2 in groups)
                {
                    if (group1 != group2)
                    {
                        int dist = playerSheet.GetDistanceBetweenGroups(group1, group2);
                        if (dist < distance)
                            distance = dist;
                    }
                }

                if (distance > 2)
                    count++;
            }
            return count * 4;
        }
    }
}

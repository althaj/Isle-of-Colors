using System.Linq;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class RiverScoring : IScoring
    {
        private PencilColor color;

        public RiverScoring(PencilColor color) => this.color = color;

        public PencilColor GetColor() => color;

        public string GetDescription() => "2 points per space in your longest river.";

        public string GetName() => "River";

        public int GetScore(PlayerSheet playerSheet)
        {
            var rivers = playerSheet.GetAllRivers(color);

            if (rivers.Count == 0)
                return 0;

            return rivers.OrderByDescending(x => x.Count).FirstOrDefault().Count * 2;
        }
    }
}

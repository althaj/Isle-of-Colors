using System.Collections.Generic;
using System.Linq;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public struct PlayerScore
    {
        public Dictionary<PencilColor, int> ColorScores { get; set; }
        public int TotalScore { get => ColorScores.Sum(k => k.Value); }

        public PlayerScore(List<PencilColor> colors)
        {
            ColorScores = new();
            foreach (var color in colors)
            {
                ColorScores.Add(color, 0);
            }
        }

        public void SetScore(PencilColor color, int score)
        {
            if(ColorScores.ContainsKey(color))
                ColorScores[color] = score;
        }
    }
}

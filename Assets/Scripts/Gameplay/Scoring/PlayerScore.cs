using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public class PlayerScore
    {
        public Dictionary<PencilColor, int> ColorScores { get; set; } = new();
        public int TotalScore { get => ColorScores == null ? 0 : ColorScores.Sum(k => k.Value); }

        public PlayerScore(GameManager gameManager)
        {
            foreach (var color in gameManager.ColorTypes)
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

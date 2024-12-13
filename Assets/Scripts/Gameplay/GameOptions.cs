using UnityEngine;

namespace PSG.IsleOfColors.Gameplay
{
    public struct GameOptions
    {
        public enum BotDifficulty
        {
            Easy = 0,
            Medium = 1,
            Hard = 2,
            MainMenu = 3
        }

        public string Player1Name { get; set; }
        public string Player2Name { get; set; }

        public bool IsSinglePlayer { get; set; }
        public BotDifficulty Difficulty { get; set; }

        public bool ShowTutorial { get; set; }
    }
}

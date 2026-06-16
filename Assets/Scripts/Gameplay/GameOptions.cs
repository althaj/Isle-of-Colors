using System.Collections.Generic;
using System.Linq;

namespace PSG.IsleOfColors.Gameplay
{
    public struct GameOptions
    {
        public enum PlayerType
        {
            Human,
            EasyBot,
            MediumBot,
            HardBot,
            MainMenu
        }

        public struct PlayerOptions
        {
            public string PlayerName { get; set; }
            public PlayerType PlayerType;
        }

        public List<PlayerOptions> Players;

        public bool ShowTutorial { get; set; }

        public bool AreOptionsValid() => 
            Players != null &&
            Players.Count >= 1 && Players.Count <= 4 &&
            Players.Any(p => p.PlayerType == PlayerType.Human || p.PlayerType == PlayerType.MainMenu) &&
            Players.All(p => !string.IsNullOrEmpty(p.PlayerName));

        public static string GetPlayerTypeString(PlayerType? playerType)
        {
            switch (playerType)
            {
                case PlayerType.EasyBot: return "Easy";
                case PlayerType.MediumBot: return "Medium";
                case PlayerType.HardBot: return "Hard";
                default: return null;
            }
        }
    }
}

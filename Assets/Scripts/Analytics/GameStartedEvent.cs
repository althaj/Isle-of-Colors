using Unity.Services.Analytics;
using PSG.IsleOfColors.Gameplay;

namespace PSG.IsleOfColors.Analytics
{
    public class GameStartedEvent : Event
    {
        public string BotDifficulty { set { SetParameter("BotDifficulty", value); } }
        public GameStartedEvent() : base("GameStarted")
        {
        }

        public GameStartedEvent(GameOptions.PlayerType? playerType) : base("GameStarted")
        {
            BotDifficulty = GameOptions.GetPlayerTypeString(playerType);
        }
    }
}

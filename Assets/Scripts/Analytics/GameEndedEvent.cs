using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Managers;
using Unity.Services.Analytics;

namespace PSG.IsleOfColors.Analytics
{
    public class GameEndedEvent : Event
    {
        public string BotDifficulty { set { SetParameter("BotDifficulty", value); } }
        public float GameDuration { set { SetParameter("GameDuration", value); } }

        public string GreenScoring { set { SetParameter("GreenScoring", value); } }
        public string BlueScoring { set { SetParameter("BlueScoring", value); } }
        public string BrownScoring { set { SetParameter("BrownScoring", value); } }
        public string RedScoring { set { SetParameter("RedScoring", value); } }

        public int GreenScore1 { set { SetParameter("GreenScore1", value); } }
        public int BlueScore1 { set { SetParameter("BlueScore1", value); } }
        public int BrownScore1 { set { SetParameter("BrownScore1", value); } }
        public int RedScore1 { set { SetParameter("RedScore1", value); } }
        public int TotalScore1 { set { SetParameter("TotalScore1", value); } }

        public int GreenScore2 { set { SetParameter("GreenScore2", value); } }
        public int BlueScore2 { set { SetParameter("BlueScore2", value); } }
        public int BrownScore2 { set { SetParameter("BrownScore2", value); } }
        public int RedScore2 { set { SetParameter("RedScore2", value); } }
        public int TotalScore2 { set { SetParameter("TotalScore2", value); } }

        public GameEndedEvent() : base("GameEnded")
        {
        }

        public GameEndedEvent(GameManager gameManager) : base("GameEnded")
        {
            GameOptions.BotDifficulty? difficulty = 
                ApplicationManager.Instance.GameOptions.IsSinglePlayer
                ? ApplicationManager.Instance.GameOptions.Difficulty
                : null;

            BotDifficulty = GameOptions.GetBotDifficultyString(difficulty);
            GameDuration = gameManager.GameDuration;

            GreenScoring = gameManager.GreenScoring.GetName();
            BlueScoring = gameManager.BlueScoring.GetName();
            BrownScoring = gameManager.BrownScoring.GetName();
            RedScoring = gameManager.RedScoring.GetName();

            GreenScore1 = gameManager.Player1.Score.ColorScores[gameManager.GetColorByName("Green")];
            BlueScore1 = gameManager.Player1.Score.ColorScores[gameManager.GetColorByName("Blue")];
            BrownScore1 = gameManager.Player1.Score.ColorScores[gameManager.GetColorByName("Brown")];
            RedScore1 = gameManager.Player1.Score.ColorScores[gameManager.GetColorByName("Red")];
            TotalScore1 = gameManager.Player1.Score.TotalScore;

            GreenScore2 = gameManager.Player2.Score.ColorScores[gameManager.GetColorByName("Green")];
            BlueScore2 = gameManager.Player2.Score.ColorScores[gameManager.GetColorByName("Blue")];
            BrownScore2 = gameManager.Player2.Score.ColorScores[gameManager.GetColorByName("Brown")];
            RedScore2 = gameManager.Player2.Score.ColorScores[gameManager.GetColorByName("Red")];
            TotalScore2 = gameManager.Player2.Score.TotalScore;
        }
    }
}

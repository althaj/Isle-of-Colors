using System.Linq;
using Newtonsoft.Json;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Gameplay.Scoring;
using PSG.IsleOfColors.Managers;
using Unity.Services.Analytics;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.Analytics
{
    public class GameEndedEvent : Unity.Services.Analytics.Event
    {
        public class PlayerScoreAnalyticsData
        {
            public string PlayerName { get; set; }
            public int[] Scores { get; set; }
        }

        public string GameOptions { set { SetParameter("GameOptions", value); } }
        public float GameDuration { set { SetParameter("GameDuration", value); } }

        public string GreenScoring { set { SetParameter("GreenScoring", value); } }
        public string BlueScoring { set { SetParameter("BlueScoring", value); } }
        public string BrownScoring { set { SetParameter("BrownScoring", value); } }
        public string RedScoring { set { SetParameter("RedScoring", value); } }

        public string PlayerScores { set { SetParameter("PlayerScores", value); } }

        public GameEndedEvent(GameManager gameManager, ApplicationManager applicationManager) : base("GameEnded")
        {
            if (gameManager == null || applicationManager == null)
            {
                Debug.LogError($"[GameEndedEvent::GetPlayerScoresAnalyticsData] Game Manager or Application Manager is invalid.");
                return;
            }

            GameOptions = JsonConvert.SerializeObject(applicationManager.GameOptions);
            GameDuration = gameManager.GameDuration;

            GreenScoring = gameManager.GreenScoring.GetName();
            BlueScoring = gameManager.BlueScoring.GetName();
            BrownScoring = gameManager.BrownScoring.GetName();
            RedScoring = gameManager.RedScoring.GetName();

            PlayerScores = JsonConvert.SerializeObject(GetPlayerScoresAnalyticsData(gameManager));
        }

        private PlayerScoreAnalyticsData[] GetPlayerScoresAnalyticsData(GameManager gameManager)
        {
            if (gameManager == null)
            {
                Debug.LogError($"[GameEndedEvent::GetPlayerScoresAnalyticsData] Game Manager is invalid.");
                return new PlayerScoreAnalyticsData[0];
            }

            return gameManager.Players.Select(p => new PlayerScoreAnalyticsData
            {
                PlayerName = p.Name,
                Scores = new int[]
                {
                    p.Score.ColorScores[gameManager.GetColorByName("Green")],
                    p.Score.ColorScores[gameManager.GetColorByName("Blue")],
                    p.Score.ColorScores[gameManager.GetColorByName("Brown")],
                    p.Score.ColorScores[gameManager.GetColorByName("Red")]
                }
            }).ToArray();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using PSG.IsleOfColors.Gameplay.Scoring;
using PSG.IsleOfColors.Managers;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay.AI
{
    public class SimpleAI : IBot
    {
        private int retryCount;
        private GameManager gameManager;

        public SimpleAI()
        {
            switch (ApplicationManager.Instance.GameOptions.Difficulty)
            {
                case GameOptions.BotDifficulty.MainMenu : retryCount = 1; break;
                case GameOptions.BotDifficulty.Easy: retryCount = 75; break;
                case GameOptions.BotDifficulty.Medium: retryCount = 150; break;
                case GameOptions.BotDifficulty.Hard: retryCount = 300; break;
            }

            gameManager = Object.FindFirstObjectByType<GameManager>();
        }

        public void DoTurn(Player player)
        {
            if (player == null)
            {
                Debug.LogError("SimpleAI.DoTurn: Player is null.");
                return;
            }
            
            if (player.DieValue == 0)
                {
                    player.StartColoring(player.Colors.First());
                    player.Confirm();
                    return;
                }

            List<PlayerSheet> sheets = new();

            List<PencilColor> colorsToCheck = player.Colors;

            // Swamp scoring fix (it socres for 2nd largest group, so we need to force the bot to create the first group).
            var greenColor = gameManager.GetColorByName("Green");
            if (
                gameManager.GreenScoring is SwampScoring
                && player.PlayerSheet.GetAllGroups(greenColor).Count == 0 
                && colorsToCheck.Contains(greenColor)
                && player.DieValue > 3
            )
                colorsToCheck = new() { greenColor };

            foreach (var color in colorsToCheck)
            {
                for (int i = 0; i < retryCount; i++)
                {
                    var newSheet = player.PlayerSheet.GetCopy();

                    newSheet.FillInRandomGroup(color, player.DieValue);

                    sheets.Add(newSheet);
                }
            }

            if (sheets.Count == 0)
                return;

            var sheet = sheets.OrderByDescending(x => GetScoreFromSheet(x).TotalScore).First();

            var newSpaces = sheet.GetNewSpaces();
            if (newSpaces.Count == 0)
                return;

            player.StartColoring(newSpaces.First().Color);
            foreach (var space in newSpaces)
                player.SetColor(space.X, space.Y);

            player.Confirm();
        }

        private PlayerScore GetScoreFromSheet(PlayerSheet sheet)
        {
            PlayerScore score = new(gameManager.Colors);

            score.SetScore(gameManager.GreenScoring.GetColor(), gameManager.GreenScoring.GetScore(sheet));
            score.SetScore(gameManager.BlueScoring.GetColor(), gameManager.BlueScoring.GetScore(sheet));
            score.SetScore(gameManager.BrownScoring.GetColor(), gameManager.BrownScoring.GetScore(sheet));
            score.SetScore(gameManager.RedScoring.GetColor(), gameManager.RedScoring.GetScore(sheet));

            return score;
        }
    }
}

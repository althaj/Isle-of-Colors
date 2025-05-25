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

        public bool DoTurn(Player player)
        {
            if (player == null || player.Colors == null || player.Colors.Count == 0)
            {
                Debug.LogError("SimpleAI.DoTurn: Player is null or player colors are null or empty.");
                return false;
            }
            
            if(player.PlayerState == StateMachine.States.EPlayerState.Finished)
            {
                return false;
            }

            if (player.DieValue == 0)
                {
                    player.StartColoring(player.Colors.First());
                    player.Confirm();
                    return true;
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
                    foreach(PlayerSheetSpace space in newSheet.GetNewSpaces())
                    {
                        space.Undo();
                    }

                    newSheet.FillInRandomGroup(color, player.DieValue);

                    sheets.Add(newSheet);
                }
            }

            if (sheets.Count == 0)
                return false;

            var sheet = sheets.OrderByDescending(x => GetScoreFromSheet(x).TotalScore).First();

            var newSpaces = sheet.GetNewSpaces();
            if (newSpaces.Count == 0)
                return false;

            PencilColor selectedColor = newSpaces.Where(x => x.Color != null).Select(x => x.Color).FirstOrDefault();
            if(selectedColor == null)
            {
                Debug.LogError("SimpleAI.DoTurn: No color found in new spaces. Cannot do a turn.");
                return false;
            }

            player.StartColoring(selectedColor);
            foreach (var space in newSpaces)
                player.SetColor(space.X, space.Y);

            if(!player.CanConfirm)
            {
                bool tepmp = player.CanConfirm;

                Debug.LogError("SimpleAI.DoTurn: Player cannot confirm the turn. Cannot do a turn.");

                foreach(PlayerSheetSpace space in player.PlayerSheet.GetNewSpaces())
                {
                    space.Undo();
                }

                player.StartColoring(null);

                return false;
            }

            player.Confirm();

            return true;
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

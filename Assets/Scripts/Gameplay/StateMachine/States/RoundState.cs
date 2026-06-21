using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace PSG.IsleOfColors.Gameplay.StateMachine.States
{
    public class RoundState : IState
    {
        public UnityEvent OnDescriptionChanged = new();

        private bool isDone = false;
        private string description;

        private GameManager gameManager;

        public RoundState(GameManager gameManager)
        {
            this.gameManager = gameManager;

            int dieValue = RNGManager.RNGManager.Manager["Game"].NextInt(1, 7);
            foreach (Player player in gameManager.Players)
            {
                player.OnPlayerStateChanged.AddListener(OnPlayerStateChanged);
                player.StartTurn(dieValue);
            }

            gameManager.RollDie(dieValue);

            OnPlayerStateChanged();
        }

        private void OnPlayerStateChanged()
        {
            if (gameManager == null)
            {
                Debug.LogError($"[RoundState::OnPlayerStateChanged] Game manager is invalid.");
                return;
            }

            if (gameManager.Players.All(p => p.PlayerState == EPlayerState.Finished))
            {
                isDone = true;
                return;
            }

            if (
                gameManager.Players
                    .Where(p => !p.IsAI)
                    .All(p => p.PlayerState == EPlayerState.Finished)
                )
            {
                foreach (Player aiPlayer in gameManager.Players.Where(p => p.IsAI))
                {
                    aiPlayer.DoAITurn();
                }
            }

            UpdateDescription();
            OnDescriptionChanged?.Invoke();
        }

        private void UpdateDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (gameManager.Players.Any(p => p.PlayerState == EPlayerState.PickingColor))
            {
                stringBuilder.Append("Players picking a color: ");
                stringBuilder.AppendJoin(", ",
                    gameManager
                        .Players
                        .Where(p => p.PlayerState == EPlayerState.PickingColor)
                        .Select(p => p.Name));
                stringBuilder.AppendLine();
            }

            if (gameManager.Players.Any(p => p.PlayerState == EPlayerState.Coloring))
            {
                stringBuilder.Append("Players coloring: ");
                stringBuilder.AppendJoin(", ",
                    gameManager
                        .Players
                        .Where(p => p.PlayerState == EPlayerState.Coloring)
                        .Select(p => p.Name));
                stringBuilder.AppendLine(".");
            }

            description = stringBuilder.ToString();
        }

        public void Execute()
        {

        }

        public void Exit()
        {
            OnDescriptionChanged?.RemoveAllListeners();
        }

        public string GetDescription()
        {
            return description;
        }

        public bool IsDone()
        {
            return isDone;
        }
    }
}

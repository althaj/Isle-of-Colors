using System.Linq;
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
            foreach(Player player in gameManager.Players)
            {
                player.OnPlayerStateChanged.AddListener(OnPlayerStateChanged);
                player.StartTurn(dieValue);
            }
            
            gameManager.RollDie(dieValue);

            OnPlayerStateChanged();
        }

        private void OnPlayerStateChanged()
        {
            if(gameManager == null)
            {
                Debug.LogError($"[RoundState::OnPlayerStateChanged] Game manager is invalid.");
                return;
            }

            if (gameManager.Players.All(p => p.PlayerState == EPlayerState.Finished))
            {
                isDone = true;
                return;
            }

            description = string.Empty;

            if (player1.PlayerState == EPlayerState.PickingColor)
                description = $"{player1.Name} is picking color. ";

            if (player1.PlayerState == EPlayerState.Coloring)
                description = $"{player1.Name} is coloring. ";

            if (player2.PlayerState == EPlayerState.PickingColor)
                description += $"{player2.Name} is picking color. ";

            if (player2.PlayerState == EPlayerState.Coloring)
                description += $"{player2.Name} is coloring. ";

            if (player1.PlayerState == EPlayerState.PickingColor && player2.PlayerState == EPlayerState.PickingColor)
                description = "Both players are picking colors.";

            if (player1.PlayerState == EPlayerState.Coloring && player2.PlayerState == EPlayerState.Coloring)
                description = "Both players are coloring.";

            OnDescriptionChanged?.Invoke();
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

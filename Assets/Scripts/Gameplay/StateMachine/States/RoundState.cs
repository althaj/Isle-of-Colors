using UnityEngine.Events;

namespace PSG.IsleOfColors.Gameplay.StateMachine.States
{
    public class RoundState : IState
    {
        public UnityEvent OnDescriptionChanged;

        private GameManager gameManager;
        private Player player1;
        private Player player2;

        private bool isDone = false;
        private string description;

        public RoundState(GameManager gameManager)
        {
            OnDescriptionChanged = new();

            this.gameManager = gameManager;
            player1 = gameManager.Player1;
            player2 = gameManager.Player2;

            player1.OnPlayerStateChanged.AddListener(OnPlayerStateChanged);
            player2.OnPlayerStateChanged.AddListener(OnPlayerStateChanged);

            int dieValue = RNGManager.RNGManager.Manager["Game"].NextInt(1, 7);
            player1.StartTurn(dieValue);
            player2.StartTurn(dieValue);
            
            gameManager.RollDie(dieValue);

            OnPlayerStateChanged();
        }

        private void OnPlayerStateChanged()
        {
            if (player1.PlayerState == EPlayerState.Finished && player2.PlayerState == EPlayerState.Finished)
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

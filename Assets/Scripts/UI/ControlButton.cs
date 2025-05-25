using UnityEngine;
using UnityEngine.UI;
using PSG.IsleOfColors.Gameplay;

namespace PSG.IsleOfColors.UI
{
    [RequireComponent(typeof(Button))]
    public class ControlButton : MonoBehaviour
    {
        [SerializeField] private bool isConfirm;

        private Button button;
        private GameManager gameManager;
        private Player currentPlayer;

        void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            if(gameManager == null)
            {
                Debug.LogError("ControlButton.Start: Missing game manager.");
                return;
            }

            if(gameManager.Player1 == null || gameManager.Player2 == null)
            {
                Debug.LogError("ControlButton.Start: Player is missing from GameManager.");
                return;   
            }

            button = GetComponent<Button>();
            if(button == null)
            {
                Debug.LogError("ControlButton.Start: Button is null.");
                return;
            }

            gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);
            gameManager.Player1.OnPlayerMove.AddListener(OnPlayerMove);
            gameManager.Player2.OnPlayerMove.AddListener(OnPlayerMove);

            OnCurrentPlayerChanged(gameManager.Player1, null);
        }

        void OnDestroy()
        {
            if(gameManager == null)
            {
                Debug.LogError("ControlButton.OnDestroy: Missing game manager.");
                return;
            }

            gameManager.OnCurrentPlayerChanged.RemoveListener(OnCurrentPlayerChanged);
            gameManager.Player1.OnPlayerMove.RemoveListener(OnPlayerMove);
            gameManager.Player2.OnPlayerMove.RemoveListener(OnPlayerMove);
        }

        void OnCurrentPlayerChanged(Player currentPlayer, Player previousPlayer)
        {
            this.currentPlayer = currentPlayer;
            UpdateButtonState(currentPlayer);
        }

        void OnPlayerMove(Player player)
        {
            if(currentPlayer == player)
            {
                UpdateButtonState(player);
            }
        }

        void UpdateButtonState(Player player)
        {
            if(player == null)
            {
                Debug.LogError("ControlButton.UpdateButtonState: Player is null.");
                return;
            }

            if(button == null)
            {
                Debug.LogError("ControlButton.UpdateButtonState: Button is null.");
                return;
            }
            
            button.interactable = isConfirm ? player.CanConfirm : player.CanUndo;
        }
    }
}

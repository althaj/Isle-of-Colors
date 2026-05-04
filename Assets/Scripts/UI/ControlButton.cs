using UnityEngine;
using UnityEngine.UI;
using PSG.IsleOfColors.Gameplay;
using Zenject;
using System;
using Unity.VisualScripting;

namespace PSG.IsleOfColors.UI
{
    [RequireComponent(typeof(Button))]
    public class ControlButton : MonoBehaviour
    {
        [SerializeField] private bool isConfirm;

        private Button button;
        private Player currentPlayer;

        [Inject] private GameManager _gameManager;

        private void OnGameInitialized()
        {
            _gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);
            _gameManager.Player1.OnPlayerMove.AddListener(OnPlayerMove);
            _gameManager.Player2.OnPlayerMove.AddListener(OnPlayerMove);

            OnCurrentPlayerChanged(_gameManager.Player1, _gameManager.Player2);

            _gameManager.OnGameInitialized.RemoveListener(OnGameInitialized);
        }

        void OnDisable()
        {
            _gameManager.OnCurrentPlayerChanged.RemoveListener(OnCurrentPlayerChanged);
            _gameManager.Player1.OnPlayerMove.RemoveListener(OnPlayerMove);
            _gameManager.Player2.OnPlayerMove.RemoveListener(OnPlayerMove);

            button.onClick.RemoveListener(OnButtonClicked);
        }

        void OnEnable()
        {
            _gameManager.InvokeAfterInitialization(OnGameInitialized);

            if (button == null)
            {
                button = GetComponent<Button>();
            }

            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            if (isConfirm)
            {
                _gameManager.Confirm();
            }
            else
            {
                _gameManager.Undo();
            }
        }

        void OnCurrentPlayerChanged(Player currentPlayer, Player previousPlayer)
        {
            this.currentPlayer = currentPlayer;
            UpdateButtonState(currentPlayer);
        }

        void OnPlayerMove(Player player)
        {
            if (currentPlayer == player)
            {
                UpdateButtonState(player);
            }
        }

        void UpdateButtonState(Player player)
        {
            if (player == null)
            {
                Debug.LogError("[ControlButton:UpdateButtonState] Player is null.");
                return;
            }

            if (button == null)
            {
                Debug.LogError("[ControlButton:UpdateButtonState] Button is null.");
                return;
            }

            button.interactable = isConfirm ? player.CanConfirm : player.CanUndo;
        }
    }
}

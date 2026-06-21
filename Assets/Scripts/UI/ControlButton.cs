using UnityEngine;
using UnityEngine.UI;
using PSG.IsleOfColors.Gameplay;
using Zenject;
using System;
using Unity.VisualScripting;
using System.Linq;

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
            foreach(Player player in _gameManager.Players)
            {
                player.OnPlayerMove.AddListener(OnPlayerMove);
            }
            OnCurrentPlayerChanged(_gameManager.Players.First());

            _gameManager.OnGameInitialized.RemoveListener(OnGameInitialized);
        }

        void OnDisable()
        {
            _gameManager.OnCurrentPlayerChanged.RemoveListener(OnCurrentPlayerChanged);
           foreach(Player player in _gameManager.Players)
            {
                player.OnPlayerMove.RemoveListener(OnPlayerMove);
            }

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

        void OnCurrentPlayerChanged(Player currentPlayer)
        {
            if(currentPlayer == null)
            {
                Debug.LogError($"[ControlButton::OnCurrentPlayerChanged] Current player is invalid.");
                return;
            }

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

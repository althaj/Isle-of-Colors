using System;
using System.Collections.Generic;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Managers;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    public class NotEnoughSpacePopup : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        [SerializeField] private GameObject background;

        private Player currentPlayer;
        private List<Player> popupDisplayedToPlayers = new List<Player>();

        [Inject] private GameManager _gameManager;

        [Inject] private ApplicationManager _applicationManager;

        void Start()
        {
            _gameManager.InvokeAfterInitialization(OnGameInitialized);

            ClosePopup();
        }

        private void OnGameInitialized()
        {
            currentPlayer = _gameManager.Player1;

            _gameManager.OnDieRolled.AddListener(OnDieRolled);
            _gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);

            _gameManager.OnGameInitialized.RemoveListener(OnGameInitialized);
        }

        public void ClosePopup()
        {
            popup.SetActive(false);
            background.SetActive(false);
        }

        private void OpenPopup()
        {
            popup.SetActive(true);
            background.SetActive(true);
        }

        private void OnDieRolled(int dieValue)
        {
            popupDisplayedToPlayers.Clear();
            DisplayPopupForCurrentPlayer();
        }

        private void OnCurrentPlayerChanged(Player activePlayer, Player otherPlayer)
        {
            currentPlayer = activePlayer;
            DisplayPopupForCurrentPlayer();
        }

        private void DisplayPopupForCurrentPlayer()
        {
            if (_applicationManager.GameOptions.IsSinglePlayer && currentPlayer == _gameManager.Player2)
            {
                return;
            }

            if (currentPlayer.DieValue == 0 && !popupDisplayedToPlayers.Contains(currentPlayer))
            {
                OpenPopup();
                popupDisplayedToPlayers.Add(currentPlayer);
            }
        }

    }
}

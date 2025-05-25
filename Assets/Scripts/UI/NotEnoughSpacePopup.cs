using System;
using System.Collections.Generic;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Managers;
using UnityEngine;

namespace PSG.IsleOfColors.UI
{
    public class NotEnoughSpacePopup : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        [SerializeField] private GameObject background;

        private Player currentPlayer;
        private List<Player> popupDisplayedToPlayers = new List<Player>();

        private GameManager gameManager;

        void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();

            currentPlayer = gameManager.Player1;

            gameManager.OnDieRolled.AddListener(OnDieRolled);
            gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);

            ClosePopup();
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
            if(ApplicationManager.Instance.GameOptions.IsSinglePlayer && currentPlayer == gameManager.Player2)
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

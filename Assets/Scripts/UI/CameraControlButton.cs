using System;
using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    public class CameraControlButton : MonoBehaviour
    {
        [SerializeField] private Player player;
        private Button button;
        private GameManager gameManager;
        void Start()
        {
            button = GetComponent<Button>();
            gameManager = FindFirstObjectByType<GameManager>();
            gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);
            OnCurrentPlayerChanged(gameManager.Player1, gameManager.Player2);
        }

        private void OnCurrentPlayerChanged(Player currentPlayer, Player otherPlayer)
        {
            if (otherPlayer == player)
                button.interactable = true;
            else
                button.interactable = false;
        }

        public void ChangeCurrentPlayer()
        {
            gameManager.ChangeCurrentPlayer();
        }
    }
}

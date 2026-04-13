using System;
using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    public class CameraControlButton : MonoBehaviour
    {
        [SerializeField] private Player player;
        private Button button;
        [Inject] private GameManager _gameManager;
        void Start()
        {
            button = GetComponent<Button>();
            _gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);
            OnCurrentPlayerChanged(_gameManager.Player1, _gameManager.Player2);
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
            _gameManager.ChangeCurrentPlayer();
        }
    }
}

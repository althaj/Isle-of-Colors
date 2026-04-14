using System;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Gameplay.Scoring;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private bool isCurrentPlayer;

        private Player currentPlayer;

        private ColorUsagePanel[] colorUsagePanels;

        [Inject] private GameManager _gameManager;

        private void Start()
        {
            colorUsagePanels = GetComponentsInChildren<ColorUsagePanel>();

            _gameManager.InvokeAfterInitialization(OnGameInitialized);
        }

        private void OnGameInitialized()
        {
            _gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);
            _gameManager.Player1.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
            _gameManager.Player2.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);

            OnCurrentPlayerChanged(_gameManager.Player1, _gameManager.Player2);

            _gameManager.OnGameInitialized.RemoveListener(OnGameInitialized);
        }

        private void OnCurrentPlayerChanged(Player currentPlayer, Player otherPlayer)
        {
            playerNameText.text = isCurrentPlayer ? currentPlayer.Name : otherPlayer.Name;
            this.currentPlayer = currentPlayer;

            if (isCurrentPlayer)
            {
                OnPlayerScoreChanged(currentPlayer);
            }
            else
            {
                OnPlayerScoreChanged(otherPlayer);
            }

            foreach (var panel in colorUsagePanels)
                panel.PlayerChanged(isCurrentPlayer ? currentPlayer : otherPlayer);
        }

        private void OnPlayerScoreChanged(Player player)
        {
            if (player == null)
            {
                Debug.LogError($"[PlayerPanel::OnPlayerScoreChanged] Player is invalid");
                return;
            }

            if (player.Score == null)
            {
                return;
            }

            if (
                player == currentPlayer && isCurrentPlayer ||
                player != currentPlayer && !isCurrentPlayer)
            {
                scoreText.text = player.Score.TotalScore.ToString();
            }
        }
    }
}

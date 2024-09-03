using System;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Gameplay.Scoring;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private bool isCurrentPlayer;

        private Player currentPlayer;

        private GameManager gameManager;
        private ColorUsagePanel[] colorUsagePanels;

        private void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();

            colorUsagePanels = GetComponentsInChildren<ColorUsagePanel>();

            gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);
            gameManager.Player1.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
            gameManager.Player2.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);

            OnCurrentPlayerChanged(gameManager.Player1, gameManager.Player2);
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
            if(player == currentPlayer && isCurrentPlayer || player != currentPlayer && !isCurrentPlayer)
                scoreText.text = player.Score.TotalScore.ToString();
        }
    }
}

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

        private GameManager gameManager;
        private ColorUsagePanel[] colorUsagePanels;

        private void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();

            colorUsagePanels = GetComponentsInChildren<ColorUsagePanel>();
            gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);

            OnCurrentPlayerChanged(gameManager.Player1, gameManager.Player2);
        }

        private void OnCurrentPlayerChanged(Player currentPlayer, Player otherPlayer)
        {
            playerNameText.text = isCurrentPlayer ? currentPlayer.Name : otherPlayer.Name;

            if (isCurrentPlayer)
            {
                otherPlayer.OnPlayerScoreChanged.RemoveListener(OnPlayerScoreChanged);
                currentPlayer.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
                OnPlayerScoreChanged(currentPlayer.Score);
            }
            else
            {
                currentPlayer.OnPlayerScoreChanged.RemoveListener(OnPlayerScoreChanged);
                otherPlayer.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
                OnPlayerScoreChanged(otherPlayer.Score);
            }

            foreach (var panel in colorUsagePanels)
                panel.PlayerChanged(isCurrentPlayer ? currentPlayer : otherPlayer);
        }

        private void OnPlayerScoreChanged(PlayerScore score)
        {
            scoreText.text = score.TotalScore.ToString();
        }
    }
}

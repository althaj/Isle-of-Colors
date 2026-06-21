using System;
using System.Linq;
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
            foreach (Player player in _gameManager.Players)
            {
                player.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
            }

            OnCurrentPlayerChanged(_gameManager.Players.First());

            _gameManager.OnGameInitialized.RemoveListener(OnGameInitialized);
        }

        private void OnCurrentPlayerChanged(Player currentPlayer)
        {
            this.currentPlayer = currentPlayer;
            playerNameText.text = currentPlayer.Name;

            OnPlayerScoreChanged(currentPlayer);

            foreach (var panel in colorUsagePanels)
            {
                panel.PlayerChanged(currentPlayer);
            }
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

            if (player == currentPlayer)
            {
                scoreText.text = player.Score.TotalScore.ToString();
            }
        }
    }
}

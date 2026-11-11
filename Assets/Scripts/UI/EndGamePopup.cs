using System.Collections.Generic;
using System.Linq;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Managers;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    public class EndGamePopup : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject background;

        [SerializeField] private RectTransform playerCardsContainer;
        [SerializeField] private PlayerCard playerCardPrefab;

        [Inject] private ApplicationManager _applicationManager;
        [Inject] private AnalyticsManager _analyticsManager;
        [Inject] private GameManager _gameManager;

        private void Start()
        {
            _gameManager.OnGameEnded.AddListener(OnGameEnded);
            ClosePopup();
        }

        private void OnGameEnded()
        {
            OpenPopup();
        }

        public void OpenPopup()
        {
            if (_gameManager == null)
            {
                Debug.LogError($"[EndGamePopup::OpenPopup] Game Manager is invalid.");
                return;
            }

            if (panel == null)
            {
                Debug.LogError($"[EndGamePopup::OpenPopup] Panel is invalid.");
                return;
            }

            if (background == null)
            {
                Debug.LogError($"[EndGamePopup::OpenPopup] Background is invalid.");
                return;
            }

            if (playerCardsContainer == null)
            {
                Debug.LogError($"[EndGamePopup::OpenPopup] Player cards container is invalid.");
                return;
            }

            Dictionary<PencilColor, int> maximumScores = GetMaximumColorScores();
            int winningTotalScore = _gameManager.Players
                .OrderByDescending(p => p.Score.TotalScore)
                .Select(p => p.Score.TotalScore)
                .First();

            foreach (Player player in _gameManager.Players)
            {
                PlayerCard playerCard = Instantiate(playerCardPrefab, playerCardsContainer);
                if (playerCard == null)
                {
                    Debug.LogError($"[EndGamePopup::OpenPopup] Player card failed to instantiate.");
                    continue;
                }

                playerCard.Initialize(player, maximumScores, winningTotalScore);
            }

            panel.SetActive(true);
            background.SetActive(true);

            _analyticsManager.GameEnded(_gameManager, _applicationManager);
        }

        private Dictionary<PencilColor, int> GetMaximumColorScores()
        {
            Dictionary<PencilColor, int> maximumScores = new();
            foreach (Player player in _gameManager.Players)
            {
                if (player == null || player.Score == null)
                {
                    Debug.LogError($"[EndGamePopup::OpenPo] Player or score is invalid.");
                    continue;
                }

                foreach (PencilColor color in player.Score.ColorScores.Keys)
                {
                    if (maximumScores.ContainsKey(color))
                    {
                        if (player.Score.ColorScores[color] > maximumScores[color])
                        {
                            maximumScores[color] = player.Score.ColorScores[color];
                        }
                    }
                    else
                    {
                        maximumScores.Add(color, player.Score.ColorScores[color]);
                    }
                }
            }

            return maximumScores;
        }

        private void ClosePopup()
        {
            if (panel == null)
            {
                Debug.LogError($"[EndGamePopup::ClosePopup] Panel is invalid.");
                return;
            }

            if (background == null)
            {
                Debug.LogError($"[EndGamePopup::ClosePopup] Background is invalid.");
                return;
            }

            if (playerCardsContainer == null)
            {
                Debug.LogError($"[EndGamePopup::ClosePopup] Player cards container is invalid.");
                return;
            }

            for (int i = 0; i < playerCardsContainer.childCount; i++)
            {
                Destroy(playerCardsContainer.GetChild(i).gameObject);
            }

            panel.SetActive(false);
            background.SetActive(false);
        }

        public void CloseToMainMenu()
        {
            _applicationManager.LoadMainMenu();
        }
    }
}

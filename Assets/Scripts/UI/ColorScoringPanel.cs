using System;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Gameplay.Scoring;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    public class ColorScoringPanel : MonoBehaviour
    {
        [SerializeField] private bool isSetupScoring;

        [SerializeField] private PencilColor color;

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI scoringTitleText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private GameObject descriptionPanel;

        private GameManager gameManager;

        private void Awake()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            gameManager.OnScoringSetupFinished.AddListener(OnScoringSetupFinished);
        }

        private void OnScoringSetupFinished()
        {
            IScoring scoring = gameManager.GetScoring(color);
            if (scoring == null)
                return;

            image.color = color.Color;
            scoringTitleText.text = scoring.GetName();
            descriptionText.text = scoring.GetDescription();

            if (!isSetupScoring)
            {
                gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);
                OnCurrentPlayerChanged(gameManager.Player1, gameManager.Player2);
            }
        }

        private void OnCurrentPlayerChanged(Player currentPlayer, Player otherPlayer)
        {
            otherPlayer.OnPlayerScoreChanged.RemoveListener(OnPlayerScoreChanged);
            currentPlayer.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
        }

        private void OnPlayerScoreChanged(PlayerScore score)
        {
            scoreText.text = score.ColorScores[color].ToString();
        }

        public void OnPointerEnter()
        {
            descriptionPanel.SetActive(true);
        }

        public void OnPointerExit()
        {
            descriptionPanel.SetActive(false);
        }
    }
}

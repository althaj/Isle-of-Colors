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

        private bool isShown;

        private GameManager gameManager;

        private Player currentPlayer;

        private void Start()
        {
            Display(false);

            gameManager = FindFirstObjectByType<GameManager>();
            gameManager.OnScoringSetupFinished.AddListener(OnScoringSetupFinished);
            OnScoringSetupFinished();
        }

        private void OnScoringSetupFinished()
        {
            IScoring scoring = gameManager.GetScoring(color);
            if (scoring == null)
                return;

            Display(true);

            image.color = color.Color;
            scoringTitleText.text = scoring.GetName();
            descriptionText.text = scoring.GetDescription();

            if (!isSetupScoring)
            {
                gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);
                gameManager.Player1.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
                gameManager.Player2.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
                
                OnCurrentPlayerChanged(gameManager.Player1, gameManager.Player2);
            }
        }

        private void Display(bool show)
        {
            image.enabled = show;
            scoringTitleText.enabled = show;
            descriptionText.enabled = show;

            if(!isSetupScoring)
            {
                scoreText.enabled = show;
            }

            isShown = show;
        }

        private void OnCurrentPlayerChanged(Player currentPlayer, Player otherPlayer)
        {
            this.currentPlayer = currentPlayer;
            OnPlayerScoreChanged(currentPlayer);
        }

        private void OnPlayerScoreChanged(Player currentPlayer)
        {
            if(this.currentPlayer == currentPlayer)
                scoreText.text = currentPlayer.Score.ColorScores[color].ToString();
        }

        public void OnPointerEnter()
        {
            if(isShown)
                descriptionPanel.SetActive(true);
        }

        public void OnPointerExit()
        {
            descriptionPanel.SetActive(false);
        }
    }
}

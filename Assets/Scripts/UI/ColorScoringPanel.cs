using System;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Gameplay.Scoring;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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

        private Player currentPlayer;

        [Inject] private GameManager _gameManager;

        private void Start()
        {
            Display(false);

            _gameManager = FindFirstObjectByType<GameManager>();
            _gameManager.OnScoringSetupFinished.AddListener(OnScoringSetupFinished);
            OnScoringSetupFinished();
        }

        private void OnScoringSetupFinished()
        {
            IScoring scoring = _gameManager.GetScoring(color);
            if (scoring == null)
                return;

            Display(true);

            image.color = color.Color;
            scoringTitleText.text = scoring.GetName();
            descriptionText.text = scoring.GetDescription();

            if (!isSetupScoring)
            {
                _gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);
                _gameManager.Player1.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
                _gameManager.Player2.OnPlayerScoreChanged.AddListener(OnPlayerScoreChanged);
                
                OnCurrentPlayerChanged(_gameManager.Player1, _gameManager.Player2);
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

using System;
using System.Collections.Generic;
using System.Linq;
using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    [RequireComponent(typeof(HighlightUI))]
    public class PlayerCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;

        [SerializeField] private Slider blueScoreSlider;
        [SerializeField] private TextMeshProUGUI blueScoreText;

        [SerializeField] private Slider greenScoreSlider;
        [SerializeField] private TextMeshProUGUI greenScoreText;

        [SerializeField] private Slider brownScoreSlider;
        [SerializeField] private TextMeshProUGUI brownScoreText;

        [SerializeField] private Slider redScoreSlider;
        [SerializeField] private TextMeshProUGUI redScoreText;

        [SerializeField] private TextMeshProUGUI totalScoreText;

        private HighlightUI highlight;

        public void Initialize(Player player, Dictionary<PencilColor, int> maximumScores, int winningTotalScore)
        {
            if (player == null)
            {
                Debug.LogError($"[PlayerCard::Initialize] Player is invalid.");
                return;
            }

            highlight = GetComponent<HighlightUI>();

            if (playerNameText == null ||
                blueScoreSlider == null || blueScoreText == null ||
                greenScoreSlider == null || greenScoreText == null ||
                brownScoreSlider == null || brownScoreText == null ||
                redScoreSlider == null || redScoreText == null ||
                totalScoreText == null ||
                highlight == null)
            {
                Debug.LogError($"[PlayerCard::Initialize] Some of the player card elements are invalid.");
                return;
            }
            // TODO: Fix this garbage of a code!
            PencilColor[] colors = maximumScores.Keys.ToArray();

            if (colors.Length != 4)
            {
                Debug.LogError($"[PlayerCard::Initialize] Maximum colors count is not 4.");
                return;
            }

            playerNameText.text = player.Name;

            blueScoreSlider.maxValue = maximumScores[colors[0]];
            blueScoreSlider.value = player.Score.ColorScores[colors[0]];
            blueScoreText.text = player.Score.ColorScores[colors[0]].ToString();

            greenScoreSlider.maxValue = maximumScores[colors[1]];
            greenScoreSlider.value = player.Score.ColorScores[colors[1]];
            greenScoreText.text = player.Score.ColorScores[colors[1]].ToString();

            brownScoreSlider.maxValue = maximumScores[colors[2]];
            brownScoreSlider.value = player.Score.ColorScores[colors[2]];
            brownScoreText.text = player.Score.ColorScores[colors[2]].ToString();

            redScoreSlider.maxValue = maximumScores[colors[3]];
            redScoreSlider.value = player.Score.ColorScores[colors[3]];
            redScoreText.text = player.Score.ColorScores[colors[3]].ToString();

            totalScoreText.text = player.Score.TotalScore.ToString();

            highlight.SetHighlight(player.Score.TotalScore == winningTotalScore);
        }
    }
}
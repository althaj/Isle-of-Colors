using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    public class EndGamePopup : MonoBehaviour
    {
        [SerializeField] private GameObject popup;

        [Header("Player names")]
        [SerializeField] private TextMeshProUGUI player1Name;
        [SerializeField] private TextMeshProUGUI player2Name;

        [Header("Green scoring")]
        [SerializeField] private Image greenImage;
        [SerializeField] private TextMeshProUGUI greenTitle;
        [SerializeField] private TextMeshProUGUI greenScore1;
        [SerializeField] private TextMeshProUGUI greenScore2;

        [Header("Blue scoring")]
        [SerializeField] private Image blueImage;
        [SerializeField] private TextMeshProUGUI blueTitle;
        [SerializeField] private TextMeshProUGUI blueScore1;
        [SerializeField] private TextMeshProUGUI blueScore2;

        [Header("Brown scoring")]
        [SerializeField] private Image brownImage;
        [SerializeField] private TextMeshProUGUI brownTitle;
        [SerializeField] private TextMeshProUGUI brownScore1;
        [SerializeField] private TextMeshProUGUI brownScore2;

        [Header("Red scoring")]
        [SerializeField] private Image redImage;
        [SerializeField] private TextMeshProUGUI redTitle;
        [SerializeField] private TextMeshProUGUI redScore1;
        [SerializeField] private TextMeshProUGUI redScore2;

        [Header("Total scoring")]
        [SerializeField] private TextMeshProUGUI totalScore1;
        [SerializeField] private TextMeshProUGUI totalScore2;
        [SerializeField] private Image totalScore1Background;
        [SerializeField] private Image totalScore2Background;

        private GameManager gameManager;

        private void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            gameManager.OnGameEnded.AddListener(OnGameEnded);
            ClosePopup();
        }

        private void OnGameEnded()
        {
            OpenPopup(gameManager.Player1, gameManager.Player2);
        }

        public void OpenPopup(Player player1, Player player2)
        {
            player1Name.text = player1.Name;
            player2Name.text = player2.Name;

            FillColorRow(player1, player2, "Green", greenImage, greenTitle, greenScore1, greenScore2);
            FillColorRow(player1, player2, "Blue", blueImage, blueTitle, blueScore1, blueScore2);
            FillColorRow(player1, player2, "Brown", brownImage, brownTitle, brownScore1, brownScore2);
            FillColorRow(player1, player2, "Red", redImage, redTitle, redScore1, redScore2);

            totalScore1.text = player1.Score.TotalScore.ToString();
            totalScore2.text = player2.Score.TotalScore.ToString();

            if(player1.Score.TotalScore >= player2.Score.TotalScore)
                totalScore1Background.color = Color.white;

            if(player2.Score.TotalScore >= player1.Score.TotalScore)
                totalScore2Background.color = Color.white;

            popup.SetActive(true);

            AnalyticsManager.Instance.GameEnded(gameManager);
        }

        private void FillColorRow(Player player1, Player player2, string colorName, Image image, TextMeshProUGUI title, TextMeshProUGUI player1Score, TextMeshProUGUI player2Score)
        {
            var color = gameManager.GetColorByName(colorName);
            image.color = color.Color;
            title.text = gameManager.GetScoring(color).GetName();
            player1Score.text = player1.Score.ColorScores[color].ToString();
            player2Score.text = player2.Score.ColorScores[color].ToString();
        }

        private void ClosePopup()
        {
            popup.SetActive(false);
        }

        public void CloseToMainMenu()
        {
            ApplicationManager.Instance.LoadMainMenu();
        }
    }
}

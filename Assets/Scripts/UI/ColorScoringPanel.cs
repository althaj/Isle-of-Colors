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
        [SerializeField] private PencilColor color;

        [SerializeField] private Image colorImage;
        [SerializeField] private TextMeshProUGUI scoringTitleText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [Inject] private GameManager _gameManager;

        private void Start()
        {
            if (_gameManager == null)
            {
                Debug.LogError($"[ColorScoringPanel::Start] Game manager is invalid.");
                return;
            }
            _gameManager.InvokeAfterInitialization(OnGameInitialized);
        }

        private void OnGameInitialized()
        {
            if (_gameManager == null)
            {
                Debug.LogError($"[ColorScoringPanel::OnGameInitialized] Game manager is invalid.");
                return;
            }

            if (colorImage == null)
            {
                Debug.LogError($"[ColorScoringPanel::OnGameInitialized] Color image is invalid.");
                return;
            }

            if (scoringTitleText == null)
            {
                Debug.LogError($"[ColorScoringPanel::OnGameInitialized] Scoring title text is invalid.");
                return;
            }

            if (descriptionText == null)
            {
                Debug.LogError($"[ColorScoringPanel::OnGameInitialized] Description text is invalid.");
                return;
            }

            IScoring scoring = _gameManager.GetScoring(color);
            if (scoring == null)
                return;

            colorImage.color = color.Color;
            scoringTitleText.text = scoring.GetName();
            descriptionText.text = scoring.GetDescription();

            _gameManager.OnGameInitialized.RemoveListener(OnGameInitialized);
        }
    }
}

using System;
using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    [RequireComponent(typeof(Animator))]
    public class ColorScore : MonoBehaviour
    {
        private const string USAGE_KEY = "Usage";

        [SerializeField] private PencilColor color;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Image backgroundImage;

        private Animator animator;

        private Player player;

        void Start()
        {
            if (color == null)
            {
                Debug.LogError($"[ColorScore::Start] Color is invalid.");
                return;
            }

            if(backgroundImage == null)
            {
                Debug.LogError($"[ColorScore::Start] Background image is invalid.");
                return;
            }

            if (scoreText == null)
            {
                Debug.LogError($"[ColorScore::Start] Score text is invalid.");
                return;
            }

            if (animator == null)
            {
                Debug.LogError($"[ColorScore::Start] Animator is invalid.");
                return;
            }

            backgroundImage.color = color.Color;
            scoreText.text = "0";
            animator.SetInteger(USAGE_KEY, 0);
        }

        public void Initialize(Player player)
        {
            if (player == null)
            {
                Debug.LogError($"[ColorScore::Initialize] Player is invalid.");
                return;
            }

            this.player = player;

            animator = GetComponent<Animator>();

            player.OnColorUsageChanged?.AddListener(OnColorUsageChanged);
            player.OnPlayerScoreChanged?.AddListener(OnPlayerScoreChanged);

        }

        private void OnPlayerScoreChanged(Player player)
        {
            if (color == null)
            {
                Debug.LogError($"[ColorScore::OnPlayerScoreChanged] Color is invalid.");
                return;
            }

            if (player == null)
            {
                Debug.LogError($"[ColorScore::OnPlayerScoreChanged] Player is invalid.");
                return;
            }

            if (scoreText == null)
            {
                Debug.LogError($"[ColorScore::OnPlayerScoreChanged] Score text is invalid.");
                return;
            }

            if (!player.Score.ColorScores.ContainsKey(color))
            {
                Debug.LogError($"[ColorScore::OnPlayerScoreChanged] Score for color {color.Name} is invalid.");
                return;
            }

            scoreText.text = player.Score.ColorScores[color].ToString();
        }

        private void OnColorUsageChanged()
        {
            if (color == null)
            {
                Debug.LogError($"[ColorScore::OnColorUsageChanged] Color is invalid.");
                return;
            }

            if (player == null)
            {
                Debug.LogError($"[ColorScore::OnColorUsageChanged] Player is invalid.");
                return;
            }

            if (animator == null)
            {
                Debug.LogError($"[ColorScore::OnColorUsageChanged] Animator is invalid.");
                return;
            }

            animator.SetInteger(USAGE_KEY, player.ColorUsage[color]);
        }

        void OnEnable()
        {
            if (player != null)
            {
                player.OnColorUsageChanged?.AddListener(OnColorUsageChanged);
                player.OnPlayerScoreChanged?.AddListener(OnPlayerScoreChanged);
            }
        }

        void OnDisable()
        {
            if (player != null)
            {
                player.OnColorUsageChanged?.RemoveListener(OnColorUsageChanged);
                player.OnPlayerScoreChanged?.RemoveListener(OnPlayerScoreChanged);
            }
        }
    }
}

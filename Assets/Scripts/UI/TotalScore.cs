using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine;
namespace PSG.IsleOfColors.UI

{
    public class TotalScore : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private Player player;

        void Start()
        {
            if (scoreText == null)
            {
                Debug.LogError($"[ColorScore::Start] Score text is invalid.");
                return;
            }

            scoreText.text = "0";
        }

        public void Initialize(Player player)
        {
            if (player == null)
            {
                Debug.LogError($"[ColorScore::Initialize] Player is invalid.");
                return;
            }

            player.OnPlayerScoreChanged?.AddListener(OnPlayerScoreChanged);
            this.player = player;
        }

        private void OnPlayerScoreChanged(Player player)
        {

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

            scoreText.text = player.Score.TotalScore.ToString();
        }

        void OnEnable()
        {
            if (player != null)
            {
                player.OnPlayerScoreChanged?.AddListener(OnPlayerScoreChanged);
            }
        }

        void OnDisable()
        {
            if (player != null)
            {
                player.OnPlayerScoreChanged?.RemoveListener(OnPlayerScoreChanged);
            }
        }
    }
}

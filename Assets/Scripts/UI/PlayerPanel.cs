using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine;

namespace PSG.IsleOfColors.UI
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNumberText;
        [SerializeField] private TextMeshProUGUI playerNameText;

        public void Initialize(int playerNumber, Player player)
        {
            if (player == null)
            {
                Debug.LogError($"[PlayerPanel::Initialize] Player is invalid.");
                return;
            }
            
            if (playerNameText == null)
            {
                Debug.LogError($"[PlayerPanel::Initialize] Player name text is invalid.");
                return;
            }

            playerNameText.text = player.Name;

            if (playerNumberText == null)
            {
                Debug.LogError($"[PlayerPanel::Initialize] Player number text is invalid.");
                return;
            }

            playerNumberText.text = playerNumber.ToString();

            ColorScore[] colorScores = GetComponentsInChildren<ColorScore>();
            foreach (ColorScore colorScore in colorScores)
            {
                colorScore.Initialize(player);
            }

            TotalScore totalScore = GetComponentInChildren<TotalScore>();
            if(totalScore == null)
            {
                Debug.LogError($"[PlayerPanel::Initialize] Total score is not valid.");
                return;
            }
            totalScore.Initialize(player);
        }

    }
}

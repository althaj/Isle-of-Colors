using PSG.IsleOfColors.Gameplay;
using System.Linq;
using TMPro;
using UnityEngine;

namespace PSG.IsleOfColors.Prototype
{
    public class PlayerDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI playerColors;

        [SerializeField] private Player player;

        private void Start()
        {
            playerName.text = player.Name;

            UpdateColorText();
        }

        private void OnEnable()
        {
            player.OnPlayerColorsChanged.AddListener(UpdateColorText);
        }

        private void OnDisable()
        {
            player.OnPlayerColorsChanged.RemoveListener(UpdateColorText);
        }

        private void UpdateColorText()
        {
            if (player == null || player.Colors == null || player.Colors.Count == 0)
                playerColors.text = string.Empty;
            else
                playerColors.text = string.Join(" ", player.Colors.Select(x => x.Name));
        }

    }
}

using System.Collections.Generic;
using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    public class ColorUsagePanel : MonoBehaviour
    {
        private Player player;
        [SerializeField] private PencilColor color;

        public void PlayerChanged(Player player)
        {
            if (player != null)
                player.OnColorUsageChanged.RemoveListener(OnColorUsageChanged);

            this.player = player;
            player.OnColorUsageChanged.AddListener(OnColorUsageChanged);
            OnColorUsageChanged();
        }

        private void OnColorUsageChanged()
        {
            if (player == null)
            {
                Debug.LogError("[ColorUsagePanel:OnColorUsageChanged] Player is invalid.");
                return;
            }

            if (!player.ColorUsage.ContainsKey(color))
            {
                Debug.LogWarning("[ColorUsagePanel:OnColorUsageChanged] Player's color usage does not contain the color.");
                return;
            }

            int colorUsage = player.ColorUsage[color];

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Image>().color = i < colorUsage ? color.Color : Color.white;
            }
        }
    }
}
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
            if(player != null)
                player.OnColorUsageChanged.RemoveListener(OnColorUsageChanged);

            this.player = player;
            player.OnColorUsageChanged.AddListener(OnColorUsageChanged);
            OnColorUsageChanged();
        }

        private void OnColorUsageChanged()
        {
            int colorUsage = player.ColorUsage[color];

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Image>().color = i < colorUsage ? color.Color : Color.white;
            }
        }
    }
}
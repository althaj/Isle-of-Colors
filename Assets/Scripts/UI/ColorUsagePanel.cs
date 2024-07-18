using System.Collections.Generic;
using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    public class ColorUsagePanel : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private GameObject colorUsagePanelPrefab;

        private Dictionary<PencilColor, TextMeshProUGUI> colorUsageTexts;

        private void Start()
        {
            colorUsageTexts = new();
            foreach (PencilColor color in player.ColorUsage.Keys)
            {
                GameObject colorUsagePanel = Instantiate(colorUsagePanelPrefab, transform);
                colorUsagePanel.name = $"{color.Name} Usage Panel";
                colorUsagePanel.GetComponent<Image>().color = color.Color;
                colorUsageTexts.Add(color, colorUsagePanel.GetComponentInChildren<TextMeshProUGUI>());
            }

            player.OnColorUsageChanged.AddListener(OnColorUsageChanged);
        }

        private void OnColorUsageChanged()
        {
            foreach (PencilColor color in player.ColorUsage.Keys)
            {
                colorUsageTexts[color].text = player.ColorUsage[color].ToString();
            }
        }
    }
}
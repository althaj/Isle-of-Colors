using UnityEngine;
using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine.UI;
using System;

namespace PSG.IsleOfColors.UI
{
    public class ColorButton : MonoBehaviour
    {
        public void Initialize(PencilColor color, Player player)
        {
            Image imageComponent = GetComponentInChildren<Image>();
            if(imageComponent != null)
            {
                imageComponent.color = color.Color;
            }

            Button buttonComponent = GetComponent<Button>();
            if(buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() => player.StartColoring(color));
            }

            HighlightableButton highlightableButtonComponent = GetComponent<HighlightableButton>();
            if(highlightableButtonComponent != null)
            {
                highlightableButtonComponent.SetHighlight(player.GetColor() == color);
            }
            
            TextMeshProUGUI textMeshProComponent = GetComponentInChildren<TextMeshProUGUI>();
            if(textMeshProComponent != null)
            {
                textMeshProComponent.text = String.Empty;
            }
        }
    }
}

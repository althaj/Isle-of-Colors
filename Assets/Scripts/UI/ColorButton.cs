using UnityEngine;
using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine.UI;

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

            LayoutElement layoutElement = GetComponent<LayoutElement>();
            if(layoutElement != null && player.GetColor() == color)
            {
                layoutElement.flexibleWidth = 2.0f;
            }
            
            TextMeshProUGUI textMeshProComponent = GetComponentInChildren<TextMeshProUGUI>();
            if(textMeshProComponent != null)
            {
                textMeshProComponent.gameObject.AddComponent<DieValueText>();
            }
        }
    }
}

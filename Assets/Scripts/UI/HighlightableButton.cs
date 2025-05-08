using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    public class HighlightableButton : MonoBehaviour
    {
        [SerializeField] private Image highlightImage;

        public void SetHighlight(bool highlight)
        {
            if (highlightImage != null)
            {
                highlightImage.enabled = highlight;
            }
        }
    }
}

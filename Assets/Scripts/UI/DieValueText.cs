using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine;

namespace PSG.IsleOfColors.UI
{
    public class DieValueText : MonoBehaviour
    {
        private TextMeshProUGUI text;

        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            FindFirstObjectByType<GameManager>().OnDieRolled.AddListener(OnDieRolled);
        }

        private void OnDieRolled(int dieValue)
        {
            text.text = dieValue.ToString();
        }
    }
}

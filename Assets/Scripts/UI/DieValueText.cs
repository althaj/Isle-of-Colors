using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine;

namespace PSG.IsleOfColors.UI
{
    public class DieValueText : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private GameManager gameManager;

        void OnEnable()
        {
            text = GetComponent<TextMeshProUGUI>();
            gameManager = FindFirstObjectByType<GameManager>();
            gameManager.OnDieRolled.AddListener(OnDieRolled);
            OnDieRolled(gameManager.CurrentDieRoll);
        }

        private void OnDisable()
        {
            gameManager.OnDieRolled.RemoveListener(OnDieRolled);
        }

        private void OnDieRolled(int dieValue)
        {
            text.text = dieValue.ToString();
        }
    }
}

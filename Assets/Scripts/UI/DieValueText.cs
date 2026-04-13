using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    public class DieValueText : MonoBehaviour
    {
        private TextMeshProUGUI text;
        [Inject] private GameManager _gameManager;

        void OnEnable()
        {
            text = GetComponent<TextMeshProUGUI>();
            _gameManager.OnDieRolled.AddListener(OnDieRolled);
            OnDieRolled(_gameManager.CurrentDieRoll);
        }

        private void OnDisable()
        {
            _gameManager.OnDieRolled.RemoveListener(OnDieRolled);
        }

        private void OnDieRolled(int dieValue)
        {
            text.text = dieValue.ToString();
        }
    }
}

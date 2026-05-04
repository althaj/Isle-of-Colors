using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    public class SetupScoringPanel : MonoBehaviour
    {
        [SerializeField] private GameObject background;

        public UnityEvent OnSetupScoringPanelClosed;

        [Inject] private GameManager _gameManager;
        private void Start()
        {
            background.SetActive(false);
            _gameManager.InvokeAfterInitialization(OnGameInitialized);
        }

        private void OnGameInitialized()
        {
            background.SetActive(true);
            _gameManager.OnGameInitialized.RemoveListener(OnGameInitialized);
        }

        public void Close()
        {
            background.SetActive(false);
            OnSetupScoringPanelClosed?.Invoke();
        }
    }
}

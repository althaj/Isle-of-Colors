using System;
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
            _gameManager.OnScoringSetupFinished.AddListener(OnScoringSetupFinished);
            background.SetActive(false);
        }

        private void OnScoringSetupFinished()
        {
            background.SetActive(true);
        }

        public void Close()
        {
            background.SetActive(false);
            OnSetupScoringPanelClosed?.Invoke();
        }
    }
}

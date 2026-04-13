using System;
using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    public class LastRoundPopup : MonoBehaviour
    {
        [Inject] private GameManager _gameManager;

        [SerializeField] private GameObject popup;

        private void Start()
        {
            _gameManager.OnLastRoundStarted.AddListener(OnLastRoundStarted);
            ClosePopup();
        }

        public void ClosePopup()
        {
            popup.SetActive(false);
        }

        private void OnLastRoundStarted()
        {
            popup.SetActive(true);
        }
        
    }
}

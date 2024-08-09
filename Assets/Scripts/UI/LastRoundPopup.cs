using System;
using PSG.IsleOfColors.Gameplay;
using UnityEngine;

namespace PSG.IsleOfColors.UI
{
    public class LastRoundPopup : MonoBehaviour
    {
        private GameManager gameManager;

        [SerializeField] private GameObject popup;

        private void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            gameManager.OnLastRoundStarted.AddListener(OnLastRoundStarted);
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

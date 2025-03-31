using System;
using PSG.IsleOfColors.Gameplay;
using UnityEngine;

namespace PSG.IsleOfColors.UI
{
    public class SetupScoringPanel : MonoBehaviour
    {
        [SerializeField] private GameObject background;

        private GameManager gameManager;

        private void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            gameManager.OnScoringSetupFinished.AddListener(OnScoringSetupFinished);
            background.SetActive(false);
        }

        private void OnScoringSetupFinished()
        {
            background.SetActive(true);
        }

        public void Close()
        {
            background.SetActive(false);
        }
    }
}

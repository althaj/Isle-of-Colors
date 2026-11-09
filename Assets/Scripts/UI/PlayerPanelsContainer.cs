using System;
using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    public class PlayerPanelsContainer : MonoBehaviour
    {
        [SerializeField] private PlayerPanel playerPanelPrefab;

        [Inject] private GameManager _gameManager;

        void Start()
        {
            if (_gameManager == null)
            {
                Debug.LogError($"[PlayerPanelsContainer::Start] Game manager is invalid.");
                return;
            }

            if (_gameManager.IsGameInitialized)
            {
                InitializePlayerPanels();
            }
            else
            {
                _gameManager.OnGameInitialized?.AddListener(OnGameInitialized);
            }
        }

        private void OnGameInitialized()
        {
            InitializePlayerPanels();
            _gameManager.OnGameInitialized?.RemoveListener(OnGameInitialized);
        }

        private void InitializePlayerPanels()
        {
            if (_gameManager == null)
            {
                Debug.LogError($"[PlayerPanelsContainer::OnGameInitialized] Game manager is invalid.");
                return;
            }

            for (int i = 0; i < _gameManager.Players.Length; i++)
            {
                PlayerPanel playerPanel = Instantiate(playerPanelPrefab, transform);
                if (playerPanel == null)
                {
                    Debug.LogError($"[PlayerPanelsContainer::OnGameInitialized] Player panel is invalid.");
                    continue;
                }

                playerPanel.Initialize(i + 1, _gameManager.Players[i]);
            }
        }
    }
}

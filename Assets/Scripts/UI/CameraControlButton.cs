using System;
using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    public class CameraControlButton : MonoBehaviour
    {
        private Button button;
        [Inject] private GameManager _gameManager;

        void Start()
        {
            button = GetComponent<Button>();
        }

        public void ChangeCurrentPlayer()
        {
            _gameManager.ChangeCurrentPlayer();
        }
    }
}

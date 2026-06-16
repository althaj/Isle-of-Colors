using System.Linq;
using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.UI.Developer
{
    public class DeveloperUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        [Inject] private GameManager _gameManager;

        void Start()
        {
            panel.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2) && Debug.isDebugBuild)
            {
                panel.SetActive(!panel.activeSelf);
            }
        }

        public void ToggleCoordinates()
        {
            var spaces = FindObjectsByType<Hex>(FindObjectsSortMode.None);
            if (spaces.Any())
            {
                bool newState = !spaces.First().transform.GetChild(0).GetChild(0).gameObject.activeSelf;
                foreach (var space in spaces)
                {
                    var coordinateObject = space.transform.GetChild(0).GetChild(0);
                    coordinateObject.gameObject.SetActive(newState);
                }
            }
        }

        public void RollDie(int value)
        {
            _gameManager.RollDie(value);
            foreach(Player player in _gameManager.Players)
            {
                player.StartTurn(value);
            }
        }
    }
}


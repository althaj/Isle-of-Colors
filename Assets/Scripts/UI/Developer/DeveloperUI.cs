using System;
using System.Linq;
using System.Text;
using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.UI.Developer
{
    public class DeveloperUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        [SerializeField] private TextMeshProUGUI playerColorsText;

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

                foreach (Player player in _gameManager.Players)
                {
                    if (panel.activeSelf)
                    {
                        player.OnPlayerColorsChanged.AddListener(OnPlayerColorsChanged);
                        OnPlayerColorsChanged();
                    }
                    else
                    {
                        player.OnPlayerColorsChanged.RemoveListener(OnPlayerColorsChanged);
                    }
                }
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
            foreach (Player player in _gameManager.Players)
            {
                player.StartTurn(value);
            }
        }

        private void OnPlayerColorsChanged()
        {
            StringBuilder playerColorStringBuilder = new();

            foreach (Player player in _gameManager.Players)
            {
                playerColorStringBuilder.AppendLine($"{player.Name}: {string.Join(", ", player.Colors.Select(c => c.Name))}.");
            }

            playerColorsText.text = playerColorStringBuilder.ToString();
        }
    }
}


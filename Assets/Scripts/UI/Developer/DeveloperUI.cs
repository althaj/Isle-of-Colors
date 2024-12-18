using System.Linq;
using PSG.IsleOfColors.Gameplay;
using UnityEngine;

namespace PSG.IsleOfColors.UI.Developer
{
    public class DeveloperUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

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
                    space.transform.GetChild(0).GetChild(0).gameObject.SetActive(newState);
                }
            }
        }

        public void RollDie(int value)
        {
            FindFirstObjectByType<GameManager>().RollDie(value);
            foreach (var player in FindObjectsByType<Player>(FindObjectsSortMode.None))
            {
                player.StartTurn(value);
            }
        }
    }
}


using System;
using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI.MainMenu
{
    public class GameOptionsPopup : MonoBehaviour
    {
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject popupPanel;

        [SerializeField] private GameObject[] hideWhenSinglePlayer;
        [SerializeField] private GameObject[] hideWhenMultiplayer;

        [SerializeField] private TMP_InputField player1NameInput;
        [SerializeField] private TMP_InputField player2NameInput;
        [SerializeField] private TMP_Dropdown difficultyDropdown;
        [SerializeField] private Toggle tutorialToggle;

        [SerializeField] private TextMeshProUGUI validationErrorLabel;

        private bool isSinglePlayer;

        public void OpenPopup(bool isSinglePlayer)
        {
            this.isSinglePlayer = isSinglePlayer;

            if (isSinglePlayer)
            {
                foreach (var element in hideWhenMultiplayer)
                    element.SetActive(true);

                foreach (var element in hideWhenSinglePlayer)
                    element.SetActive(false);
            }
            else
            {
                foreach (var element in hideWhenMultiplayer)
                    element.SetActive(false);

                foreach (var element in hideWhenSinglePlayer)
                    element.SetActive(true);
            }

            ClearValidationError();

            background.SetActive(true);
            popupPanel.SetActive(true);
        }

        public void ClosePopup()
        {
            background.SetActive(false);
            popupPanel.SetActive(false);

            LoadPlayerPrefs();
        }

        public void Cancel()
        {
            ClosePopup();
        }

        public void Confirm()
        {
            if (!ValidateGameOptions())
                return;

            SavePlayerPrefs();

            GameOptions options = new GameOptions
            {
                Player1Name = player1NameInput.text,
                Player2Name = player2NameInput.text,
                Difficulty = (GameOptions.BotDifficulty)difficultyDropdown.value,
                ShowTutorial = tutorialToggle.isOn,
                IsSinglePlayer = isSinglePlayer
            };

            if (isSinglePlayer)
            {
                switch (difficultyDropdown.value)
                {
                    case 0: options.Player2Name = "David BOT"; break;
                    case 1: options.Player2Name = "Vierka BOT"; break;
                    case 2: options.Player2Name = "Janči BOT"; break;
                }
            }

            ApplicationManager.Instance.StartGame(options);
        }

        private void LoadPlayerPrefs()
        {
            player1NameInput.text = PlayerPrefs.GetString("Player1Name");
            player2NameInput.text = PlayerPrefs.GetString("Player2Name");
            difficultyDropdown.value = PlayerPrefs.GetInt("Difficulty");
            tutorialToggle.isOn = PlayerPrefs.GetInt("ShowTutorial") == 1;
        }

        private void SavePlayerPrefs()
        {
            PlayerPrefs.SetString("Player1Name", player1NameInput.text);
            PlayerPrefs.SetInt("ShowTutorial", tutorialToggle.isOn ? 1 : 0);

            if (isSinglePlayer)
                PlayerPrefs.SetInt("Difficulty", difficultyDropdown.value);
            else
                PlayerPrefs.SetString("Player2Name", player2NameInput.text);
        }

        private bool ValidateGameOptions()
        {
            if (String.IsNullOrWhiteSpace(player1NameInput.text))
            {
                ShowValidationError("Player 1 name cannot be empty.", player1NameInput.gameObject);
                return false;
            }

            if (String.IsNullOrWhiteSpace(player2NameInput.text) && !isSinglePlayer)
            {
                ShowValidationError("Player 2 name cannot be empty.", player2NameInput.gameObject);
                return false;
            }

            ClearValidationError();
            
            return true;
        }

        private void ClearValidationError()
        {
            validationErrorLabel.gameObject.SetActive(false);
        }

        private void ShowValidationError(string errorMessage, GameObject element)
        {
            validationErrorLabel.text = errorMessage;
            validationErrorLabel.gameObject.SetActive(true);

            EventSystem.current.SetSelectedGameObject(element);
        }
    }
}

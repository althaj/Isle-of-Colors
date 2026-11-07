using System;
using System.Collections.Generic;
using System.Linq;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using static PSG.IsleOfColors.Gameplay.GameOptions;

namespace PSG.IsleOfColors.UI.MainMenu
{
    public class GameOptionsPopup : MonoBehaviour
    {
        private const string GAME_OPTIONS_KEY = "GameOptions";

        [SerializeField] private GameObject background;
        [SerializeField] private GameObject popupPanel;

        [SerializeField] private RectTransform playerRowContainer;
        [SerializeField] private PlayerRow playerRowPrefab;

        [SerializeField] private Toggle tutorialToggle;

        [SerializeField] private TextMeshProUGUI validationErrorLabel;

        [Header("Bot names")]
        [SerializeField] private string[] easyBotNames;
        [SerializeField] private string[] mediumBotNames;
        [SerializeField] private string[] hardBotNames;

        private List<PlayerRow> playerRows = new();

        [Inject] private ApplicationManager _applicationManager;
        [Inject] private AudioManager _audioManager;
        [Inject] private AnalyticsManager _analyticsManager;

        [Inject] private DiContainer _container;

        public void OpenPopup()
        {
            ClearValidationError();

            LoadPlayerPrefs();

            background.SetActive(true);
            popupPanel.SetActive(true);
        }

        public void AddPlayer()
        {
            AddPlayer(new());
        }

        public void AddPlayer(PlayerOptions playerOptions)
        {
            if (playerRows.Count >= 4)
            {
                ShowValidationError("Maximum player count is 4.");
                _audioManager.PlayUISound(UIAudioType.Error);
                return;
            }

            ClearValidationError();

            PlayerRow newPlayerRow = _container.InstantiatePrefab(playerRowPrefab, playerRowContainer).GetComponent<PlayerRow>();
            if (newPlayerRow == null)
            {
                Debug.LogError($"[GameOptionsPopup::AddPlayer] Failed to instantiate new player row.");
                return;
            }

            int botDifficulty = 0;
            if (playerOptions.PlayerType == PlayerType.MediumBot)
                botDifficulty = 1;
            else if (playerOptions.PlayerType == PlayerType.HardBot)
                botDifficulty = 2;

            playerRows.Add(newPlayerRow);
            newPlayerRow.Initialize(
                playerRows.Count,
                playerOptions.PlayerType == PlayerType.Human,
                playerOptions.PlayerName,
                botDifficulty);

            newPlayerRow.OnPlayerRemoved?.AddListener(OnPlayerRowRemoved);
        }

        private void OnPlayerRowRemoved(int playerNumber)
        {
            PlayerRow playerRow = playerRows.FirstOrDefault(pr => pr.PlayerNumber == playerNumber);
            if (playerRow == null)
            {
                Debug.LogError($"[GameOptionsPopup::OnPlayerRemoved] Player with number {playerNumber} does not exist.");
                return;
            }

            playerRows.Remove(playerRow);
            for (int i = 0; i < playerRows.Count; i++)
            {
                playerRows[i].SetPlayerNumber(i + 1);
            }

            ClearValidationError();
        }

        public void ClosePopup()
        {
            for (int i = 0; i < playerRowContainer.childCount; i++)
            {
                Destroy(playerRowContainer.GetChild(i).gameObject);
            }

            playerRows.Clear();

            background.SetActive(false);
            popupPanel.SetActive(false);
        }

        public void Confirm()
        {
            if (!ValidateGameOptions())
            {
                _audioManager.PlayUISound(UIAudioType.Error);
                return;
            }

            SavePlayerPrefs();
            GameOptions options = GetGameOptions();

            _audioManager.PlayUISound(UIAudioType.Confirm);

            // TODO: Fix analytics for multiplayer
            // _analyticsManager.GameStarted(isSinglePlayer ? GetSecondPlayerType() : null);

            _applicationManager.StartGame(options);
        }

        private GameOptions GetGameOptions()
        {
            return new GameOptions
            {
                Players = playerRows.Select(pr => GetPlayerOptionsFromRow(pr)).ToList(),
                ShowTutorial = tutorialToggle.isOn
            };
        }

        private void LoadPlayerPrefs()
        {
            if (!PlayerPrefs.HasKey(GAME_OPTIONS_KEY))
            {
                return;
            }

            GameOptions gameOptions = JsonUtility.FromJson<GameOptions>(PlayerPrefs.GetString(GAME_OPTIONS_KEY));

            if (gameOptions.Players == null)
            {
                return;
            }

            foreach (PlayerOptions playerOptions in gameOptions.Players)
            {
                AddPlayer(playerOptions);
            }

            tutorialToggle.isOn = gameOptions.ShowTutorial;
        }

        public void SavePlayerPrefs()
        {
            PlayerPrefs.SetString(GAME_OPTIONS_KEY, JsonUtility.ToJson(GetGameOptions()));
        }

        private bool ValidateGameOptions()
        {
            if (playerRows.Count < 2)
            {
                ShowValidationError("At least 2 players are needed to play the game.");
                return false;
            }

            if (!playerRows.Any(pr => pr.IsHuman))
            {
                ShowValidationError("At least one player has to be human.");
                return false;
            }

            if (playerRows.Any(pr => pr.IsHuman && string.IsNullOrEmpty(pr.HumanPlayerName)))
            {
                ShowValidationError("Player name cannot be empty.");
                return false;
            }

            ClearValidationError();

            return true;
        }

        private void ClearValidationError()
        {
            validationErrorLabel.gameObject.SetActive(false);
        }

        private void ShowValidationError(string errorMessage, GameObject element = null)
        {
            validationErrorLabel.text = errorMessage;
            validationErrorLabel.gameObject.SetActive(true);

            if (element != null)
                EventSystem.current.SetSelectedGameObject(element);
        }

        private PlayerOptions GetPlayerOptionsFromRow(PlayerRow playerRow)
        {
            if (playerRow == null)
            {
                Debug.LogError($"[GameOptionsPopup::GetPlayerOptionsFromRow] Player row is invalid.");
                return default;
            }

            return new()
            {
                PlayerName = playerRow.IsHuman ? playerRow.HumanPlayerName : GetBotName(playerRow),
                PlayerType = playerRow.PlayerType

            };
        }

        private string GetBotName(PlayerRow playerRow)
        {
            if (playerRow.IsHuman)
                return null;

            switch (playerRow.BotDifficulty)
            {
                case 0:
                    if (easyBotNames == null || easyBotNames.Length == 0)
                    {
                        Debug.LogError($"[GameOptionsPopup::GetBotName] Easy bot names is invalid.");
                        return null;
                    }
                    return easyBotNames[playerRow.PlayerNumber % easyBotNames.Length];
                case 1:
                    if (mediumBotNames == null || mediumBotNames.Length == 0)
                    {
                        Debug.LogError($"[GameOptionsPopup::GetBotName] Medium bot names is invalid.");
                        return null;
                    }
                    return mediumBotNames[playerRow.PlayerNumber % mediumBotNames.Length];
                case 2:
                    if (hardBotNames == null || hardBotNames.Length == 0)
                    {
                        Debug.LogError($"[GameOptionsPopup::GetBotName] Hard bot names is invalid.");
                        return null;
                    }
                    return hardBotNames[playerRow.PlayerNumber % hardBotNames.Length];
                default:
                    Debug.LogError($"[GameOptionsPopup::GetBotName] Bot difficulty {playerRow.BotDifficulty} is invalid.");
                    return null;

            }
        }
    }
}

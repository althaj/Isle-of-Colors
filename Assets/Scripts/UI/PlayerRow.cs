using System;
using System.Collections.Generic;
using ModestTree;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static PSG.IsleOfColors.Gameplay.GameOptions;

namespace PSG.IsleOfColors.UI
{
    public class PlayerRow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNumberText;

        [SerializeField] private ToggleButton humanBotToggle;
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private RectTransform botDificultyPanel;

        [SerializeField] private Button easyButton;
        [SerializeField] private Button mediumButton;
        [SerializeField] private Button hardButton;

        [SerializeField] private Button removePlayerButton;

        public UnityEvent<int> OnPlayerRemoved = new();

        public int PlayerNumber { get; private set; }
        public bool IsHuman { get; private set; }
        public int BotDifficulty { get; private set; }

        public string HumanPlayerName
        {
            get
            {
                if (!IsHuman) return null;
                if (playerNameInput == null)
                {
                    Debug.LogError($"[PlayerRow::HumanPlayerName] Player name input is invalid.");
                    return null;
                }

                return playerNameInput.text;
            }
        }

        public PlayerType PlayerType
        {
            get
            {
                if (IsHuman)
                {
                    return PlayerType.Human;
                }

                switch (BotDifficulty)
                {
                    case 0: return PlayerType.EasyBot;
                    case 1: return PlayerType.MediumBot;
                    case 2: return PlayerType.HardBot;
                    default:
                        Debug.LogError($"[PlayerRow::PlayerType] Bot difficulty is invalid.");
                        return PlayerType.EasyBot;
                }
            }
        }

        public void Initialize(int playerNumber, bool isHuman = true, string humanPlayerName = "", int botDifficulty = 1)
        {
            SetPlayerNumber(playerNumber);

            humanBotToggle.SetState(isHuman);
            OnHumanBotToggled(isHuman);

            if(isHuman)
                playerNameInput.text = humanPlayerName;

            switch (botDifficulty)
            {
                case 0: OnBotDifficultySetToEasy(); break;
                case 1: OnBotDifficultySetToMedium(); break;
                case 2: OnBotDifficultySetToHard(); break;
                default:
                    Debug.LogError($"[PlayerRow::Initialize] {botDifficulty} is not a valid difficulty.");
                    break;
            }
        }

        public void SetPlayerNumber(int playerNumber)
        {
            this.PlayerNumber = playerNumber;

            if (playerNumberText == null)
            {
                Debug.LogError($"[PlayerRow::Initialize] Player number text is invalid.");
                return;
            }

            playerNumberText.text = playerNumber.ToString();
        }

        void OnEnable()
        {
            if (easyButton == null || mediumButton == null || hardButton == null)
            {
                Debug.LogError($"[PlayerRow::OnEnable] Bot difficulty buttons are invalid.");
                return;
            }

            if (humanBotToggle == null)
            {
                Debug.LogError($"[PlayerRow::OnEnable] Human / bot toggle is invalid.");
                return;
            }

            if (removePlayerButton == null)
            {
                Debug.LogError($"[PlayerRow::OnEnable] Remove player button is invalid.");
                return;
            }

            easyButton.onClick?.AddListener(OnBotDifficultySetToEasy);
            mediumButton.onClick?.AddListener(OnBotDifficultySetToMedium);
            hardButton.onClick?.AddListener(OnBotDifficultySetToHard);

            humanBotToggle.OnToggled?.AddListener(OnHumanBotToggled);

            removePlayerButton.onClick?.AddListener(OnRemovePlayer);
        }

        void OnDisable()
        {
            if (easyButton == null || mediumButton == null || hardButton == null)
            {
                Debug.LogError($"[PlayerRow::OnDisable] Bot difficulty buttons are invalid.");
                return;
            }

            if (humanBotToggle == null)
            {
                Debug.LogError($"[PlayerRow::OnDisable] Human / bot toggle is invalid.");
                return;
            }

            if (removePlayerButton == null)
            {
                Debug.LogError($"[PlayerRow::OnDisable] Remove player button is invalid.");
                return;
            }

            easyButton.onClick?.RemoveListener(OnBotDifficultySetToEasy);
            mediumButton.onClick?.RemoveListener(OnBotDifficultySetToMedium);
            hardButton.onClick?.RemoveListener(OnBotDifficultySetToHard);

            humanBotToggle.OnToggled?.RemoveListener(OnHumanBotToggled);
        }

        private void OnBotDifficultySetToEasy()
        {
            BotDifficulty = 0;
            HighlightButton(easyButton);
        }

        private void OnBotDifficultySetToMedium()
        {
            BotDifficulty = 1;
            HighlightButton(mediumButton);
        }

        private void OnBotDifficultySetToHard()
        {
            BotDifficulty = 2;
            HighlightButton(hardButton);
        }

        private void HighlightButton(Button button)
        {
            foreach (Button b in new List<Button> { easyButton, mediumButton, hardButton })
            {
                HighlightableButton highlight = b.GetComponent<HighlightableButton>();
                if (highlight == null)
                {
                    Debug.LogError($"[PlayerRow::HighlightButton] Button {b.name} is not highlightable button.");
                    continue;
                }
                highlight.SetHighlight(b == button);
            }
        }

        private void OnHumanBotToggled(bool isHuman)
        {
            IsHuman = isHuman;

            if (playerNameInput == null)
            {
                Debug.LogError($"[PlayerRow::OnHumanBotToggled] Player name input is invalid.");
                return;
            }

            if (botDificultyPanel == null)
            {
                Debug.LogError($"[PlayerRow::OnHumanBotToggled] Bot difficulty panel is invalid.");
                return;
            }

            if (isHuman)
            {
                playerNameInput.gameObject.SetActive(true);
                botDificultyPanel.gameObject.SetActive(false);
            }
            else
            {
                playerNameInput.gameObject.SetActive(false);
                botDificultyPanel.gameObject.SetActive(true);
            }
        }

        private void OnRemovePlayer()
        {
            OnPlayerRemoved?.Invoke(PlayerNumber);
            Destroy(gameObject);
        }
    }
}

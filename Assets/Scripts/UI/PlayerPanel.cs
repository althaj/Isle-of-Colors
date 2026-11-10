using System;
using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    [RequireComponent(typeof(HighlightUI))]
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNumberText;
        [SerializeField] private TextMeshProUGUI playerNameText;

        [SerializeField] private Image color1Image;
        [SerializeField] private Image color2Image;

        [Inject] private GameManager _gameManager;

        private Player player;
        private HighlightUI highlight;

        private bool isInitialized = false;

        void Start()
        {
            highlight = GetComponent<HighlightUI>();
        }

        public void Initialize(int playerNumber, Player player)
        {
            if (player == null)
            {
                Debug.LogError($"[PlayerPanel::Initialize] Player is invalid.");
                return;
            }

            this.player = player;

            if (playerNameText == null)
            {
                Debug.LogError($"[PlayerPanel::Initialize] Player name text is invalid.");
                return;
            }

            playerNameText.text = player.Name;

            if (playerNumberText == null)
            {
                Debug.LogError($"[PlayerPanel::Initialize] Player number text is invalid.");
                return;
            }

            playerNumberText.text = playerNumber.ToString();

            ColorScore[] colorScores = GetComponentsInChildren<ColorScore>();
            foreach (ColorScore colorScore in colorScores)
            {
                colorScore.Initialize(player);
            }

            TotalScore totalScore = GetComponentInChildren<TotalScore>();
            if (totalScore == null)
            {
                Debug.LogError($"[PlayerPanel::Initialize] Total score is not valid.");
                return;
            }
            totalScore.Initialize(player);

            if (_gameManager == null)
            {
                Debug.LogError($"[PlayerPanel::Initialize] Game manager is not valid.");
                return;
            }

            _gameManager.OnCurrentPlayerChanged?.AddListener(OnCurrentPlayerChanged);
            OnCurrentPlayerChanged(_gameManager.CurrentPlayer);

            player.OnPlayerColorsChanged?.AddListener(OnPlayerColorsChanged);
            OnPlayerColorsChanged();
        }

        void OnEnable()
        {
            if (!isInitialized)
            {
                return;
            }

            if (_gameManager == null)
            {
                Debug.LogError($"[PlayerPanel::OnEnable] Game manager is not valid.");
                return;
            }

            _gameManager.OnCurrentPlayerChanged?.AddListener(OnCurrentPlayerChanged);
            OnCurrentPlayerChanged(_gameManager.CurrentPlayer);

            if (player == null)
            {
                Debug.LogError($"[PlayerPanel::OnEnable] Player is invalid.");
                return;
            }

            player.OnPlayerColorsChanged?.AddListener(OnPlayerColorsChanged);
            OnPlayerColorsChanged();

        }

        void OnDisable()
        {
            if (_gameManager == null)
            {
                Debug.LogError($"[PlayerPanel::OnDisable] Game manager is not valid.");
                return;
            }

            _gameManager.OnCurrentPlayerChanged?.RemoveListener(OnCurrentPlayerChanged);

            if (player == null)
            {
                Debug.LogError($"[PlayerPanel::OnDisable] Player is invalid.");
                return;
            }

            player.OnPlayerColorsChanged?.RemoveListener(OnPlayerColorsChanged);
        }

        private void OnCurrentPlayerChanged(Player currentPlayer)
        {
            if (highlight == null)
            {
                highlight = GetComponent<HighlightUI>();
            }

            if (highlight == null)
            {
                Debug.LogError($"[PlayerPanel::OnCurrentPlayerChanged] Highlight is invalid.");
                return;
            }

            highlight.SetHighlight(currentPlayer == player);
        }

        private void OnPlayerColorsChanged()
        {
            if (player == null)
            {
                Debug.LogError($"[PlayerPanel::OnPlayerColorsChanged] Player is invalid.");
                return;
            }

            if (color1Image == null || color2Image == null)
            {
                Debug.LogError($"[PlayerPanel::OnPlayerColorsChanged] Color images are invalid.");
                return;
            }

            if (player.Colors.Count > 0 && player.Colors[0] != null)
            {
                color1Image.color = player.Colors[0].Color;
                color1Image.enabled = true;
            }
            else
            {
                color1Image.enabled = false;
            }

            if (player.Colors.Count > 1 && player.Colors[1] != null)
            {
                color2Image.color = player.Colors[1].Color;
                color2Image.enabled = true;
            }
            else
            {
                color2Image.enabled = false;
            }
        }

        public void SwitchToPlayer()
        {
            if (player == null)
            {
                Debug.LogError($"[PlayerPanel::SwitchToPlayer] Player is invalid.");
                return;
            }

            if (_gameManager == null)
            {
                Debug.LogError($"[PlayerPanel::SwitchToPlayer] Game manager is invalid.");
                return;
            }

            _gameManager.SetCurrentPlayer(player);
        }
    }
}

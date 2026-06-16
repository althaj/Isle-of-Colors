using System.Linq;
using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    public class ColorsPanel : MonoBehaviour
    {
        [SerializeField] private GameObject colorButtonPrefab;

        private Player player;

        [Inject] private GameManager _gameManager;
        [Inject] private DiContainer _container;

        void Start()
        {
            _gameManager.InvokeAfterInitialization(OnGameInitialized);
        }

        private void OnGameInitialized()
        {
            _gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);

            OnCurrentPlayerChanged(_gameManager.Players.FirstOrDefault());

            _gameManager.OnGameInitialized.RemoveListener(OnGameInitialized);
        }

        private void OnCurrentPlayerChanged(Player currentPlayer)
        {
            if(currentPlayer == null)
            {
                Debug.LogError($"[ColorsPanel::OnCurrentPlayerChanged] Current player is invalid.");
                return;
            }

            if (player != null)
            {
                player.OnColorUsageChanged.RemoveListener(OnPlayerColorsChanged);
                player.OnSelectedColorChanged.RemoveListener(OnSelectedColorChanged);
            }

            player = currentPlayer;
            player.OnPlayerColorsChanged.AddListener(OnPlayerColorsChanged);
            player.OnSelectedColorChanged.AddListener(OnSelectedColorChanged);
            OnPlayerColorsChanged();
        }

        private void OnPlayerColorsChanged()
        {
            if (player == null || player.Colors == null)
                return;

            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            foreach (var color in player.Colors)
                CreateButton(color);
        }

        private void CreateButton(PencilColor color)
        {
            GameObject button = _container.InstantiatePrefab(colorButtonPrefab, transform);

            ColorButton colorButton = button.AddComponent<ColorButton>() as ColorButton;
            if(colorButton != null && player != null)
            {
                colorButton.Initialize(color, player);
            }
        }

        void OnSelectedColorChanged()
        {
            OnPlayerColorsChanged();
        }
    }
}

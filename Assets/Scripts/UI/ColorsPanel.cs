using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    public class ColorsPanel : MonoBehaviour
    {
        [SerializeField] private GameObject colorButtonPrefab;

        private Player player;
        private GameManager gameManager;

        void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            gameManager.OnCurrentPlayerChanged.AddListener(OnCurrentPlayerChanged);

            OnCurrentPlayerChanged(gameManager.Player1, gameManager.Player2);
        }

        private void OnCurrentPlayerChanged(Player currentPlayer, Player otherPlayer)
        {
            if (player != null)
                player.OnColorUsageChanged.RemoveListener(OnPlayerColorsChanged);

            player = currentPlayer;
            player.OnPlayerColorsChanged.AddListener(OnPlayerColorsChanged);
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
            GameObject button = Instantiate(colorButtonPrefab, transform);
            button.GetComponent<Image>().color = color.Color;
            button.GetComponent<Button>().onClick.AddListener(() => player.StartColoring(color));
        }
    }
}

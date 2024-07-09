using PSG.IsleOfColors.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private GameObject colorButton;

        private Transform colorsPanel;

        private void Awake()
        {
            GetComponentInChildren<TextMeshProUGUI>().text = player.Name;

            // TODO upraviù?
            colorsPanel = transform.GetChild(1);

            player.OnPlayerColorsChanged.AddListener(OnPlayerColorsChanged);
        }

        private void OnPlayerColorsChanged()
        {
            for (int i = 0; i < colorsPanel.childCount; i++)
            {
                Destroy(colorsPanel.GetChild(i).gameObject);
            }

            foreach (var color in player.Colors)
                CreateButton(color);
        }

        private void CreateButton(PencilColor color)
        {
            GameObject button = Instantiate(colorButton, colorsPanel);
            button.GetComponent<Image>().color = color.Color;
            button.GetComponent<Button>().onClick.AddListener(() => player.StartColoring(color));
        }
    }
}

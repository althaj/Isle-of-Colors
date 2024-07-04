using PSG.IsleOfColors.Gameplay;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.Prototype
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private GameObject hexPrefab;
        [SerializeField] private GameObject emptyPrefab;
        [SerializeField] private Player player;

        [SerializeField] private GameObject hexPanel;

        private void SetupMap()
        {
            for (int y = 0; y < player.PlayerSheet.Spaces.Length; y++)
            {
                var rowGO = CreateRow(y + 1);
                for (int x = 0; x < player.PlayerSheet.Spaces[y].Length; x++)
                {
                    var hex = CreateObject(player.PlayerSheet.Spaces[y][x], rowGO, x, y);
                }
            }
        }

        private GameObject CreateRow(int index)
        {
            GameObject row = new GameObject($"Row {index}", new Type[] { typeof(HorizontalLayoutGroup) });
            row.transform.SetParent(hexPanel.transform);

            var layoutGroup = row.GetComponent<HorizontalLayoutGroup>();

            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;

            if (index % 2 == 0)
            {
                layoutGroup.padding.left = 35;
                layoutGroup.padding.right = -layoutGroup.padding.left;
            }

            return row;
        }

        private GameObject CreateObject(PlayerSheetSpace space, GameObject row, int x, int y)
        {
            int index = x + 1;
            bool isHex = space != null;
            GameObject obj = Instantiate(isHex ? hexPrefab : emptyPrefab, row.transform);
            if (isHex)
            {
                obj.name = $"Hex {index}";
                obj.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
                obj.GetComponent<HexButton>().AttachSpace(space);
                obj.GetComponent<Button>().onClick.AddListener(() => ColorSpace(x, y));
            }
            else
            {
                obj.name = $"Empty {index}";
            }

            return obj;
        }

        internal void Initialize()
        {
            player.PlayerSheet.OnMapGenerated.AddListener(SetupMap);
        }

        public void StartColoring(PencilColor color)
        {
            player.PlayerSheet.StartColoring(color);
        }

        public void ColorSpace(int x, int y)
        {
            player.PlayerSheet.SetColor(x, y);
        }

        public void Undo()
        {
            player.PlayerSheet.Undo();
        }

        public void Confirm()
        {
            player.PlayerSheet.Confirm();
        }
    }
}

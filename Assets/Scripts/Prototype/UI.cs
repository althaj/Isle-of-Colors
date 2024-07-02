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

        void Start()
        {
            player.PlayerSheet.OnMapGenerated.AddListener(SetupMap);
        }

        private void SetupMap()
        {
            for (int x = 0; x < player.PlayerSheet.Spaces.Length; x++)
            {
                var rowGO = CreateRow(x + 1);
                for (int y = 0; y < player.PlayerSheet.Spaces[x].Length; y++)
                {
                    if (player.PlayerSheet.Spaces[x][y] != null)
                    {
                        var hex = CreateObject(hexPrefab, y + 1, rowGO, true);
                    }
                    else
                        CreateObject(emptyPrefab, y + 1, rowGO, false);
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

        private GameObject CreateObject(GameObject prefab, int index, GameObject row, bool isHex)
        {
            GameObject obj = Instantiate(prefab, row.transform);
            if (isHex)
            {
                obj.name = $"Hex {index}";
                obj.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
            }
            else
            {
                obj.name = $"Empty {index}";
            }

            return obj;
        }
    }
}

using PSG.IsleOfColors.Prototype;
using System;
using TMPro;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay
{
    public class GameMap : MonoBehaviour
    {
        [SerializeField] private GameObject hexPrefab;

        private Player player;

        private float deltaY = Mathf.Cos(Mathf.Deg2Rad * 30);

        private void Start()
        {
            player = GetComponent<Player>();
        }

        internal void CreateMap()
        {
            for (int y = 0; y < player.PlayerSheet.Spaces.Length; y++)
            {
                for (int x = 0; x < player.PlayerSheet.Spaces[y].Length; x++)
                {
                    var hex = CreateObject(x, y);
                }
            }
        }

        private GameObject CreateObject(int x, int y)
        {
            var space = player.PlayerSheet.Spaces[y][x];

            float startX = player.PlayerSheet.Spaces[y].Length / -2;
            float startY = player.PlayerSheet.Spaces.Length / -2 * deltaY;

            if (space != null)
            {
                GameObject obj = Instantiate(hexPrefab, transform);

                Vector3 position = new Vector3(startX + x, startY + y * deltaY, 0);
                if (y % 2 == 1)
                    position.x += 0.5f;

                obj.transform.localPosition = position;

                obj.name = $"Hex {x} {y}";

                obj.GetComponent<Hex>().AttachSpace(space, player, x, y);
                return obj;
            }

            return null;
        }
    }

}
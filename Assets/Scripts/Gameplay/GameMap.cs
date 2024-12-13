using UnityEngine;

namespace PSG.IsleOfColors.Gameplay
{
    public class GameMap : MonoBehaviour
    {
        [SerializeField] private GameObject hexPrefab;

        private float deltaY = Mathf.Cos(Mathf.Deg2Rad * 30);


        internal void CreateMap()
        {
            Player player = GetComponent<Player>();

            Transform parent = new GameObject("Map").transform;
            parent.SetParent(transform);
            parent.localPosition = Vector3.zero;

            for (int y = 0; y < player.PlayerSheet.Spaces.Length; y++)
            {
                for (int x = 0; x < player.PlayerSheet.Spaces[y].Length; x++)
                {
                    var hex = CreateObject(player, x, y, parent);
                }
            }
        }

        private GameObject CreateObject(Player player, int x, int y, Transform parent)
        {
            var space = player.PlayerSheet.Spaces[y][x];

            float startX = player.PlayerSheet.Spaces[y].Length / -2;
            float startY = player.PlayerSheet.Spaces.Length / -2 * deltaY;

            if (space != null)
            {
                GameObject obj = Instantiate(hexPrefab, parent);

                Vector3 position = new Vector3(startX + x, startY + y * deltaY, 0);
                if (y % 2 == 1)
                    position.x += 0.5f;

                obj.transform.localPosition = position;

                obj.name = $"Hex {x} {y}";

                obj.GetComponent<Hex>().Initialize(space, player);
                return obj;
            }

            return null;
        }
    }

}
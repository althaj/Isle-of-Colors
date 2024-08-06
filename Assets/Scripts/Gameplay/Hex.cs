using TMPro;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay
{
    public class Hex : MonoBehaviour
    {
        private Animator animator;
        private SpriteRenderer sprite;
        private Player player;
        private int x;
        private int y;

        private bool isEnabled = false;

        public void Initialize(PlayerSheetSpace space, Player player, int x, int y)
        {
            animator = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();
            
            this.player = player;
            this.x = x;
            this.y = y;

            GetComponentInChildren<TextMeshProUGUI>(true).text = $"{x},{y}\n{space.Q}";

            space.OnColorChanged.AddListener(OnColorChanged);
            space.OnEnabledChanged.AddListener(OnEnabledChanged);
        }

        private void OnEnabledChanged(bool enabled)
        {
            animator.SetBool("Enabled", enabled);
            isEnabled = enabled;
        }

        private void OnColorChanged(PencilColor color)
        {
            if(color == null)
                sprite.color = Color.white;
            else
                sprite.color = color.Color;
        }

        private void OnMouseDown()
        {
            if (isEnabled)
            {
                player.SetColor(x, y);
            }
        }
    }
}

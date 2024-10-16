using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay
{
    public class Hex : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer baseSpriteRenderer;
        [SerializeField] private SpriteRenderer enabledSpriteRenderer;
        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
        [SerializeField] private SpriteRenderer mainSpriteRenderer;
        [SerializeField] private List<SpriteRenderer> propSpriteRenderers;

        [SerializeField] private List<Sprite> emptySprites;

        private Animator animator;
        private Player player;
        private PlayerSheetSpace space;

        private bool isEnabled = false;

        private RNGManager.RNGInstance rngInstance;

        public void Initialize(PlayerSheetSpace space, Player player)
        {
            animator = GetComponent<Animator>();

            this.player = player;
            this.space = space;

            rngInstance = RNGManager.RNGManager.Manager["Hex"];

            GetComponentInChildren<TextMeshProUGUI>(true).text = $"{space.X},{space.Y}\n{space.Q}";

            space.OnColorChanged.AddListener(OnColorChanged);
            space.OnEnabledChanged.AddListener(OnEnabledChanged);

            baseSpriteRenderer.sprite = RNGManager.RNGManager.Manager["Hex"].NextElement(emptySprites);

            UpdateVisual();
        }

        private void OnEnabledChanged(bool enabled)
        {
            animator.SetBool("Enabled", enabled);
            isEnabled = enabled;

            UpdateVisual();
        }

        private void OnColorChanged(PencilColor color)
        {
            UpdateVisual();
        }

        private void OnMouseDown()
        {
            if (isEnabled)
            {
                player.SetColor(space.X, space.Y);
            }
        }

        private void UpdateVisual()
        {
            foreach (var propRenderer in propSpriteRenderers)
                propRenderer.enabled = false;

            if (space.Color == null)
            {
                enabledSpriteRenderer.enabled = isEnabled;

                backgroundSpriteRenderer.enabled = false;
                mainSpriteRenderer.enabled = false;
            }
            else
            {
                enabledSpriteRenderer.enabled = false;
                backgroundSpriteRenderer.color = space.Color.Color;
                backgroundSpriteRenderer.enabled = true;

                mainSpriteRenderer.sprite = rngInstance.NextElement(space.Color.MainSprites);
                mainSpriteRenderer.enabled = true;

                if (space.Color.PropSprites.Count > 0 && propSpriteRenderers.Count > 0 && rngInstance.NextFloat(100) > 80)
                {
                    var propRenderer = rngInstance.NextElement(propSpriteRenderers);
                    propRenderer.sprite = rngInstance.NextElement(space.Color.PropSprites);
                    propRenderer.enabled = true;
                }
            }
        }
    }
}

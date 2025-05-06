using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.Prototype
{
	public class HexButton : MonoBehaviour
	{
		private PlayerSheetSpace space;
        
        private Button button;
        private Animator animator;

        public void AttachSpace(PlayerSheetSpace space)
		{
			this.space = space;

			space.OnColorChanged.AddListener(OnColorChanged);
			space.OnEnabledChanged.AddListener(OnEnabledChanged);
		}

        private void OnEnabledChanged(bool enabled)
        {
            if(button == null)
                button = GetComponent<Button>();

            button.interactable = enabled;
        }

        private void OnColorChanged(PencilColor color, bool isNew)
        {            
            if (animator == null)
                animator = GetComponent<Animator>();

            animator.SetTrigger(color == null ? "NoColor" : color.Name);
        }
    }
}

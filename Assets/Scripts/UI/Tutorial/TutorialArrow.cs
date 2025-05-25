using UnityEngine;

namespace PSG.IsleOfColors.UI.Tutorial
{
    public enum TutorialArrowDirection
    {
        Up,
        UpLeft,
        UpRight,
        Left,
        Right
    }

    [RequireComponent(typeof(RectTransform))]
    public class TutorialArrow : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Transform parent;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            parent = rectTransform.parent;

            Hide();
        }

        /// <summary>
        /// Shows the tutorial arrow next to a rect transform.
        /// </summary>
        /// <param name="targetTransform">Target transform to show the arrow next to.</param>
        /// <param name="direction">Direction the arrow will be pointing.</param>
        /// <param name="offset">Offset from the target rect transform, in pivot, so value of 1 is the height of the arrow.</param>
        public void Show(RectTransform targetTransform, TutorialArrowDirection direction, float offset)
        {
            if(rectTransform == null)
                return;

            if(targetTransform == null)
            {
                Hide();
                return;
            }

            rectTransform.SetParent(targetTransform);
            
            SetDirection(direction, offset);

            SetChildrenActive(true);
        }

        /// <summary>
        /// Hide the tutorial arrow.
        /// </summary>
        public void Hide()
        {
            SetChildrenActive(false);
        }
        
        /// <summary>
        /// Set the direction of the arrow, together with the rotation and pivot.
        /// </summary>
        /// <param name="direction">Direction of the tutorial arrow.</param>
        /// <param name="offset">Offset from it's parent, in pivot, so value of 1 is the height of the arrow.</param>
        private void SetDirection(TutorialArrowDirection direction, float offset)
        {
            rectTransform.pivot = new Vector2(0.5f, 1 + offset);

            switch(direction)
            {
                case TutorialArrowDirection.UpLeft:
                    rectTransform.rotation = Quaternion.Euler(0, 0, 45.0f);
                    rectTransform.anchorMin = new Vector2(1, 0);
                    rectTransform.anchorMax = new Vector2(1, 0);
                    break;
                case TutorialArrowDirection.UpRight:
                    rectTransform.rotation = Quaternion.Euler(0, 0, -45.0f);
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(0, 0);
                    break;
                case TutorialArrowDirection.Left:
                    rectTransform.rotation = Quaternion.Euler(0, 0, 90.0f);
                    rectTransform.anchorMin = new Vector2(1, 0.5f);
                    rectTransform.anchorMax = new Vector2(1, 0.5f);
                    break;
                case TutorialArrowDirection.Right:
                    rectTransform.rotation = Quaternion.Euler(0, 0, -90.0f);
                    rectTransform.anchorMin = new Vector2(0, 0.5f);
                    rectTransform.anchorMax = new Vector2(0, 0.5f);
                    break;
                case TutorialArrowDirection.Up:
                default:
                    rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                    rectTransform.anchorMin = new Vector2(0.5f, 0);
                    rectTransform.anchorMax = new Vector2(0.5f, 0);
                    break;
            }
            rectTransform.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// Enable or disable all children.
        /// </summary>
        /// <param name="active"></param>
        private void SetChildrenActive(bool active)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(active);
            }
        }
    }
}

using UnityEngine;

namespace PSG.IsleOfColors.UI.Tutorial
{
    [CreateAssetMenu(fileName = "TutorialMessage", menuName = "Isle of Colors/Tutorial/Tutorial Message")]
    public class TutorialMessage : ScriptableObject
    {
        [Header("Message Settings")]
        public string Message;
        public TutorialMessagePosition MessagePosition;
        public TutorialHighlight Highlight;

        [Header("Tutorial Arrow Settings")]
        public string TutorialArrowTargetName;
        public TutorialArrowDirection TutorialArrowDirection;
        public float TutorialArrowOffset = 0.5f;

    }
}

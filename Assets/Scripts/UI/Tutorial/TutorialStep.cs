using UnityEngine;

namespace PSG.IsleOfColors.UI.Tutorial
{
    [CreateAssetMenu(fileName = "TutorialStep", menuName = "Isle of Colors/Tutorial/Tutorial Step")]
    public class TutorialStep : ScriptableObject
    {
        public TutorialMessage[] Messages;
    }
}

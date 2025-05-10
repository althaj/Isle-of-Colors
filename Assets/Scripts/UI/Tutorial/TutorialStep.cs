using UnityEngine;

public enum TutorialStepId
{
    Undefined,
    Welcome,
    UI
}

namespace PSG.IsleOfColors.UI.Tutorial
{
    [CreateAssetMenu(fileName = "TutorialStep", menuName = "Isle of Colors/Tutorial/Tutorial Step")]
    public class TutorialStep : ScriptableObject
    {
        public TutorialStepId Id;
        public TutorialMessage[] Messages;
    }
}

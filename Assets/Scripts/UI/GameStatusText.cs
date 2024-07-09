using PSG.IsleOfColors.Gameplay.StateMachine;
using PSG.IsleOfColors.Gameplay.StateMachine.States;
using TMPro;
using UnityEngine;

namespace PSG.IsleOfColors.UI
{
    public class GameStatusText : MonoBehaviour
    {
        private GameStateMachine stateMachine;
        private TextMeshProUGUI text;

        void Awake()
        {
            stateMachine = FindFirstObjectByType<GameStateMachine>();
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            stateMachine.OnStateChanged.AddListener(OnStateChanged);
        }

        private void OnDisable()
        {
            stateMachine.OnStateChanged.RemoveListener(OnStateChanged);
        }

        private void OnStateChanged(IState newState)
        {
            text.text = newState.GetDescription();
        }
    }
}
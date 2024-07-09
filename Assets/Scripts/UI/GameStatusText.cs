using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Gameplay.StateMachine;
using PSG.IsleOfColors.Gameplay.StateMachine.States;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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
            stateMachine.OnStateDescriptionChanged.AddListener(OnStatusTextChanged);
        }

        private void OnDisable()
        {
            stateMachine.OnStateDescriptionChanged.RemoveListener(OnStatusTextChanged);
        }

        private void OnStatusTextChanged(string description)
        {
            text.text = description;
        }
    }
}
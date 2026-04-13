using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Gameplay.StateMachine;
using PSG.IsleOfColors.Gameplay.StateMachine.States;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    public class GameStatusText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [Inject] private GameStateMachine stateMachine;

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
            if (text == null)
            {
                Debug.LogError("[GameStatusText:OnStatusTextChanged] Text is invalid.");
                return;
            }

            text.text = description;
        }
    }
}
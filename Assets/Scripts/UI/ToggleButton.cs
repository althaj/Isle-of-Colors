using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    [RequireComponent(typeof(Button))]
    public class ToggleButton : MonoBehaviour
    {
        private Button button;

        [SerializeField] private bool initialState = true;
        [SerializeField] private HighlightableButton highlightOn;
        [SerializeField] private HighlightableButton highlightOff;

        private bool state;
        private bool initialized = false;

        public UnityEvent<bool> OnToggled = new();

        void Start()
        {
            Initialize(initialState);
        }

        private void Initialize(bool state)
        {
            if (!initialized)
            {
                button = GetComponent<Button>();
                if (button == null)
                {
                    Debug.LogError($"[ToggleButton::Start] Button is invalid.");
                    return;
                }

                button.onClick?.AddListener(OnButtonClick);

                if (highlightOn == null || highlightOff == null)
                {
                    Debug.LogError($"[ToggleButton::Initialize] Highlights are invalid.");
                    return;
                }

                initialized = true;
                SetState(state);
            }
        }

        void Onable()
        {
            if (button)
            {
                button.onClick?.AddListener(OnButtonClick);
            }
        }

        void Osable()
        {
            if (button)
            {
                button.onClick?.RemoveListener(OnButtonClick);
            }
        }

        private void OnButtonClick()
        {
            SetState(!state);
        }

        public void SetState(bool state)
        {
            if (!initialized)
            {
                Initialize(state);
                return;
            }

            this.state = state;
            OnToggled?.Invoke(state);

            if (highlightOn == null || highlightOff == null)
            {
                Debug.LogError($"[ToggleButton::SetState] Highlights are invalid.");
                return;
            }

            highlightOn.SetHighlight(state);
            highlightOff.SetHighlight(!state);
        }
    }
}
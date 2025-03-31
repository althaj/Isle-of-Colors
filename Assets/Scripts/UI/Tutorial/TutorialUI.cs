using System;
using PSG.IsleOfColors.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PSG.IsleOfColors.UI.Tutorial
{
    public enum TutorialMessagePosition
    {
        Center
    }

    public enum TutorialMessageSize
    {
        Normal,
        MidTall,
        Tall
    }

    public enum TutorialHighlight
    {
        None,
        Full
    }

    public class TutorialUI : MonoBehaviour
    {
        private TMP_Text messageText;
        
        [SerializeField] private RectTransform canvas;
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform messageBox;
        [SerializeField] private TutorialArrow tutorialArrow;

        private TutorialStep currentTutorialStep;
        private int nextTutorialMessageId;
        
        [Header("Tutorial steps")]
        [SerializeField] private TutorialStep welcomeTutorialStep;

        public UnityEvent<TutorialStepId> OnTutorialStepEnded;

        void Start()
        {
            if(!ApplicationManager.Instance.GameOptions.ShowTutorial)
            {
                Destroy(gameObject);
                return;
            }
            
            if(messageBox != null)
            {
                messageText = messageBox.GetComponentInChildren<TMP_Text>();
            }

            ShowTutorialStep(welcomeTutorialStep);
        }

        /// <summary>
        /// Shows a tutorial step, starting with the first message.
        /// </summary>
        /// <param name="tutorialStep">Tutorial step to show.</param>
        private void ShowTutorialStep(TutorialStep tutorialStep)
        {
            currentTutorialStep = tutorialStep;
            nextTutorialMessageId = 0;
            ShowNextTutorialMessage();
        }

        /// <summary>
        /// End the current tutorial step.
        /// </summary>
        private void EndTutorialStep()
        {
            ShowBackground(TutorialHighlight.Full);
            ShowText(string.Empty, TutorialMessagePosition.Center, TutorialMessageSize.Normal);
            ShowTutorialArrow(string.Empty, TutorialArrowDirection.Up, 0);

            OnTutorialStepEnded?.Invoke(currentTutorialStep.Id);

            currentTutorialStep = null;
        }

        /// <summary>
        /// Display the next message in the current tutorial step. If there is no next message, end the tutorial step.
        /// </summary>
        public void ShowNextTutorialMessage()
        {
            if(currentTutorialStep == null)
            {
                EndTutorialStep();
                return;
            }

            if(nextTutorialMessageId < 0 || currentTutorialStep.Messages.Length < nextTutorialMessageId + 1)
            {
                EndTutorialStep();
                return;
            }

            ShowTutorialMessage(currentTutorialStep.Messages[nextTutorialMessageId]);

            nextTutorialMessageId++;
        }

        /// <summary>
        /// Show a tutorial message, together with background and a tutorial highlight.
        /// </summary>
        /// <param name="message"></param>
        private void ShowTutorialMessage(TutorialMessage message)
        {
            if(message == null)
                return;

            ShowBackground(message.Highlight);
            ShowText(message.Message, message.MessagePosition, message.MessageSize);
            ShowTutorialArrow(message.TutorialArrowTargetName, message.TutorialArrowDirection, message.TutorialArrowOffset);
        }

        /// <summary>
        /// Shows background, highlighting part of the screen.
        /// </summary>
        /// <param name="highlight">Part of the UI or game area to highlght.</param>
        private void ShowBackground(TutorialHighlight highlight)
        {
            switch(highlight)
            {
                case TutorialHighlight.Full:
                    background.gameObject.SetActive(false);
                    break;
                case TutorialHighlight.None:
                default:
                    background.gameObject.SetActive(true);
                    background.anchorMin = new Vector2(0, 0);
                    background.anchorMax = new Vector2(1, 1);
                    break;
            }
        }

        /// <summary>
        /// Shows message in a box and sets its position.
        /// </summary>
        /// <param name="message">Message to display. Leave empty to hide the message box.</param>
        /// <param name="position">Position of the message box.</param>
        private void ShowText(string message, TutorialMessagePosition position, TutorialMessageSize size)
        {
            if(messageBox == null)
            {
                return;
            }

            if(messageText == null || string.IsNullOrEmpty(message))
            {
                messageBox.gameObject.SetActive(false);
                return;
            }

            messageBox.gameObject.SetActive(true);
            messageText.text = message;

            switch(position)
            {
                case TutorialMessagePosition.Center:
                default:

                    break;
            }

            switch(size)
            {
                case TutorialMessageSize.Tall:
                    messageBox.sizeDelta = new Vector2(400, 400);
                    break;
                case TutorialMessageSize.MidTall:
                    messageBox.sizeDelta = new Vector2(400, 300);
                    break;
                case TutorialMessageSize.Normal:
                default:
                    messageBox.sizeDelta = new Vector2(400, 200);
                    break;
            }
        }

        /// <summary>
        /// Show tutorial arrow highlighting a rect transform in the UI.
        /// </summary>
        /// <param name="targetTransformName">Name of the rect transform to highlight. Leave empty to hide the arrow.</param>
        /// <param name="direction">Direction of the arrow.</param>
        /// <param name="offset">Offset of the arrow from the highlighted rect transform, in pivot, so 1 is height of the arrow.</param>
        private void ShowTutorialArrow(string targetTransformName, TutorialArrowDirection direction, float offset)
        {
            if(tutorialArrow == null)
            {
                return;
            }

            if(string.IsNullOrEmpty(targetTransformName))
            {
                tutorialArrow.Hide();
                return;
            }

            Transform targetTransform = canvas.Find(targetTransformName);
            if(targetTransform == null)
            {
                tutorialArrow.Hide();
                return;
            }

            RectTransform targetRectTransform = targetTransform.GetComponent<RectTransform>();

            tutorialArrow.Show(targetRectTransform, direction, offset);
        }
    }
}

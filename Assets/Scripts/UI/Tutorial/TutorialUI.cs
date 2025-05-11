using System.Collections.Generic;
using System.Linq;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using PSG.IsleOfColors.Gameplay.StateMachine.States;

namespace PSG.IsleOfColors.UI.Tutorial
{
    public enum TutorialMessagePosition
    {
        Center,
        Bottom
    }

    public enum TutorialMessageSize
    {
        Normal,
        Small,
        MidTall,
        Tall
    }

    public enum TutorialHighlight
    {
        None,
        Full,
        TopPanel,
        OtherPlayerPanel,
        ColorsPanel,
        ConfirmUndoButtons,
        RightArrow
    }

    public class TutorialUI : MonoBehaviour
    {
        private TMP_Text messageText;
        
        [SerializeField] private RectTransform canvas;
        [SerializeField] private RectTransform messageBox;
        [SerializeField] private TutorialArrow tutorialArrow;

        [Header("Backgrounds")]
        [SerializeField] private RectTransform clickBlocker;
        [SerializeField] private RectTransform fullBackground;
        [SerializeField] private RectTransform topBackground;
        [SerializeField] private RectTransform otherPlayerBackground;
        [SerializeField] private RectTransform colorsPanelBackground;
        [SerializeField] private RectTransform confirmUndoBackground;
        [SerializeField] private RectTransform rightArrowBackground;
        private RectTransform[] backgroundPanels;
        
        private TutorialStep currentTutorialStep;
        private int nextTutorialMessageId;

        private GameManager gameManager;

        
        [Header("Tutorial steps")]
        [SerializeField] private List<TutorialStep> tutorialSteps;

        public UnityEvent<TutorialStepId> OnTutorialStepEnded;

        void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();

            if(gameManager == null || gameManager.Player1 == null || gameManager.Player2 == null)
            {
                Debug.LogError("Game Manager, Player 1 or Player 2 are invalid.");
                Destroy(gameObject);
                return;
            }

            if(!ApplicationManager.Instance.GameOptions.ShowTutorial)
            {
                Destroy(gameObject);
                return;
            }
            
            if(messageBox != null)
            {
                messageText = messageBox.GetComponentInChildren<TMP_Text>();
            }

            SetupScoringPanel setupScoringPanel = FindFirstObjectByType<SetupScoringPanel>();
            if(setupScoringPanel != null)
            {
                setupScoringPanel.OnSetupScoringPanelClosed?.AddListener(OnSetupScoringPanelClosed);
            }

            backgroundPanels = new RectTransform[]{
                fullBackground,
                topBackground,
                otherPlayerBackground,
                colorsPanelBackground,
                confirmUndoBackground,
                rightArrowBackground
            };

            HideBackgrounds();

            ShowTutorialStep(TutorialStepId.Welcome);

            gameManager.Player1.OnPlayerStateChanged.AddListener(OnPlayerStateChanged);
            gameManager.Player2.OnPlayerStateChanged.AddListener(OnPlayerStateChanged);
        }

        /// <summary>
        /// Shows a tutorial step, starting with the first message.
        /// </summary>
        /// <param name="tutorialStepId">Id of tutorial to show.</param>
        private void ShowTutorialStep(TutorialStepId tutorialStepId)
        {
            if(tutorialSteps == null)
            {
                Debug.LogError("Tutorial steps have not been initialized.");
                return;
            }

            TutorialStep tutorialStep = tutorialSteps.FirstOrDefault(x => x.Id == tutorialStepId);
            if(tutorialStep == null)
            {
                Debug.LogErrorFormat("Could not find tutorial with ID {0}.", tutorialStepId);
                return;
            }

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
            HideBackgrounds();

            if(clickBlocker == null || clickBlocker.gameObject == null)
            {
                Debug.LogError("Click blocker has not been assigned.");
            }
            else
            {
                clickBlocker.gameObject.SetActive(true);
            }

            switch (highlight)
            {
                case TutorialHighlight.Full:
                    if(clickBlocker != null && clickBlocker.gameObject != null)
                    {
                        clickBlocker.gameObject.SetActive(false);
                    }
                    break;
                case TutorialHighlight.TopPanel:
                    EnableBackground(topBackground, "Top panel");
                    break;
                case TutorialHighlight.OtherPlayerPanel:
                    EnableBackground(otherPlayerBackground, "Other player panel");
                    break;
                case TutorialHighlight.ColorsPanel:
                    EnableBackground(colorsPanelBackground, "Colors panel");
                    break;
                case TutorialHighlight.ConfirmUndoButtons:
                    EnableBackground(confirmUndoBackground, "Confirm / Undo");
                    break;
                case TutorialHighlight.RightArrow:
                    EnableBackground(rightArrowBackground, "RIght arrow");
                    break;
                case TutorialHighlight.None:
                default:
                    EnableBackground(fullBackground, "Full");
                    break;
            }
        }

        /// <summary>
        /// Hides all tutorial backgrounds.
        /// </summary>
        private void HideBackgrounds()
        {
            if (backgroundPanels != null)
            {
                for (int i = 0; i < backgroundPanels.Length; i++)
                {
                    if (backgroundPanels[i] == null || backgroundPanels[i].gameObject == null)
                    {
                        continue;
                    }

                    backgroundPanels[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Enables a background.
        /// </summary>
        /// <param name="background">RectTransform to enable (its gameObject will be set to active).</param>
        /// <param name="title">Title of the background, used in debug error in case it cannot be enabled.</param>
        private void EnableBackground(RectTransform background, string title)
        {
            if (background == null || background.gameObject == null)
            {
                Debug.LogErrorFormat("Missing {0} tutorial background.", title);
                return;
            }

            background.gameObject.SetActive(true);
            return;
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
                case TutorialMessagePosition.Bottom:
                    messageBox.anchorMin = new Vector2(0.5f, 0);
                    messageBox.anchorMax = new Vector2(0.5f, 0);
                    messageBox.pivot = new Vector2(0.5f, 0);
                    messageBox.anchoredPosition = new Vector2(0, 50.0f);
                break;
                case TutorialMessagePosition.Center:
                default:
                    messageBox.anchorMin = new Vector2(0.5f, 0.5f);
                    messageBox.anchorMax = new Vector2(0.5f, 0.5f);
                    messageBox.pivot = new Vector2(0.5f, 0.5f);
                    messageBox.anchoredPosition = Vector2.zero;
                    break;
            }

            switch(size)
            {
                case TutorialMessageSize.Small:
                    messageBox.sizeDelta = new Vector2(400, 150);
                    break;
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

        private void OnSetupScoringPanelClosed()
        {
            ShowTutorialStep(TutorialStepId.UI);
        }

        private void OnPlayerStateChanged()
        {
            if(gameManager == null)
            {
                Debug.LogError("Game manager is invalid.");
                return;
            }

            if(gameManager.Player1 == null || gameManager.Player2 == null)
            {
                Debug.LogError("Player 1 or Player 2 is invalid.");
                return;
            }

            if(gameManager.Player1.PlayerState == EPlayerState.Finished && gameManager.Player2.PlayerState == EPlayerState.Finished)
            {
                ShowTutorialStep(TutorialStepId.EndGame);
                gameManager.Player1.OnPlayerStateChanged.RemoveListener(OnPlayerStateChanged);
                gameManager.Player2.OnPlayerStateChanged.RemoveListener(OnPlayerStateChanged);
            }
        }
    }
}

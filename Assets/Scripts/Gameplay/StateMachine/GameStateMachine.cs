using PSG.IsleOfColors.Gameplay.StateMachine.States;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PSG.IsleOfColors.Gameplay.StateMachine
{
    public class GameStateMachine : MonoBehaviour
    {
        private IState currentState;

        public UnityEvent<IState> OnStateChanged;
        public UnityEvent<string> OnStateDescriptionChanged;

        private GameManager gameManager;

        private void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();

            NextState();
        }

        private void Update()
        {
            if (currentState == null)
                return;

            if (currentState.IsDone())
            {
                NextState();
            }

            currentState.Execute();
        }

        private void NextState()
        {
            currentState?.Exit();

            if (currentState == null)
            {
                currentState = new SetupState(gameManager.Player1, gameManager.Player2, gameManager.Colors);
            }
            else
            {
                switch (currentState)
                {
                    case SetupState: currentState = NewRound(); break;
                    case RoundState:
                            currentState = gameManager.IsGameFinished() ? new EndGameState() : NewRound();
                        break;
                    default: throw new ArgumentException($"NextState: Cannot exit from state {currentState.GetType().Name}.");
                }
            }

            OnStateChanged?.Invoke(currentState);
            OnDescriptionChanged();
        }

        private IState NewRound()
        {
            var state = new RoundState(gameManager);
            state.OnDescriptionChanged.AddListener(OnDescriptionChanged);
            return state;
        }

        private void OnDescriptionChanged()
        {
            OnStateDescriptionChanged?.Invoke(currentState.GetDescription());
        }
    }
}

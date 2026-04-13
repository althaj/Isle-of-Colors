using PSG.IsleOfColors.Gameplay.StateMachine.States;
using PSG.IsleOfColors.Managers;
using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace PSG.IsleOfColors.Gameplay.StateMachine
{
    public class GameStateMachine : MonoBehaviour
    {
        private IState currentState;

        public UnityEvent<IState> OnStateChanged;
        public UnityEvent<string> OnStateDescriptionChanged;

        [Inject] private GameManager _gameManager;
        [Inject] private ApplicationManager _applicationManager;

        private void Start()
        {
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
                currentState = new SetupState(_gameManager, _applicationManager);
            }
            else
            {
                switch (currentState)
                {
                    case SetupState: currentState = NewRound(); break;
                    case RoundState:
                        currentState = _gameManager.IsGameFinished() ? new EndGameState() : NewRound();
                        break;
                    default: throw new ArgumentException($"NextState: Cannot exit from state {currentState.GetType().Name}.");
                }
            }

            OnStateChanged?.Invoke(currentState);
            OnDescriptionChanged();
        }

        private IState NewRound()
        {
            var state = new RoundState(_gameManager);
            state.OnDescriptionChanged.AddListener(OnDescriptionChanged);
            return state;
        }

        private void OnDescriptionChanged()
        {
            OnStateDescriptionChanged?.Invoke(currentState.GetDescription());
        }

        public void Reset()
        {
            currentState = null;
            NextState();
        }
    }
}

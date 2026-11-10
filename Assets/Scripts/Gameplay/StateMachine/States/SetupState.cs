using System;
using System.Collections.Generic;
using System.Linq;
using PSG.IsleOfColors.Gameplay.AI;
using PSG.IsleOfColors.Managers;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay.StateMachine.States
{
    public class SetupState : IState
    {
        private bool isDone = false;

        private GameManager _gameManager;

        private ApplicationManager _applicationManager;

        public SetupState(GameManager gameManager, ApplicationManager applicationManager)
        {
            _gameManager = gameManager;
            _applicationManager = applicationManager;

            if (_applicationManager.GameOptions.ShowTutorial)
            {
                gameManager.OnTutorialStepEnded.AddListener(OnTutorialStepEnded);
            }
            else
            {
                SetupGame();
            }
        }

        private void OnTutorialStepEnded(TutorialStepId Id)
        {
            if (Id == TutorialStepId.Welcome)
            {
                _gameManager.OnTutorialStepEnded.RemoveListener(OnTutorialStepEnded);
                SetupGame();
            }
        }

        private void SetupGame()
        {
            if (_gameManager.ColorTypes.Count != 4)
            {
                Debug.LogError($"[SetupState::SetupGame] Incorrect color types, need 4, found {_gameManager.ColorTypes.Count}");
                return;
            }

            int playerCount = _gameManager.Players.Count();
            List<PencilColor> colors = ShuffleColors(_gameManager.ColorTypes);
            if (playerCount > 2)
            {
                colors.AddRange(ShuffleColors(_gameManager.ColorTypes));
            }

            if (playerCount == 3)
            {
                colors.RemoveRange(6, colors.Count - 6);
            }

            colors = ShuffleColors(colors);

            for (int i = 0; i < _applicationManager.GameOptions.Players.Count; i++)
            {
                GameOptions.PlayerOptions playerOptions = _applicationManager.GameOptions.Players[i];
                if (playerOptions.PlayerType != GameOptions.PlayerType.Human && playerOptions.PlayerType != GameOptions.PlayerType.MainMenu)
                {
                    _gameManager.Players[i].SetBot(new SimpleAI(_applicationManager, _gameManager, playerOptions.PlayerType));
                }

                _gameManager.Players[i].AddColor(colors[2*i]);
                _gameManager.Players[i].AddColor(colors[2*i + 1]);
            }

            isDone = true;
        }

        private List<PencilColor> ShuffleColors(IEnumerable<PencilColor> colors)
        {
            List<PencilColor> shuffledColors = colors.ToList();
            for (int i = shuffledColors.Count - 1; i > 0; i--)
            {
                int j = RNGManager.RNGManager.Manager["Game"].NextInt(i + 1);
                PencilColor color = shuffledColors[i];
                shuffledColors[i] = shuffledColors[j];
                shuffledColors[j] = color;
            }

            return shuffledColors;
        }

        public void Execute()
        {

        }

        public void Exit()
        {
        }

        public string GetDescription() => "Setting up the game.";

        public bool IsDone() => isDone;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using PSG.IsleOfColors.Gameplay.AI;
using PSG.IsleOfColors.Managers;

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

            }
            List<PencilColor> colors = new();
            for (int i = 0; i < _gameManager.Players.Count() / 2; i++)
            {
                colors.AddRange(_gameManager.ColorTypes);
            }

            // TODO implement shuffle to RNGManager
            colors = colors.OrderBy(x => RNGManager.RNGManager.Manager["Game"].NextInt(100)).ToList();

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

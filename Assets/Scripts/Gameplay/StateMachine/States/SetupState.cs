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
            _gameManager.SetupScoring();

            Player player1 = _gameManager.Player1;
            Player player2 = _gameManager.Player2;
            List<PencilColor> colors = _gameManager.Colors;

            player1.Initialize();
            player2.Initialize();

            if (_applicationManager.GameOptions.IsSinglePlayer)
            {
                player2.SetBot(new SimpleAI(_applicationManager));
            }

            if (colors.Count != 4)
                throw new ArgumentException($"SetupState: Incorrect number of colors. Expecting 4, got {colors.Count}.");

            // TODO implement shuffle to RNGManager
            colors = colors.OrderBy(x => RNGManager.RNGManager.Manager["Game"].NextInt(100)).ToList();
            player1.AddColor(colors[0]);
            player1.AddColor(colors[1]);
            player2.AddColor(colors[2]);
            player2.AddColor(colors[3]);
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

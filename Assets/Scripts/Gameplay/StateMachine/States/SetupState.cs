using System;
using System.Collections.Generic;
using System.Linq;
using PSG.IsleOfColors.Gameplay.AI;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay.StateMachine.States
{
    public class SetupState : IState
    {
        private bool isDone = false;

        public SetupState(GameManager gameManager)
        {
            gameManager.SetupScoring();

            Player player1 = gameManager.Player1;
            Player player2 = gameManager.Player2;
            List<PencilColor> colors = gameManager.Colors;

            player1.Initialize();
            player2.Initialize();

            if(ApplicationManager.Instance.GameOptions.IsSinglePlayer)
            {
                player2.SetBot(new SimpleAI());
            }

            if (colors.Count != 4)
                throw new ArgumentException($"SetupState: Incorrect number of colors. Expecting 4, got {colors.Count}.");

            // TODO naimplementovať shuffle do RNGManager
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
            Debug.Log("Finished setting up the game.");
        }

        public string GetDescription() => "Setting up the game.";

        public bool IsDone() => isDone;
    }
}

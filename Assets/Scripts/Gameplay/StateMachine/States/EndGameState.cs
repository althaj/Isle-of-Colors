using UnityEngine;

namespace PSG.IsleOfColors.Gameplay.StateMachine.States
{
    public class EndGameState : IState
    {
        public void Execute()
        {
            
        }

        public void Exit()
        {
            
        }

        public string GetDescription()
        {
            return "End of the game triggered.";
        }

        public bool IsDone()
        {
            return false;
        }
    }
}

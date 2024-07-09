using UnityEngine;

namespace PSG.IsleOfColors.Gameplay.StateMachine.States
{
	public interface IState
	{
		public string GetDescription();

		public void Execute();
		public void Exit();

		public bool IsDone();
	}
}
using UnityEngine.Events;

namespace PSG.IsleOfColors.Gameplay
{
	public class PlayerSheetSpace
	{
		public PencilColor Color { get; private set; }
		public int MoveIndex { get; private set; } = 0;
		public bool IsNew { get; private set; } = false;

		public UnityEvent<PencilColor> OnColorChanged;

		public PlayerSheetSpace()
		{
		}

		public void SetColor(PencilColor color, int moveIndex)
		{
			Color = color;
			MoveIndex = moveIndex;
			IsNew = true;

			OnColorChanged.Invoke(color);
		}

		public void Confirm()
		{
			IsNew = false;
		}
	}
}

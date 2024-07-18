using UnityEngine;
using UnityEngine.Events;

namespace PSG.IsleOfColors.Gameplay
{
	public class PlayerSheetSpace
	{
		public PencilColor Color { get; private set; }
		public int MoveIndex { get; private set; } = -1;
		public bool IsNew { get; private set; } = false;

		public int X { get; private set; }
		public int Y { get; private set; }

		public bool IsEnabled
		{
			get => isEnabled; set
			{
				isEnabled = value;
				OnEnabledChanged?.Invoke(value);
			}
		}
		private bool isEnabled = false;


		public UnityEvent<PencilColor> OnColorChanged;
		public UnityEvent<bool> OnEnabledChanged;

		public PlayerSheetSpace(int x, int y)
		{
			OnColorChanged = new UnityEvent<PencilColor>();
			OnEnabledChanged = new UnityEvent<bool>();
			X = x;
			Y = y;
		}

		public void SetColor(PencilColor color, int moveIndex)
		{
			Color = color;
			MoveIndex = moveIndex;
			IsNew = true;

			OnColorChanged?.Invoke(color);
		}

		public void Confirm()
		{
			IsNew = false;
			MoveIndex = -1;
		}

		public void Undo()
		{
			SetColor(null, -1);
			IsNew = false;
		}
	}
}

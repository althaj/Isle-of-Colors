using UnityEngine;
using UnityEngine.Events;

namespace PSG.IsleOfColors.Gameplay
{
	public class PlayerSheetSpace
	{
		public PencilColor Color { get; set; }
		public int MoveIndex { get; set; } = -1;
		public bool IsNew { get; set; } = false;

		public int X { get; private set; }
		public int Y { get; private set; }

		// https://www.redblobgames.com/grids/hexagons/#conversions-offset
		public int Q
		{
			get
			{
				return X - (Y - (Y & 1)) / 2;
			}
		}

		public bool IsEnabled
		{
			get => isEnabled; set
			{
				if(isEnabled != value)
				{
					isEnabled = value;
					OnEnabledChanged?.Invoke(value);
				}
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

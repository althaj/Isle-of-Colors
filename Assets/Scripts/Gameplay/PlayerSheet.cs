using System.Linq;
using System.Numerics;
using UnityEngine.Events;

namespace PSG.IsleOfColors.Gameplay
{
	public class PlayerSheet
	{
		public PlayerSheetSpace[][] Spaces { get; private set; }

		private int currentMoveIndex = 0;

		public UnityEvent OnMapGenerated;

		public PlayerSheet()
		{
			OnMapGenerated = new UnityEvent();
		}

		public void GenerateMap(Map map)
		{
			Spaces = new PlayerSheetSpace[map.rows.Count][];
			for (int x = 0; x < Spaces.Length; x++)
			{
				Spaces[x] = new PlayerSheetSpace[map.rows.Max(x => x.Length)];

				for(int y = 0; y < Spaces[x].Length; y++)
				{
					if (map.rows[x].Length <= y || map.rows[x][y] == 'x')
						Spaces[x][y] = null;
					else
						Spaces[x][y] = new PlayerSheetSpace();
                }
			}

			OnMapGenerated?.Invoke();
		}

		public void SetColor(PencilColor color, Vector2 coordinates)
		{
			Spaces[(int)coordinates.X][(int)coordinates.Y].SetColor(color, currentMoveIndex++);
		}

		public void Confirm()
		{
			foreach(var spaceX in Spaces)
			{
				foreach(var space in spaceX)
				{
					if (space.IsNew)
						space.Confirm();
				}
			}

			currentMoveIndex = 0;
		}
	}
}

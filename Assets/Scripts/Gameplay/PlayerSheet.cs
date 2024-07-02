namespace PSG.IsleOfColors.Gameplay
{
	public class PlayerSheet
	{
		public PlayerSheetSpace[][] Spaces { get; private set; }

		public PlayerSheet()
		{
			Spaces = new PlayerSheetSpace[11][];
			for (int i = 0; i < Spaces.Length; i++)
			{
				Spaces[i] = new PlayerSheetSpace[9];
			}
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay
{
	[CreateAssetMenu(fileName = "PencilColor", menuName = "Isle of Colors/Pencil Color")]
	public class PencilColor : ScriptableObject
	{
		public string Name;
		public Color Color;

		public List<Sprite> MainSprites = new();
		public List<Sprite> PropSprites = new();
	}
}

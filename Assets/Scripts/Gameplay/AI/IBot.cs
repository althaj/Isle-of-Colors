using UnityEngine;

namespace PSG.IsleOfColors.Gameplay.AI
{
    public interface IBot
    {
        public bool DoTurn(Player player);
    }
}

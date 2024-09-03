using UnityEngine;

namespace PSG.IsleOfColors.Gameplay.AI
{
    public interface IBot
    {
        public void DoTurn(Player player);
    }
}

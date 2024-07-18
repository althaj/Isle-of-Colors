using RNGManager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PSG.IsleOfColors.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player player1;
        [SerializeField] private Player player2;
        [SerializeField] private List<PencilColor> colors;

        public Player Player1 { get => player1; }
        public Player Player2 { get => player2; }
        public List<PencilColor> Colors { get => colors; set => colors = value; }

        public UnityEvent<int> OnDieRolled;

        private PencilColor player1Color;
        private PencilColor player2Color;

        private bool lastRound = false;

        private void Awake()
        {
            RNGManager.RNGManager.Manager.AddInstance(new RNGInstance(title: "Game"));
        }

        public bool IsGameFinished()
        {
            if(lastRound)
                return true;

            if(Player1.ColorUsage.Any(x => x.Value >= 6) || Player2.ColorUsage.Any(x => x.Value >= 6))
                lastRound = true;

            return false;
        }

        public void UseColor(PencilColor color)
        {
            if (Player1.Colors.Contains(color))
            {
                Player1.UseColor(color);
                player1Color = color;
            }

            if (Player2.Colors.Contains(color))
            {
                Player2.UseColor(color);
                player2Color = color;
            }

            SwapColors();
        }

        public void SwapColors()
        {
            if (player1Color == null || player2Color == null)
                return;

            Player1.AddColor(player2Color);
            Player2.AddColor(player1Color);
            player1Color = player2Color = null;
        }

        public int RollDie()
        {
            int result = RNGManager.RNGManager.Manager["Game"].NextInt(1, 6);
            OnDieRolled?.Invoke(result);
            return result;
        }
    }
}

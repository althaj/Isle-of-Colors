using RNGManager;
using System.Collections.Generic;
using UnityEngine;

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

        private PencilColor player1Color;
        private PencilColor player2Color;

        private void Awake()
        {
            RNGManager.RNGManager.Manager.AddInstance(new RNGInstance(title: "Game"));
        }

        public bool IsGameFinished()
        {
            return false;
        }

        public void UseColor(PencilColor color)
        {
            if (Player1.Colors.Contains(color))
            {
                Player1.UseColor(color);
                player1Color = color;
            }
            else
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
    }
}
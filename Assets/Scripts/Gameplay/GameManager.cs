using PSG.IsleOfColors.Gameplay.Scoring;
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
        private bool noMoves = false;

        public IScoring GreenScoring { get; private set; }
        public IScoring BlueScoring { get; private set; }
        public IScoring BrownScoring { get; private set; }
        public IScoring RedScoring { get; private set; }

        private void Awake()
        {
            RNGManager.RNGManager.Manager.AddInstance(new RNGInstance(title: "Game"));
        }

        public PencilColor GetColorByName(string name) => Colors.Single(x => x.Name.CompareTo(name) == 0);

        public void SetupScoring()
        {
            GreenScoring = new SwampScoring(GetColorByName("Green"));
            BlueScoring = new FjordsScoring(GetColorByName("Blue"));
            BrownScoring = new DesertScoring(GetColorByName("Brown"), GetColorByName("Blue"));
            RedScoring = new HamletScoring(GetColorByName("Red"));

            Debug.Log($"Green - {GreenScoring.GetName()}: {GreenScoring.GetDescription()}");
            Debug.Log($"Blue - {BlueScoring.GetName()}: {BlueScoring.GetDescription()}");
            Debug.Log($"Brown - {BrownScoring.GetName()}: {BrownScoring.GetDescription()}");
            Debug.Log($"Red - {RedScoring.GetName()}: {RedScoring.GetDescription()}");
        }

        public void NoMoves()
        {
            noMoves = true;
        }

        public bool IsGameFinished()
        {
            if(lastRound)
                return true;

            if(Player1.ColorUsage.Any(x => x.Value >= 6) || Player2.ColorUsage.Any(x => x.Value >= 6) || noMoves)
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
            int result = RNGManager.RNGManager.Manager["Game"].NextInt(1, 7);
            OnDieRolled?.Invoke(result);
            return result;
        }
    }
}

using PSG.IsleOfColors.Gameplay.Scoring;
using PSG.IsleOfColors.Managers;
using PSG.IsleOfColors.UI.Tutorial;
using RNGManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public UnityEvent<Player, Player> OnCurrentPlayerChanged;
        public UnityEvent OnScoringSetupFinished;
        public UnityEvent OnLastRoundStarted;
        public UnityEvent OnGameEnded;

        public UnityEvent<TutorialStepId> OnTutorialStepEnded;

        private PencilColor player1Color;
        private PencilColor player2Color;

        private bool lastRound = false;
        private bool noMoves = false;

        public IScoring GreenScoring { get; private set; }
        public IScoring BlueScoring { get; private set; }
        public IScoring BrownScoring { get; private set; }
        public IScoring RedScoring { get; private set; }

        private Player currentPlayer;
        private Player otherPlayer;

        public int CurrentDieRoll { get; private set; }

        public float GameDuration { get => gameDurationStopwatch != null ? (float)gameDurationStopwatch.Elapsed.TotalSeconds : 0; }
        private Stopwatch gameDurationStopwatch;

        private void Awake()
        {
            if (!String.IsNullOrWhiteSpace(ApplicationManager.Instance.GameOptions.Player1Name))
                player1.Name = ApplicationManager.Instance.GameOptions.Player1Name;

            if (!String.IsNullOrWhiteSpace(ApplicationManager.Instance.GameOptions.Player2Name))
                player2.Name = ApplicationManager.Instance.GameOptions.Player2Name;

            RNGManager.RNGManager.Manager.AddInstance(new RNGInstance(title: "Game"));
            SetCurrentPlayer(player1);

            gameDurationStopwatch = new Stopwatch();
            gameDurationStopwatch.Start();
        }

        private void Start()
        {
            TutorialUI tutorialUI = FindFirstObjectByType<TutorialUI>();
            if(tutorialUI != null)
                tutorialUI.OnTutorialStepEnded.AddListener(ReceiveOnTutorialStepEnded);
        }

        public PencilColor GetColorByName(string name) => Colors.Single(x => x.Name.CompareTo(name) == 0);

        public void SetupScoring()
        {
            // Green scoring
            switch (RNGManager.RNGManager.Manager["Game"].NextInt(1, 4))
            {
                case 1:
                    GreenScoring = new GrassScoring(GetColorByName("Green"));
                    break;
                case 2:
                    GreenScoring = new ForestScoring(GetColorByName("Green"), GetColorByName("Blue"));
                    break;

                default:
                    GreenScoring = new SwampScoring(GetColorByName("Green"));
                    break;
            }

            // Blue scoring
            switch (RNGManager.RNGManager.Manager["Game"].NextInt(1, 4))
            {
                case 1:
                    BlueScoring = new ShoreScoring(GetColorByName("Blue"));
                    break;
                case 2:
                    BlueScoring = new RiverScoring(GetColorByName("Blue"));
                    break;

                default:
                    BlueScoring = new FjordsScoring(GetColorByName("Blue"));
                    break;
            }

            // Brown scoring
            switch (RNGManager.RNGManager.Manager["Game"].NextInt(1, 4))
            {
                case 1:
                    BrownScoring = new BeachScoring(GetColorByName("Brown"), GetColorByName("Green"), GetColorByName("Blue"));
                    break;
                case 2:
                    BrownScoring = new DesertScoring(GetColorByName("Brown"), GetColorByName("Blue"));
                    break;

                default:
                    BrownScoring = new CavesScoring(GetColorByName("Brown"));
                    break;
            }

            // Red scoring
            switch (RNGManager.RNGManager.Manager["Game"].NextInt(1, 4))
            {
                case 1:
                    RedScoring = new HamletScoring(GetColorByName("Red"));
                    break;
                case 2:
                    RedScoring = new VillageScoring(GetColorByName("Red"), GetColorByName("Green"), GetColorByName("Blue"), GetColorByName("Brown"));
                    break;

                default:
                    RedScoring = new TownScoring(GetColorByName("Red"));
                    break;
            }

            OnScoringSetupFinished?.Invoke();
        }

        public IScoring GetScoring(PencilColor color)
        {
            if (GreenScoring != null && GreenScoring.GetColor() == color)
                return GreenScoring;

            if (BlueScoring != null && BlueScoring.GetColor() == color)
                return BlueScoring;

            if (BrownScoring != null && BrownScoring.GetColor() == color)
                return BrownScoring;

            if (RedScoring != null && RedScoring.GetColor() == color)
                return RedScoring;

            return null;
        }

        public void NoMoves()
        {
            noMoves = true;
        }

        public bool IsGameFinished()
        {
            if (lastRound)
            {
                gameDurationStopwatch.Stop();

                OnGameEnded?.Invoke();
                return true;
            }

            if (Player1.ColorUsage.Any(x => x.Value >= 6) || Player2.ColorUsage.Any(x => x.Value >= 6) || noMoves)
            {
                OnLastRoundStarted?.Invoke();
                lastRound = true;
            }

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

        public void RollDie(int value)
        {
            CurrentDieRoll = value;
            OnDieRolled?.Invoke(CurrentDieRoll);
        }

        private void SetCurrentPlayer(Player player)
        {
            currentPlayer = player;
            if (player == player1)
                otherPlayer = player2;
            else
                otherPlayer = player1;

            OnCurrentPlayerChanged?.Invoke(currentPlayer, otherPlayer);
        }

        public void ChangeCurrentPlayer()
        {
            if (currentPlayer == player1)
                SetCurrentPlayer(player2);
            else
                SetCurrentPlayer(player1);
        }

        public void Confirm()
        {
            currentPlayer.Confirm();
        }

        public void Undo()
        {
            currentPlayer.Undo();
        }

        public void Reset()
        {
            lastRound = false;
            noMoves = false;
            player1.Reset();
            player2.Reset();
            SetupScoring();
        }

        private void ReceiveOnTutorialStepEnded(TutorialStepId Id)
        {
            OnTutorialStepEnded?.Invoke(Id);
        }
    }
}

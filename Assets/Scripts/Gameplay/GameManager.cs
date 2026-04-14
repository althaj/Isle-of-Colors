using PSG.IsleOfColors.Gameplay.Scoring;
using PSG.IsleOfColors.Gameplay.StateMachine;
using PSG.IsleOfColors.UI.Tutorial;
using RNGManager;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace PSG.IsleOfColors.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<PencilColor> colors;

        public bool IsGameInitialized { get; private set; } = false;

        private Player[] players;

        public Player Player1 { get => players != null && players.Length > 0 ? players[0] : null; }
        public Player Player2 { get => players != null && players.Length > 1 ? players[1] : null; }
        public List<PencilColor> Colors { get => colors; set => colors = value; }

        public UnityEvent<int> OnDieRolled;
        public UnityEvent<Player, Player> OnCurrentPlayerChanged;
        public UnityEvent OnLastRoundStarted;
        public UnityEvent OnGameEnded;
        public UnityEvent OnGameInitialized;

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

        [Inject] private GameFactory _gameFactory;
        [Inject] private GameStateMachine _stateMachine;

        private void Start()
        {
            players = _gameFactory.InitializePlayers(2).ToArray();

            RNGManager.RNGManager.Manager.AddInstance(new RNGInstance(title: "Game"));
            SetCurrentPlayer(Player1);

            SetupScoring();

            _stateMachine.StartStateMachine();

            gameDurationStopwatch = new Stopwatch();
            gameDurationStopwatch.Start();

            TutorialUI tutorialUI = FindFirstObjectByType<TutorialUI>();
            if (tutorialUI != null)
            {
                tutorialUI.OnTutorialStepEnded.AddListener(ReceiveOnTutorialStepEnded);
            }

            IsGameInitialized = true;
            OnGameInitialized?.Invoke();
        }

        public void InvokeAfterInitialization(UnityAction action)
        {
            if (action == null)
                return;

            if (IsGameInitialized)
            {
                action.Invoke();
            }
            else
            {
                OnGameInitialized.AddListener(action);
            }
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
            if (player == Player1)
                otherPlayer = Player2;
            else
                otherPlayer = Player1;

            OnCurrentPlayerChanged?.Invoke(currentPlayer, otherPlayer);
        }

        public void ChangeCurrentPlayer()
        {
            if (currentPlayer == Player1)
                SetCurrentPlayer(Player2);
            else
                SetCurrentPlayer(Player1);
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
            Player1.Reset();
            Player2.Reset();
            SetupScoring();
        }

        private void ReceiveOnTutorialStepEnded(TutorialStepId Id)
        {
            OnTutorialStepEnded?.Invoke(Id);
        }
    }
}

using PSG.IsleOfColors.Gameplay.Scoring;
using PSG.IsleOfColors.Gameplay.StateMachine;
using PSG.IsleOfColors.Managers;
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


        public Player[] Players { get; private set; }
        private PencilColor[] UsedColors { get; set; }

        public List<PencilColor> ColorTypes { get => colors; set => colors = value; }

        public UnityEvent<int> OnDieRolled;
        public UnityEvent<Player> OnCurrentPlayerChanged;
        public UnityEvent OnLastRoundStarted;
        public UnityEvent OnGameEnded;
        public UnityEvent OnGameInitialized;

        public UnityEvent<TutorialStepId> OnTutorialStepEnded;

        private bool lastRound = false;
        private bool noMoves = false;

        public IScoring GreenScoring { get; private set; }
        public IScoring BlueScoring { get; private set; }
        public IScoring BrownScoring { get; private set; }
        public IScoring RedScoring { get; private set; }

        private Player currentPlayer;

        public int CurrentDieRoll { get; private set; }

        public float GameDuration { get => gameDurationStopwatch != null ? (float)gameDurationStopwatch.Elapsed.TotalSeconds : 0; }
        private Stopwatch gameDurationStopwatch;


        [Inject] private GameFactory _gameFactory;
        [Inject] private GameStateMachine _stateMachine;

        private void Start()
        {
            Players = _gameFactory.InitializePlayers().ToArray();
            UsedColors = new PencilColor[Players.Length];

            RNGManager.RNGManager.Manager.AddInstance(new RNGInstance(title: "Game"));
            SetCurrentPlayer(Players[0]);

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

        public PencilColor GetColorByName(string name) => ColorTypes.Single(x => x.Name.CompareTo(name) == 0);

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

            if (Players.Any(p => p.ColorUsage.Any(c => c.Value >= 6)) || noMoves)
            {
                OnLastRoundStarted?.Invoke();
                lastRound = true;
            }

            return false;
        }

        public void RollDie(int value)
        {
            CurrentDieRoll = value;

            RotateColors();

            OnDieRolled?.Invoke(CurrentDieRoll);
        }

        private void RotateColors()
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (UsedColors[i] == null)
                {
                    continue;
                }

                int nextIndex = (i + 1) % Players.Length;
                Players[nextIndex].AddColor(UsedColors[i]);
                UsedColors[i] = null;
            }
        }

        private void SetCurrentPlayer(Player player)
        {
            currentPlayer = player;

            OnCurrentPlayerChanged?.Invoke(currentPlayer);
        }

        public void SwitchPlayer()
        {
            if (currentPlayer == null)
            {
                UnityEngine.Debug.LogError($"[GameManager::SwitchPlayer] Current Player is invalid.");
                return;
            }

            if (Players == null || Players.Length == 0)
            {
                UnityEngine.Debug.LogError($"[GameManager::SwitchPlayer] No players available.");
                return;
            }

            int index = System.Array.IndexOf(Players, currentPlayer);
            if (index < 0)
            {
                UnityEngine.Debug.LogError($"[GameManager::SwitchPlayer] Current player not found in Players array.");
                return;
            }

            int nextIndex = (index + 1) % Players.Length;
            currentPlayer = Players[nextIndex];
            OnCurrentPlayerChanged?.Invoke(currentPlayer);
        }

        public void Confirm()
        {
            currentPlayer.Confirm();
        }

        public void UseColor(Player player, PencilColor color)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == player)
                {
                    UsedColors[i] = color;
                    return;
                }
            }
        }

        public void Undo()
        {
            currentPlayer.Undo();
        }

        public void Reset()
        {
            lastRound = false;
            noMoves = false;

            foreach (Player player in Players)
            {
                player.Reset();
            }

            SetupScoring();
        }

        private void ReceiveOnTutorialStepEnded(TutorialStepId Id)
        {
            OnTutorialStepEnded?.Invoke(Id);
        }
    }
}

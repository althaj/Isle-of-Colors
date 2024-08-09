using PSG.IsleOfColors.Gameplay.Scoring;
using PSG.IsleOfColors.Gameplay.StateMachine.States;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.VersionControl.Asset;

namespace PSG.IsleOfColors.Gameplay
{
    public class Player : MonoBehaviour
    {
        public string Name { get => playerName; set => playerName = value; }
        [SerializeField] private string playerName;
        [SerializeField] private Map map;

        public List<PencilColor> Colors { get; private set; }
        public PlayerSheet PlayerSheet { get; private set; }
        public Dictionary<PencilColor, int> ColorUsage { get; private set; }

        public UnityEvent OnPlayerColorsChanged;
        public UnityEvent OnPlayerStateChanged;
        public UnityEvent OnColorUsageChanged;
        public UnityEvent<PlayerScore> OnPlayerScoreChanged;


        private int dieValue;
        private int currentMoveIndex = 0;
        private bool isColoring = false;
        private bool turnFinished = true;
        private PencilColor coloringColor;

        private GameManager gameManager;

        public PlayerScore Score { get; private set; }

        public EPlayerState PlayerState
        {
            get
            {
                if (turnFinished)
                    return EPlayerState.Finished;

                if (isColoring)
                    return EPlayerState.Coloring;

                return EPlayerState.PickingColor;
            }
        }

        void Awake()
        {
            gameManager = FindFirstObjectByType<GameManager>();

            ColorUsage = new();
            foreach (PencilColor color in gameManager.Colors)
            {
                ColorUsage.Add(color, 0);
            }

            Score = new PlayerScore(gameManager.Colors);
        }

        internal void Initialize()
        {
            Colors = new List<PencilColor>();
            PlayerSheet = new PlayerSheet();
            PlayerSheet.GenerateMap(map);
            GetComponent<GameMap>().CreateMap();
        }

        public void UseColor(PencilColor color)
        {
            if (!Colors.Contains(color))
            {
                string message = $"UseColor: Player {Name} does not own the color {color}.";
                Debug.LogError(message, this);
                throw new System.ArgumentException(message);
            }

            Colors.Remove(color);
            ColorUsage[color]++;
            OnColorUsageChanged?.Invoke();
            OnPlayerColorsChanged?.Invoke();
        }

        public void AddColor(PencilColor color)
        {
            if (Colors.Contains(color))
            {
                string message = $"AddColor: Player {Name} already owns the color {color}.";
                Debug.LogError(message, this);
                throw new System.ArgumentException(message);
            }

            Colors.Add(color);
            OnPlayerColorsChanged?.Invoke();
        }

        public void SetColor(int x, int y)
        {
            if (!isColoring)
                return;

            if (PlayerSheet.Spaces[y][x] == null)
                return;

            PlayerSheet.Spaces[y][x].SetColor(coloringColor, currentMoveIndex++);
            PlayerSheet.UpdateAvailableMoves(isColoring, currentMoveIndex, dieValue);
        }

        public void StartColoring(PencilColor color)
        {
            if (turnFinished || isColoring)
                return;

            isColoring = true;
            coloringColor = color;
            PlayerSheet.UpdateAvailableMoves(isColoring, currentMoveIndex, dieValue);
            OnPlayerStateChanged.Invoke();
        }

        public void Undo()
        {
            if (!isColoring)
                return;

            if (currentMoveIndex > 0)
            {
                foreach (var spaceY in PlayerSheet.Spaces)
                {
                    foreach (var space in spaceY)
                    {
                        if (space != null && space.MoveIndex == currentMoveIndex - 1)
                            space.Undo();
                    }
                }

                currentMoveIndex--;
            }
            else
            {
                isColoring = false;
                coloringColor = null;
                OnPlayerStateChanged.Invoke();
            }

            PlayerSheet.UpdateAvailableMoves(isColoring, currentMoveIndex, dieValue);
        }

        public void Confirm()
        {
            if (PlayerSheet.Spaces.Sum(x => x.Count(y => y != null && y.IsNew)) != dieValue)
                return;

            foreach (var spaceY in PlayerSheet.Spaces)
            {
                foreach (var space in spaceY)
                {
                    if (space != null && space.IsNew)
                        space.Confirm();
                }
            }

            gameManager.UseColor(coloringColor);

            currentMoveIndex = 0;
            coloringColor = null;
            isColoring = false;
            turnFinished = true;

            PlayerSheet.UpdateAvailableMoves(isColoring, currentMoveIndex, dieValue);

            OnPlayerStateChanged.Invoke();

            // UPDATE SCORES
            Score.SetScore(gameManager.GreenScoring.GetColor(), gameManager.GreenScoring.GetScore(PlayerSheet));
            Score.SetScore(gameManager.BlueScoring.GetColor(), gameManager.BlueScoring.GetScore(PlayerSheet));
            Score.SetScore(gameManager.BrownScoring.GetColor(), gameManager.BrownScoring.GetScore(PlayerSheet));
            Score.SetScore(gameManager.RedScoring.GetColor(), gameManager.RedScoring.GetScore(PlayerSheet));
            OnPlayerScoreChanged?.Invoke(Score);
        }

        public void StartTurn(int dieValue)
        {
            this.dieValue = dieValue;
            turnFinished = false;
            isColoring = false;
            currentMoveIndex = 0;
            coloringColor = null;

            if (!PlayerSheet.GetAllGroups(null).Any(x => x.Count >= dieValue))
            {
                Debug.Log("No more moves.", gameObject);
                gameManager.NoMoves();
                this.dieValue = 0;
            }
        }
    }
}

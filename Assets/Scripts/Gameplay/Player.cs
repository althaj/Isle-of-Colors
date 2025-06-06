using PSG.IsleOfColors.Gameplay.AI;
using PSG.IsleOfColors.Gameplay.Scoring;
using PSG.IsleOfColors.Gameplay.StateMachine.States;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PSG.IsleOfColors.Gameplay
{
    public class Player : MonoBehaviour
    {
        public string Name { get => playerName; set => playerName = value; }
        [SerializeField] private string playerName;
        [SerializeField] private Map map;
        [SerializeField] private bool disableSound;

        public List<PencilColor> Colors { get; private set; }
        public PlayerSheet PlayerSheet { get; private set; }
        public Dictionary<PencilColor, int> ColorUsage { get; private set; }

        public UnityEvent OnPlayerColorsChanged;
        public UnityEvent OnPlayerStateChanged;
        public UnityEvent OnColorUsageChanged;
        public UnityEvent<Player> OnPlayerScoreChanged;
        public UnityEvent OnSelectedColorChanged;
        public UnityEvent<Player> OnPlayerMove;

        public bool IsSoundEnabled { get => !disableSound && ai == null; }

        public int DieValue { get; private set; }
        private int currentMoveIndex = 0;
        private bool isColoring = false;
        private bool turnFinished = false;
        private PencilColor coloringColor;

        private GameManager gameManager;

        public PlayerScore Score { get; private set; }

        private IBot ai;

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

        public bool CanConfirm {
            get {
                bool hasCompletedTurn = PlayerSheet.Spaces.Sum(x => x.Count(y => y != null && y.IsNew)) == DieValue;
                bool hasSelectedColor = coloringColor != null;
                return hasCompletedTurn && hasSelectedColor && !turnFinished;
            }
        }

        public bool CanUndo => currentMoveIndex > 0 && !turnFinished;

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

        internal void SetBot(IBot bot)
        {
            ai = bot;
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
            if (PlayerSheet.Spaces[y][x] == null)
                return;

            PlayerSheet.Spaces[y][x].SetColor(coloringColor, currentMoveIndex++);
            PlayerSheet.UpdateAvailableMoves(currentMoveIndex, DieValue);

            OnPlayerMove?.Invoke(this);
        }

        public PencilColor GetColor() => coloringColor;

        public void StartColoring(PencilColor color)
        {
            if (turnFinished)
                return;

            if(color != coloringColor)
            {
                isColoring = true;
                coloringColor = color;
                
                PlayerSheet.UpdateNewSpacesWithColor(color);
                
                OnPlayerStateChanged?.Invoke();
                OnSelectedColorChanged?.Invoke();
                OnPlayerMove?.Invoke(this);
            }
        }

        public void Undo()
        {
            if(!CanUndo)
            {
                return;
            }
            
            foreach (var spaceY in PlayerSheet.Spaces)
            {
                foreach (var space in spaceY)
                {
                    if (space != null && space.MoveIndex == currentMoveIndex - 1)
                        space.Undo();
                }
            }

            currentMoveIndex--;

            PlayerSheet.UpdateAvailableMoves(currentMoveIndex, DieValue);

            OnPlayerMove?.Invoke(this);
        }

        public void Confirm()
        {
            if(!CanConfirm)
            {
                return;
            }

            PlayerSheet.Confirm();

            gameManager.UseColor(coloringColor);

            PlayerSheet.UpdateAvailableMoves(currentMoveIndex, DieValue);

            currentMoveIndex = 0;
            coloringColor = null;
            isColoring = false;
            turnFinished = true;

            OnPlayerStateChanged?.Invoke();

            // UPDATE SCORES
            Score.SetScore(gameManager.GreenScoring.GetColor(), gameManager.GreenScoring.GetScore(PlayerSheet));
            Score.SetScore(gameManager.BlueScoring.GetColor(), gameManager.BlueScoring.GetScore(PlayerSheet));
            Score.SetScore(gameManager.BrownScoring.GetColor(), gameManager.BrownScoring.GetScore(PlayerSheet));
            Score.SetScore(gameManager.RedScoring.GetColor(), gameManager.RedScoring.GetScore(PlayerSheet));
            OnPlayerScoreChanged?.Invoke(this);
            OnPlayerMove?.Invoke(this);
        }

        public void StartTurn(int dieValue)
        {
            DieValue = dieValue;
            turnFinished = false;
            isColoring = false;
            currentMoveIndex = 0;
            coloringColor = null;

            if (!PlayerSheet.GetAllGroups(null).Any(x => x.Count >= dieValue))
            {
                Debug.Log("No more moves.", gameObject);
                gameManager.NoMoves();
                DieValue = 0;
            }

            PlayerSheet.UpdateAvailableMoves(currentMoveIndex, DieValue);

            if (ai != null)
                ai.DoTurn(this);

            OnPlayerMove?.Invoke(this);
        }

        public void Reset()
        {
            var keys = new List<PencilColor>(ColorUsage.Keys);

            foreach(var key in keys)
            {
                ColorUsage[key] = 0;
            }

            PlayerSheet.Reset();
        }
    }
}

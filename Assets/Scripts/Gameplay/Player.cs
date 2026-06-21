using PSG.IsleOfColors.Gameplay.AI;
using PSG.IsleOfColors.Gameplay.Scoring;
using PSG.IsleOfColors.Gameplay.StateMachine.States;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace PSG.IsleOfColors.Gameplay
{
    public class Player : MonoBehaviour
    {
        public string Name { get => playerName; set => playerName = value; }
        [SerializeField] private string playerName;
        [SerializeField] private Map map;
        [SerializeField] private bool disableSound;

        public List<PencilColor> Colors { get; private set; } = new();
        public PlayerSheet PlayerSheet { get; private set; } = new();
        public Dictionary<PencilColor, int> ColorUsage { get; private set; } = new();

        public UnityEvent OnPlayerColorsChanged;
        public UnityEvent OnPlayerStateChanged;
        public UnityEvent OnColorUsageChanged;
        public UnityEvent<Player> OnPlayerScoreChanged;
        public UnityEvent OnSelectedColorChanged;
        public UnityEvent<Player> OnPlayerMove;

        public bool IsSoundEnabled { get => !disableSound && ai == null; }
        public bool IsAI { get => ai != null; }

        public int DieValue { get; private set; }
        private int currentMoveIndex = 0;
        private bool isColoring = false;
        private bool turnFinished = false;
        private PencilColor coloringColor;

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

        public bool CanConfirm
        {
            get
            {
                if (PlayerSheet == null || PlayerSheet.Spaces == null)
                {
                    return false;
                }

                bool hasCompletedTurn = PlayerSheet.Spaces.Sum(x => x.Count(y => y != null && y.IsNew)) == DieValue;
                bool hasSelectedColor = coloringColor != null;
                return hasCompletedTurn && hasSelectedColor && !turnFinished;
            }
        }

        public bool CanUndo => currentMoveIndex > 0 && !turnFinished;

        [Inject] private GameManager _gameManager;

        internal void Initialize(GameOptions.PlayerOptions playerOptions)
        {
            foreach (PencilColor color in _gameManager.ColorTypes)
            {
                ColorUsage.Add(color, 0);
            }

            Score = new(_gameManager);

            PlayerSheet.GenerateMap(map);
            GetComponent<GameMap>().CreateMap();

            Name = playerOptions.PlayerName;
        }

        internal void SetBot(IBot bot)
        {
            ai = bot;
        }

        public void UseColor(PencilColor color)
        {
            if (color == null)
            {
                Debug.LogError($"[Player::AddColor] Color is invalid.");
                return;
            }

            if (!Colors.Contains(color))
            {
                Debug.LogError($"[Player:UseColor] Player {Name} does not own the color {color}.", this);
                return;
            }

            _gameManager.UseColor(this, color);

            Colors.Remove(color);
            ColorUsage[color]++;
            OnColorUsageChanged?.Invoke();
            OnPlayerColorsChanged?.Invoke();
        }

        public void AddColor(PencilColor color)
        {
            if (color == null)
            {
                Debug.LogError($"[Player::AddColor] Color is invalid.");
                return;
            }

            if (Colors.Contains(color))
            {
                Debug.LogError($"[Player:AddColor] AddColor: Player {Name} already owns the color {color}.", this);
                return;
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

            if (color != coloringColor)
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
            if (!CanUndo)
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
            if (!CanConfirm)
            {
                return;
            }

            PlayerSheet.Confirm();

            UseColor(coloringColor);

            PlayerSheet.UpdateAvailableMoves(currentMoveIndex, DieValue);

            currentMoveIndex = 0;
            coloringColor = null;
            isColoring = false;
            turnFinished = true;

            OnPlayerStateChanged?.Invoke();

            // UPDATE SCORES
            Score.SetScore(_gameManager.GreenScoring.GetColor(), _gameManager.GreenScoring.GetScore(PlayerSheet));
            Score.SetScore(_gameManager.BlueScoring.GetColor(), _gameManager.BlueScoring.GetScore(PlayerSheet));
            Score.SetScore(_gameManager.BrownScoring.GetColor(), _gameManager.BrownScoring.GetScore(PlayerSheet));
            Score.SetScore(_gameManager.RedScoring.GetColor(), _gameManager.RedScoring.GetScore(PlayerSheet));
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
                _gameManager.NoMoves();
                DieValue = 0;
            }

            PlayerSheet.UpdateAvailableMoves(currentMoveIndex, DieValue);

            OnPlayerMove?.Invoke(this);
        }

        public void Reset()
        {
            var keys = new List<PencilColor>(ColorUsage.Keys);
            foreach (var key in keys)
            {
                ColorUsage[key] = 0;
            }

            Colors = new();

            PlayerSheet.Reset();
        }

        internal void DoAITurn()
        {
            if (ai == null)
            {
                Debug.LogError($"[Player::DoAITurn] AI is invaild.");
                return;
            }

            ai.DoTurn(this);
        }
    }
}

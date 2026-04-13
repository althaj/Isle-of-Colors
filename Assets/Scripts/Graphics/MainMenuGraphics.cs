using System.Collections;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Gameplay.AI;
using PSG.IsleOfColors.Gameplay.StateMachine;
using PSG.IsleOfColors.Managers;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.Graphics
{
    public class MainMenuGraphics : MonoBehaviour
    {
        [SerializeField] private float turnDelay;

        private Player[] players;
        private IBot ai;

        [Inject] private ApplicationManager _applicationManager;
        [Inject] private GameManager _gameManager;
        [Inject] private GameStateMachine _stateMachine;

        private void Start()
        {
            players = new[] { _gameManager.Player1, _gameManager.Player2 };

            _applicationManager.GameOptions = new GameOptions
            {
                Difficulty = GameOptions.BotDifficulty.MainMenu,
                ShowTutorial = false
            };

            ai = new SimpleAI(_applicationManager);

            if (_gameManager == null || _stateMachine == null)
            {
                Debug.LogError("[MainMenuGraphics:Start] GameManager or GameStateMachine not found. Cannot start the game animation.");
                return;
            }

            StartCoroutine(PlayAnimation());
        }

        private IEnumerator PlayAnimation()
        {
            if (_gameManager == null || _stateMachine == null)
            {
                Debug.LogError("[MainMenuGraphics:PlayAnimation] GameManager or GameStateMachine not found. Cannot start the game animation.");
                yield break;
            }

            _stateMachine.Reset();
            _gameManager.Reset();

            while (true)
            {
                if (_gameManager.IsGameFinished())
                {
                    yield return new WaitForSeconds(turnDelay);
                    _gameManager.Reset();
                    _stateMachine.Reset();
                }

                yield return new WaitForSeconds(Random.Range(turnDelay * 0.1f, turnDelay));

                foreach (Player player in players)
                {
                    ai.DoTurn(player);
                }
            }
        }
    }

}

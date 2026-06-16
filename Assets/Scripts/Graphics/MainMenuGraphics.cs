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

        private IBot ai;

        [Inject] private ApplicationManager _applicationManager;
        [Inject] private GameManager _gameManager;
        [Inject] private GameStateMachine _stateMachine;

        private void Start()
        {
            _gameManager.InvokeAfterInitialization(OnGameInitialized);
        }

        private void OnGameInitialized()
        {
            ai = new SimpleAI(_applicationManager, _gameManager, GameOptions.PlayerType.MainMenu);

            StartCoroutine(PlayAnimation());

            _gameManager.OnGameInitialized.RemoveListener(OnGameInitialized);
        }

        private IEnumerator PlayAnimation()
        {
            while (true)
            {
                if (_gameManager.IsGameFinished())
                {
                    yield return new WaitForSeconds(turnDelay);
                    _gameManager.Reset();
                    _stateMachine.Reset();
                }

                yield return new WaitForSeconds(Random.Range(turnDelay * 0.1f, turnDelay));

                foreach (Player player in _gameManager.Players)
                {
                    ai.DoTurn(player);
                }
            }
        }
    }

}

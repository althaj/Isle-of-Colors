using System.Collections;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Gameplay.AI;
using PSG.IsleOfColors.Gameplay.StateMachine;
using PSG.IsleOfColors.Managers;
using UnityEngine;

namespace PSG.IsleOfColors.Graphics
{
    public class MainMenuGraphics : MonoBehaviour
    {
        [SerializeField] private float turnDelay;

        private Player[] players;
        private IBot ai;
        private GameManager gameManager;
        private GameStateMachine stateMachine;

        private void Start()
        {
            players = FindObjectsByType<Player>(FindObjectsSortMode.None);
            
            ApplicationManager.Instance.GameOptions = new GameOptions
            {
                Difficulty = GameOptions.BotDifficulty.MainMenu,
                ShowTutorial = false
            };
            
            ai = new SimpleAI();
            gameManager = FindFirstObjectByType<GameManager>();
            stateMachine = FindFirstObjectByType<GameStateMachine>();
            
            if (gameManager == null || stateMachine == null)
            {
                Debug.LogError("GameManager or GameStateMachine not found. Cannot start the game animation.");
                return;
            }

            StartCoroutine(PlayAnimation());
        }

        private IEnumerator PlayAnimation()
        {
            if (gameManager == null || stateMachine == null)
            {
                Debug.LogError("GameManager or GameStateMachine not found. Cannot start the game animation.");
                yield break;
            }

            stateMachine.Reset();
            gameManager.Reset();

            while (true)
            {
                if(gameManager.IsGameFinished())
                {
                    yield return new WaitForSeconds(turnDelay);
                    gameManager.Reset();
                    stateMachine.Reset();
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

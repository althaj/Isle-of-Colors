using System.Collections;
using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Gameplay.AI;
using PSG.IsleOfColors.Gameplay.StateMachine;
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
            ai = new SimpleAI();

            ApplicationManager.Instance.GameOptions = new GameOptions{
                Difficulty = GameOptions.BotDifficulty.MainMenu
            };

            gameManager = FindFirstObjectByType<GameManager>();
            stateMachine = FindFirstObjectByType<GameStateMachine>();

            StartCoroutine(DoTurn());
        }

        private IEnumerator DoTurn()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(turnDelay * 0.1f, turnDelay));

                if (gameManager.IsGameFinished())
                {
                    yield return new WaitForSeconds(turnDelay);
                    gameManager.Reset();
                    stateMachine.Reset();
                }
                else
                {
                    foreach (Player player in players)
                    {
                        ai.DoTurn(player);
                    }
                }

            }
        }

    }

}

using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PSG.IsleOfColors.Managers
{
    public class ApplicationManager : MonoBehaviour
    {
        [SerializeField] private string versionString;

        public string VersionString { get => versionString; }

        private GameOptions gameOptions;

        public GameOptions GameOptions
        {
            get
            {
                if (!gameOptions.AreOptionsValid())
                {
                    gameOptions = new()
                    {
                        Players = new()
                        {
                            new()
                            {
                                PlayerName = "Fero",
                                PlayerType = GameOptions.PlayerType.MainMenu
                            },
                            new()
                            {
                                PlayerName = "Jožo",
                                PlayerType = GameOptions.PlayerType.MainMenu

                            }
                        }
                    };
                }

                return gameOptions;
            }

            set => gameOptions = value;
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void StartGame(GameOptions gameOptions)
        {
            GameOptions = gameOptions;
            SceneManager.LoadScene("Game");
        }
    }
}

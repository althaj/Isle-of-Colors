using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PSG.IsleOfColors.Managers
{
    public class ApplicationManager : MonoBehaviour
    {
        [SerializeField] private string versionString;

        public string VersionString { get => versionString; }

        public GameOptions GameOptions { get; set; }

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

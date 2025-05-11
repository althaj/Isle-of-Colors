using System;
using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PSG.IsleOfColors.Managers
{
    public class ApplicationManager : SingletonManager<ApplicationManager>
    {
        [SerializeField] private bool loadMainMenu;
        [SerializeField] private string versionString;

        public string VersionString { get => versionString; }

        public new void Start()
        {
            base.Start();
            if (loadMainMenu)
            {
                LoadMainMenu();
            }

        }

        private GameOptions go;

        // TODO: Update when the tutorial is fully functional and ready
        public GameOptions GameOptions { 
            get {
                go.ShowTutorial = true;
                return go;
            }
            set {
                go = value;
            }
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

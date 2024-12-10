using System;
using PSG.IsleOfColors.Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PSG.IsleOfColors
{
    public class ApplicationManager : MonoBehaviour
    {
        [SerializeField] private bool loadMainMenu;
        [SerializeField] private string versionString;

        public string VersionString { get => versionString; }

        private static ApplicationManager instance;
        public static ApplicationManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindFirstObjectByType<ApplicationManager>();

                if (instance == null)
                    instance = new GameObject("Application Manager", new Type[] { typeof(ApplicationManager) }).GetComponent<ApplicationManager>();

                return instance;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            if (loadMainMenu)
            {
                LoadMainMenu();
            }
        }

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

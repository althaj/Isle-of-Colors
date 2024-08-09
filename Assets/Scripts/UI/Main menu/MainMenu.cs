using UnityEngine;

namespace PSG.IsleOfColors.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        private GameOptionsPopup gameSettingsPopup;

        private void Start()
        {
            gameSettingsPopup = FindFirstObjectByType<GameOptionsPopup>();
            gameSettingsPopup.ClosePopup();
        }

        public void StartSinglePlayer()
        {
            gameSettingsPopup.OpenPopup(true);
        }

        public void StartMultiplayer()
        {
            gameSettingsPopup.OpenPopup(false);
        }

        public void OpenSettings()
        {
            
        }

        public void OpenRules()
        {
            
        }
    }
}

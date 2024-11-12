using UnityEngine;

namespace PSG.IsleOfColors.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        private GameOptionsPopup gameSettingsPopup;
        private RulesPopup rulesPopup;

        private void Start()
        {
            gameSettingsPopup = FindFirstObjectByType<GameOptionsPopup>();
            gameSettingsPopup.ClosePopup();

            rulesPopup = FindFirstObjectByType<RulesPopup>();
            rulesPopup.ClosePopup();
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
            rulesPopup.OpenPopup();
        }
    }
}

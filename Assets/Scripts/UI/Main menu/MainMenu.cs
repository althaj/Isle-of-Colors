using TMPro;
using UnityEngine;

namespace PSG.IsleOfColors.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        private GameOptionsPopup gameSettingsPopup;
        private RulesPopup rulesPopup;

        [SerializeField] private TextMeshProUGUI versionLabel;

        private void Start()
        {
            gameSettingsPopup = FindFirstObjectByType<GameOptionsPopup>();
            gameSettingsPopup.ClosePopup();

            rulesPopup = FindFirstObjectByType<RulesPopup>();
            rulesPopup.ClosePopup();

            versionLabel.text = ApplicationManager.Instance.VersionString;
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

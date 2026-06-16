using PSG.IsleOfColors.Analytics;
using PSG.IsleOfColors.Gameplay;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.Managers
{
    public class AnalyticsManager : MonoBehaviour
    {
        public async void Start()
        {
            await UnityServices.InitializeAsync();

            AnalyticsService.Instance.StartDataCollection();
        }

        public void UserOpenedRulesPopup()
        {
            AnalyticsService.Instance.RecordEvent(new RulesOpenedEvent());
        }

        public void UserOpenedSettingsPopup()
        {
            AnalyticsService.Instance.RecordEvent(new SettingsMenuOpenedEvent());
        }

        public void UserSavedSettingsPopup(AudioSettings audioSettings)
        {
            AnalyticsService.Instance.RecordEvent(new SettingsMenuSavedEvent(audioSettings));
        }

        public void GameEnded(GameManager gameManager, ApplicationManager applicationManager)
        {
            AnalyticsService.Instance.RecordEvent(new GameEndedEvent(gameManager, applicationManager));
        }

        public void GameStarted(GameOptions.PlayerType? difficulty)
        {
            AnalyticsService.Instance.RecordEvent(new GameStartedEvent(difficulty));
        }
    }
}

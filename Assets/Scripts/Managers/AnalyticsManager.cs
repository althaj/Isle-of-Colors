using System;
using PSG.IsleOfColors.Analytics;
using PSG.IsleOfColors.Gameplay;
using Unity.Services.Analytics;
using Unity.Services.Core;

namespace PSG.IsleOfColors.Managers
{
    public class AnalyticsManager : SingletonManager<AnalyticsManager>
    {
        public new async void Start()
        {
            base.Start();

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

        public void GameEnded(GameManager gameManager)
        {
            AnalyticsService.Instance.RecordEvent(new GameEndedEvent(gameManager));
        }

        public void GameStarted(GameOptions.BotDifficulty? difficulty)
        {
            AnalyticsService.Instance.RecordEvent(new GameStartedEvent(difficulty));
        }
    }
}

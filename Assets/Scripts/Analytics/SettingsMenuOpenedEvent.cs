using Unity.Services.Analytics;

namespace PSG.IsleOfColors.Analytics
{
    public class SettingsMenuOpenedEvent : Event
    {
        public SettingsMenuOpenedEvent() : base("SettingsMenuOpened")
        {
        }
    }
}

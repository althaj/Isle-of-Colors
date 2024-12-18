using PSG.IsleOfColors.Managers;
using Unity.Services.Analytics;

namespace PSG.IsleOfColors.Analytics
{
    public class SettingsMenuSavedEvent : Event
    {
        public float MasterVolume { set { SetParameter("MasterVolume", value); } }
        public float MusicVolume { set { SetParameter("MusicVolume", value); } }
        public float UISoundVolume { set { SetParameter("UISoundVolume", value); } }

        public SettingsMenuSavedEvent() : base("SettingsMenuSaved")
        {
        }

        public SettingsMenuSavedEvent(AudioSettings audioSettings) : base("SettingsMenuSaved")
        {
            MasterVolume = audioSettings.MasterVolume;
            MusicVolume = audioSettings.MusicVolume;
            UISoundVolume = audioSettings.UISoundVolume;
        }
    }
}

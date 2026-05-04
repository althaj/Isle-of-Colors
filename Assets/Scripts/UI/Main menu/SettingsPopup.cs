using UnityEngine;
using UnityEngine.UI;
using PSG.IsleOfColors.Managers;
using Zenject;

namespace PSG.IsleOfColors.UI.MainMenu
{
    public class SettingsPopup : MonoBehaviour
    {
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject popupPanel;

        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider uiSoundsVolumeSlider;

        private float volumeMultiplier = 20;

        private Managers.AudioSettings audioSettings;

        [Inject] private AudioManager _audioManager;
        [Inject] private AnalyticsManager _analyticsManager;

        public void OpenPopup()
        {
            audioSettings = _audioManager.LoadAudioSettings();

            masterVolumeSlider.onValueChanged.RemoveAllListeners();
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            uiSoundsVolumeSlider.onValueChanged.RemoveAllListeners();

            masterVolumeSlider.value = Mathf.Pow(10f, audioSettings.MasterVolume / volumeMultiplier);
            musicVolumeSlider.value = Mathf.Pow(10f, audioSettings.MusicVolume / volumeMultiplier);
            uiSoundsVolumeSlider.value = Mathf.Pow(10f, audioSettings.UISoundVolume / volumeMultiplier);

            masterVolumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
            musicVolumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
            uiSoundsVolumeSlider.onValueChanged.AddListener(OnSliderValueChanged);

            background.SetActive(true);
            popupPanel.SetActive(true);

            _analyticsManager.UserOpenedSettingsPopup();
        }

        public void ClosePopup()
        {
            _audioManager.ReloadAudioSettings();

            background.SetActive(false);
            popupPanel.SetActive(false);
        }

        private void OnSliderValueChanged(float value)
        {
            UpdateAudioSettings();
        }

        private void UpdateAudioSettings()
        {
            audioSettings.MasterVolume = Mathf.Log10(masterVolumeSlider.value) * volumeMultiplier;
            audioSettings.MusicVolume = Mathf.Log10(musicVolumeSlider.value) * volumeMultiplier;
            audioSettings.UISoundVolume = Mathf.Log10(uiSoundsVolumeSlider.value) * volumeMultiplier;

            _audioManager.ApplyAudioSettings(audioSettings);
        }

        public void Save()
        {
            UpdateAudioSettings();
            _audioManager.SaveAudioSettings(audioSettings);
            background.SetActive(false);
            popupPanel.SetActive(false);

            _analyticsManager.UserSavedSettingsPopup(audioSettings);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using PSG.IsleOfColors.Managers;

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

        public void OpenPopup()
        {
            audioSettings = AudioManager.Instance.LoadAudioSettings();

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
        }

        public void ClosePopup()
        {
            AudioManager.Instance.ReloadAudioSettings();

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

            AudioManager.Instance.ApplyAudioSettings(audioSettings);
        }

        public void Save()
        {
            UpdateAudioSettings();
            AudioManager.Instance.SaveAudioSettings(audioSettings);
            background.SetActive(false);
            popupPanel.SetActive(false);
        }
    }
}

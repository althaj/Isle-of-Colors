using UnityEngine;
using UnityEngine.Audio;

namespace PSG.IsleOfColors.Managers
{
    public enum UIAudioType
    {
        ButtonClick,
        Confirm,
        Close,
        Error
    }

    public class AudioManager : SingletonManager<AudioManager>
    {
        private AudioSource musicAudioSource;
        private AudioSource uiAudioSource;

        private AudioResource musicResource;

        private AudioResource uiButtonClickResource;
        private AudioResource confirmResource;
        private AudioResource closeResource;
        private AudioResource errorResource;

        private AudioMixer mixer;

        private void Awake()
        {
            musicResource = Resources.Load<AudioResource>("Audio/Music Resource");
            uiButtonClickResource = Resources.Load<AudioResource>("Audio/UI Button Click Resource");
            confirmResource = Resources.Load<AudioResource>("Audio/Confirm Resource");
            closeResource = Resources.Load<AudioResource>("Audio/Close Resource");
            errorResource = Resources.Load<AudioResource>("Audio/Error Resource");

            mixer = Resources.Load<AudioMixer>("Audio/Game Audio Mixer");

            musicAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource.loop = true;
            musicAudioSource.resource = musicResource;
            musicAudioSource.dopplerLevel = 0;
            musicAudioSource.minDistance = 500;
            musicAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/Music")[0];
            musicAudioSource.Play();

            uiAudioSource = gameObject.AddComponent<AudioSource>();
            uiAudioSource.playOnAwake = false;
            uiAudioSource.dopplerLevel = 0;
            uiAudioSource.minDistance = 500;
            uiAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/UI sounds")[0];

            ReloadAudioSettings();
        }

        public void PlayUISound(UIAudioType audioType)
        {
            switch (audioType)
            {
                case UIAudioType.ButtonClick: PlayAudioResource(uiButtonClickResource); break;
                case UIAudioType.Confirm: PlayAudioResource(confirmResource); break;
                case UIAudioType.Close: PlayAudioResource(closeResource); break;
                case UIAudioType.Error: PlayAudioResource(errorResource); break;
            }
        }

        private void PlayAudioResource(AudioResource resource)
        {
            uiAudioSource.Stop();
            uiAudioSource.resource = resource;
            uiAudioSource.Play();
        }

        public AudioSettings LoadAudioSettings()
        {
            AudioSettings result = new AudioSettings();

            result.MasterVolume = PlayerPrefs.GetFloat("Audio_MasterVolume", -12f);
            result.MusicVolume = PlayerPrefs.GetFloat("Audio_MusicVolume", -24f);
            result.UISoundVolume = PlayerPrefs.GetFloat("Audio_UISoundVolume", 0f);

            return result;
        }

        public void SaveAudioSettings(AudioSettings audioSettings)
        {
            PlayerPrefs.SetFloat("Audio_MasterVolume", audioSettings.MasterVolume);
            PlayerPrefs.SetFloat("Audio_MusicVolume", audioSettings.MusicVolume);
            PlayerPrefs.SetFloat("Audio_UISoundVolume", audioSettings.UISoundVolume);

            ApplyAudioSettings(audioSettings);
        }

        public void ApplyAudioSettings(AudioSettings audioSettings)
        {
            mixer.FindMatchingGroups("Master")[0].audioMixer.SetFloat("MasterVolume", audioSettings.MasterVolume);
            mixer.FindMatchingGroups("Master")[0].audioMixer.SetFloat("MusicVolume", audioSettings.MusicVolume);
            mixer.FindMatchingGroups("Master")[0].audioMixer.SetFloat("UISoundsVolume", audioSettings.UISoundVolume);
        }

        public void ReloadAudioSettings()
        {
            ApplyAudioSettings(LoadAudioSettings());
        }
    }
}

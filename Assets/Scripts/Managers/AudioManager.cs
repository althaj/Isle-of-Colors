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

        [SerializeField] private AudioResource[] musicList;

        [SerializeField] private AudioResource uiButtonClickClip;
        [SerializeField] private AudioResource confirmClip;
        [SerializeField] private AudioResource closeClip;
        [SerializeField] private AudioResource errorClip;

        [SerializeField] private AudioMixer mixer;

        private void Awake()
        {
            if (mixer == null)
                mixer = Resources.Load<AudioMixer>("Audio/Game Audio Mixer");

            if (musicList == null || musicList.Length == 0)
                musicList = Resources.LoadAll<AudioResource>("Audio/Music");

            ReloadAudioSettings();

            if (uiButtonClickClip == null)
                uiButtonClickClip = Resources.Load<AudioResource>("Audio/Click");

            if (confirmClip == null)
                confirmClip = Resources.Load<AudioResource>("Audio/Confirm");

            if (closeClip == null)
                closeClip = Resources.Load<AudioResource>("Audio/Close");

            if (errorClip == null)
                errorClip = Resources.Load<AudioResource>("Audio/Error");

            musicAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource.loop = false;
            musicAudioSource.playOnAwake = false;
            musicAudioSource.dopplerLevel = 0;
            musicAudioSource.minDistance = 500;
            musicAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/Music")[0];
            PlaySong();

            uiAudioSource = gameObject.AddComponent<AudioSource>();
            uiAudioSource.playOnAwake = false;
            uiAudioSource.dopplerLevel = 0;
            uiAudioSource.minDistance = 500;
            uiAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/UI sounds")[0];
        }

        private void Update()
        {
            if (!musicAudioSource.isPlaying)
            {
                PlaySong();
            }
        }

        private void PlaySong()
        {
            if(musicList.Length > 0)
            {
                musicAudioSource.resource = RNGManager.RNGManager.Manager["Music"].NextElement(musicList);
                musicAudioSource.Play();
            }
        }

        public void PlayUISound(UIAudioType audioType)
        {
            switch (audioType)
            {
                case UIAudioType.ButtonClick: PlayAudioResource(uiButtonClickClip); break;
                case UIAudioType.Confirm: PlayAudioResource(confirmClip); break;
                case UIAudioType.Close: PlayAudioResource(closeClip); break;
                case UIAudioType.Error: PlayAudioResource(errorClip); break;
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

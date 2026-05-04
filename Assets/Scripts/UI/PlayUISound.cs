using PSG.IsleOfColors.Managers;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    public class PlayUISound : MonoBehaviour
    {
        [SerializeField] private UIAudioType audioType;

        [Inject] private AudioManager _audioManager;

        public void PlayAudio()
        {
            _audioManager.PlayUISound(audioType);
        }
    }
}

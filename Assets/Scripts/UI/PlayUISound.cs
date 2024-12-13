using PSG.IsleOfColors.Managers;
using UnityEngine;

namespace PSG.IsleOfColors.UI
{
    public class PlayUISound : MonoBehaviour
    {
        [SerializeField] private UIAudioType audioType;
        public void PlayAudio()
        {
            AudioManager.Instance.PlayUISound(audioType);
        }
    }
}

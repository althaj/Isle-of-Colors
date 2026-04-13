using PSG.IsleOfColors.Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PSG.IsleOfColors.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButtonSound : MonoBehaviour
    {
        [SerializeField] private UIAudioType audioType;

        [Inject] private AudioManager _audioManager;

        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => PlaySound(_audioManager));
        }

        private void PlaySound(AudioManager audioManager)
        {
            audioManager.PlayUISound(audioType);
        }
    }
}

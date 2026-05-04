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

        void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(PlaySound);
        }

        void OnDisable()
        {
            GetComponent<Button>().onClick.RemoveListener(PlaySound);
        }

        private void PlaySound()
        {
            _audioManager.PlayUISound(audioType);
        }
    }
}

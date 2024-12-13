using PSG.IsleOfColors.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButtonSound : MonoBehaviour
    {
        [SerializeField] private UIAudioType audioType;

        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => AudioManager.Instance.PlayUISound(audioType));
        }
    }
}

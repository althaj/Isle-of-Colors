using PSG.IsleOfColors.Managers;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private UIAudioType audioType;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => AudioManager.Instance.PlayUISound(audioType));
    }
}

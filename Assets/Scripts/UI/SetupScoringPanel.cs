using UnityEngine;

namespace PSG.IsleOfColors.UI
{
    public class SetupScoringPanel : MonoBehaviour
    {
        [SerializeField] private GameObject background;

        private void Awake()
        {
            background.SetActive(true);
        }

        public void Close()
        {
            background.SetActive(false);
        }
    }
}

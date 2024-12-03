using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.IsleOfColors.UI
{
    public class RulesPopup : MonoBehaviour
    {
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject popupPanel;
        [SerializeField] private Transform pagesContainer;

        [SerializeField] private ScrollRect scrollRect;

        [SerializeField] private Button previousPageButton;
        [SerializeField] private Button nextPageButton;

        private GameObject[] pages;
        int currentPageId;

        void Start()
        {
            pages = new GameObject[pagesContainer.childCount];
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i] = pagesContainer.GetChild(i).gameObject;
            }
        }

        public void OpenPopup()
        {
            background.SetActive(true);
            popupPanel.SetActive(true);
            SwitchToPage(0);
        }

        public void ClosePopup()
        {
            background.SetActive(false);
            popupPanel.SetActive(false);
        }

        public void SwitchToNextPage()
        {
            SwitchToPage(currentPageId + 1);
        }

        public void SwitchToPreviousPage()
        {
            SwitchToPage(currentPageId - 1);
        }

        private void SwitchToPage(int pageIndex)
        {
            currentPageId = pageIndex;
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].SetActive(i == currentPageId);
            }

            StartCoroutine(ScrollToTop());

            previousPageButton.interactable = currentPageId > 0;
            nextPageButton.interactable = currentPageId < pages.Length - 1;
        }

        // Scrolling needs to happen at the end of a frame
        IEnumerator ScrollToTop()
        {
            yield return new WaitForEndOfFrame();
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayerUIScene.Expandables
{
    public sealed class ExpandableWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button _expandButton;
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _collapsedImage;
        [SerializeField] private GameObject _expandedImage;

        private bool _isExpanded;

        private void OnEnable()
        {
            _expandButton.onClick.AddListener(OnExpandButtonClicked);
        }

        private void OnDisable()
        {
            _expandButton.onClick.RemoveListener(OnExpandButtonClicked);
        }

        public void Expand()
        {
            ToggleWindow(true);
        }

        public void Collapse()
        {
            CollapseWindow();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isExpanded)
            {
                return;
            }

            _collapsedImage.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _collapsedImage.SetActive(false);
        }

        private void OnExpandButtonClicked()
        {
            ToggleWindow(!_isExpanded);
        }

        private void CollapseWindow()
        {
            _content.SetActive(false);
            _expandedImage.SetActive(false);
            _collapsedImage.SetActive(false);
            _isExpanded = false;
        }

        private void ToggleWindow(bool isExpanded)
        {
            _content.SetActive(isExpanded);
            _expandedImage.SetActive(isExpanded);
            _collapsedImage.SetActive(!isExpanded);
            _isExpanded = isExpanded;
        }
    }
}
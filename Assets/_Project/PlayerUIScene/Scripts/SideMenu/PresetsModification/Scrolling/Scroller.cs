using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayerUIScene.SideMenu.FurnitureArrangementPanel.Scrolling
{
    public sealed class Scroller : MonoBehaviour
    {
        private const float ScrollbarMaxValue = 1f;
        private const float ScrollCentringDuration = 1f;
        private const float DefaultValue = 0f;
        private const int DefaultSelectedValue = 0;
        private const int ScrollingStep = 1;

        [HideInInspector] public UnityEvent<int> OnNewScrollElementSelected;

        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private RectTransform _togglesParentTransform;
        [SerializeField] private Toggle _togglePrefab;
        [SerializeField] private bool _showToggles = true;
        [Header("Buttons")] [SerializeField] private Button _scrollLeftButton;
        [SerializeField] private Button _scrollRightButton;

        private List<Toggle> _togglesSummary = new List<Toggle>();
        private float[] _scrollPositions = new[] { DefaultValue };
        private int _selectedElement = 0;

        public void UpdateScroller(int number)
        {
            CreateScrollArray(number);
            UpdateToggles(number);
            ToggleScrollButtons(number != ScrollingStep);
            DoScroll(DefaultSelectedValue);
        }

        private void OnEnable()
        {
            _scrollbar.onValueChanged.AddListener(OnScroll);
            _scrollLeftButton.onClick.AddListener(OnScrollLeftButtonClicked);
            _scrollRightButton.onClick.AddListener(OnScrollRightButtonClicked);
        }

        private void Start()
        {
            _togglesParentTransform.gameObject.SetActive(_showToggles);
        }

        private void OnDisable()
        {
            _scrollbar.onValueChanged.RemoveListener(OnScroll);
            _scrollLeftButton.onClick.RemoveListener(OnScrollLeftButtonClicked);
            _scrollRightButton.onClick.RemoveListener(OnScrollRightButtonClicked);
        }

        private void OnScrollLeftButtonClicked()
        {
            DoScroll(_selectedElement - ScrollingStep);
        }

        private void OnScrollRightButtonClicked()
        {
            DoScroll(_selectedElement + ScrollingStep);
        }

        private void OnScroll(float value)
        {
            if (!_scrollbar.interactable)
            {
                return;
            }

            int selectedElement = _scrollPositions
                .Select((position, index) => (position, index))
                .OrderBy(x => Mathf.Abs(x.position - value))
                .First()
                .index;

            if (selectedElement == _selectedElement)
            {
                return;
            }

            DoScroll(selectedElement);
        }

        private void DoScroll(int selectedElement)
        {
            if (!_scrollbar.interactable)
            {
                return;
            }

            _scrollbar.interactable = false;
            DOTween.To(() => _scrollbar.value, x => _scrollbar.value = x, _scrollPositions[selectedElement], ScrollCentringDuration)
                .OnComplete(() => _scrollbar.interactable = true);

            _selectedElement = selectedElement;

            SelectTargetToggle();
            UpdateScrollingButtons(_selectedElement);
            OnNewScrollElementSelected?.Invoke(_selectedElement);
        }

        private void ToggleScrollButtons(bool toEnable)
        {
            _scrollRightButton.gameObject.SetActive(toEnable);
            _scrollLeftButton.gameObject.SetActive(toEnable);
        }

        private void SelectTargetToggle()
        {
            if (!_showToggles)
            {
                return;
            }

            if (_togglesSummary.Count == DefaultValue)
            {
                return;
            }

            foreach (Toggle toggle in _togglesSummary)
            {
                toggle.isOn = false;
            }

            _togglesSummary[_selectedElement].isOn = true;
        }

        private void UpdateScrollingButtons(int selectedElement)
        {
            _scrollRightButton.interactable = selectedElement != _scrollPositions.Length - ScrollingStep;
            _scrollLeftButton.interactable = selectedElement != 0;
        }

        private void CreateScrollArray(int number)
        {
            ResetScrollbar();

            _scrollPositions = Enumerable.Range(DefaultSelectedValue, number)
                .Select(i => i * ScrollbarMaxValue / (number - ScrollingStep))
                .ToArray();
        }

        private void ResetScrollbar()
        {
            _scrollbar.value = DefaultValue;
            _selectedElement = DefaultSelectedValue;
        }

        private void UpdateToggles(int number)
        {
            if (!_showToggles)
            {
                return;
            }

            foreach (Toggle toggle in _togglesSummary)
            {
                Destroy(toggle.gameObject);
            }

            _togglesSummary.Clear();

            if (number == ScrollingStep)
            {
                return;
            }

            for (int i = 0; i < number; i++)
            {
                _togglesSummary.Add(Instantiate(_togglePrefab, _togglesParentTransform.transform));
            }

            _togglesSummary[0].isOn = true;
        }
    }
}
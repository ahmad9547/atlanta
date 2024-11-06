using Core.ServiceLocator;
using Metaverse.AreaModification.Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayerUIScene.SideMenu.AreaModification
{
    public sealed class ModificationToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ModificationType _modificationType;
        [SerializeField] private bool _isOn;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _background;
        [SerializeField] private GameObject _onImage;
        [SerializeField] private GameObject _offImage;
        [SerializeField] private GameObject _onImageParent;
        [SerializeField] private GameObject _offImageParent;

        public bool IsOn => _isOn;

        [HideInInspector] public UnityEvent<bool> OnValueChanged = new UnityEvent<bool>();

        #region Services

        private IAreaModificator _areaModificatorInstance;

        private IAreaModificator _areaModificator
            => _areaModificatorInstance ??= Service.Instance.Get<IAreaModificator>();

        #endregion

        private void OnEnable()
        {
            _button.onClick.AddListener(ChangeToggleState);
            _areaModificator.OnStateChanged.AddListener(OnStateChanged);
        }

        private void Start()
        {
            ToggleVisual();
            _background.SetActive(false);
            _onImage.SetActive(false);
            _offImage.SetActive(false);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ChangeToggleState);
            _areaModificator.OnStateChanged.AddListener(OnStateChanged);
        }

        private void ChangeToggleState()
        {
            _isOn = !_isOn;
            ToggleVisual();
            OnValueChanged?.Invoke(_isOn);
        }

        private void OnStateChanged(ModificationType modificationType, bool isOn)
        {
            if (modificationType != _modificationType)
            {
                return;
            }

            _isOn = isOn;
            ToggleVisual();
        }

        private void ToggleVisual()
        {
            _onImageParent.SetActive(_isOn);
            _offImageParent.SetActive(!_isOn);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _background.SetActive(true);
            _onImage.SetActive(true);
            _offImage.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _background.SetActive(false);
            _onImage.SetActive(false);
            _offImage.SetActive(false);
        }
    }
}
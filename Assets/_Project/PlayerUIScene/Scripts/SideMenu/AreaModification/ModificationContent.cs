using System;
using Core.ServiceLocator;
using Metaverse.AreaModification.Services;
using PhotonEngine.CustomProperties.Services;
using UnityEngine;

namespace PlayerUIScene.SideMenu.AreaModification
{
    public class ModificationContent : MonoBehaviour
    {
        [SerializeField] private ModificationType _modificationType;
        [SerializeField] private ModificationToggle _modificationToggle;

        #region Services

        private IRoomCustomPropertiesService _roomCustomPropertiesService;
        private IRoomCustomPropertiesService _roomCustomProperties
            => _roomCustomPropertiesService ??= Service.Instance.Get<IRoomCustomPropertiesService>();

        private IAreaModificator _areaModificatorInstance;
        private IAreaModificator _areaModificator
            => _areaModificatorInstance ??= Service.Instance.Get<IAreaModificator>();

        #endregion

        private void OnEnable()
        {
            _modificationToggle.OnValueChanged.AddListener(OnToggleValueChanged);
        }

        private void Start()
        {
            _areaModificator.ChangeState(_modificationType,GetState());
        }

        private void OnDisable()
        {
            _modificationToggle.OnValueChanged.RemoveListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            _roomCustomProperties.AddOrUpdateRoomCustomProperty(_modificationType.ToString(), isOn);
            _areaModificator.ChangeState(_modificationType, isOn);
        }

        private bool GetState()
        {
            return _roomCustomProperties.CustomPropertiesContainKey(_modificationType.ToString())
                ? Convert.ToBoolean(_roomCustomProperties.GetRoomCustomProperty(_modificationType.ToString()))
                : _modificationToggle.IsOn;
        }
    }
}

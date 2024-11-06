using Core.ServiceLocator;
using Metaverse.AreaModification.Services;
using PlayerUIScene.SideMenu.AreaModification;
using UnityEngine;

namespace Metaverse.AreaModification
{
    public sealed class FurnitureModification : MonoBehaviour
    {
        [SerializeField] private GameObject _furnitureParent;

        #region Services

        private IAreaModificator _areaModificatorInstance;
        private IAreaModificator _areaModificator
            => _areaModificatorInstance ??= Service.Instance.Get<IAreaModificator>();

        #endregion

        private void OnEnable()
        {
            _areaModificator.OnStateChanged.AddListener(OnFurnitureStateChanged);
        }

        private void OnDisable()
        {
            _areaModificator.OnStateChanged.AddListener(OnFurnitureStateChanged);
        }
        
        private void OnFurnitureStateChanged(ModificationType modificationType, bool isOn)
        {
            if (modificationType != ModificationType.Furniture)
            {
                return;
            }

            _furnitureParent.SetActive(isOn);
        }
    }
}
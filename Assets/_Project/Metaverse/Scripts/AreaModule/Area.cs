using Core.ServiceLocator;
using Metaverse.AreaModule.Services;
using Metaverse.Services;
using UnityEngine;

namespace Metaverse.AreaModule
{
    public sealed class Area : MonoBehaviour
    {
        [SerializeField] private AreaType _areaType;
        [Header("Debug:")] [SerializeField] private Color _teleportPointColor;

        #region Services

        private ILocalPlayerService _localPlayerHolderInstance;
        private ILocalPlayerService _localPlayerHolder
            => _localPlayerHolderInstance ??= Service.Instance.Get<ILocalPlayerService>();

        private IAreaInformationHolderService _areaInformationHolderInstance;
        private IAreaInformationHolderService _areaInformationHolder
            => _areaInformationHolderInstance ??= Service.Instance.Get<IAreaInformationHolderService>();

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (!_localPlayerHolder.IsOtherPlayerEqualsLocalPlayer(other.gameObject))
            {
                return;
            }

            _areaInformationHolder.ChangePlayerArea(_areaType.AreaInformation);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _teleportPointColor;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}
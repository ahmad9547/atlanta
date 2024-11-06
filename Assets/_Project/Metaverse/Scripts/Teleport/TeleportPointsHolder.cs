using System.Collections.Generic;
using System.Linq;
using Core.ServiceLocator;
using Metaverse.ErrorHandling;
using Metaverse.ErrorHandling.Services;
using Metaverse.Teleport.Interfaces;
using UnityEngine;

namespace Metaverse.Teleport
{
    public sealed class TeleportPointsHolder : MonoBehaviour
    {
        [SerializeField] private List<TeleportPoint> _startupTeleportPoints;

        #region Services

        private IStartupPointHolderService _startupPointHolderInstance;
        private IStartupPointHolderService _startupPointHolder
            => _startupPointHolderInstance ??= Service.Instance.Get<IStartupPointHolderService>();

        #endregion

        public TeleportPoint SelectTeleportPoint()
        {
            return FindTargetTeleportPoint();
        }

        private TeleportPoint FindTargetTeleportPoint()
        {
            TeleportPoint teleportPoint = _startupTeleportPoints.Find(teleportPoint =>
                teleportPoint.TeleportPointType.PointType == _startupPointHolder.StartupPointType);

            if (teleportPoint != null)
            {
                return teleportPoint;
            }

            ErrorLogger.LogError(ErrorType.Error,
                $"Unable to find teleport point with ID - {_startupPointHolder.StartupPointType}. Setting default teleport point.");
            return _startupTeleportPoints.First();
        }
    }
}
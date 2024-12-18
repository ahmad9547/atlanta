using Core.ServiceLocator;
using UnityEngine;

namespace Metaverse.Teleport.Database
{
    public sealed class TeleportPointNamesProvider : MonoBehaviour
    {
        [SerializeField] private TeleportPointNamesDatabase _teleportPointNamesDatabase;

        #region Services

        private ITeleportPointNamesService _teleportPointNamesInstance;
        private ITeleportPointNamesService _teleportPointNames
            => _teleportPointNamesInstance ??= Service.Instance.Get<ITeleportPointNamesService>();

        #endregion

        private void Awake()
        {
            _teleportPointNames.InitializeTeleportPointNames(_teleportPointNamesDatabase);
        }
    }
}
using Core.ServiceLocator;
using UnityEngine.Events;

namespace Metaverse.PlayersSettings
{
    public interface IPlayerSettingsService : IService
    {
        bool IsUnmuted { get; set; }

        bool IsThirdPersonCameraActive { get; set; }

        UnityEvent OnThirdPersonCameraDefaultDampingSet { get; }

        UnityEvent OnThirdPersonCameraDampingDisabled { get; }

        void SetThirdPersonCameraDefaultDamping();

        void DisableThirdPersonCameraDamping();
    }
}
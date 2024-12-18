using UnityEngine.Events;

namespace Metaverse.PlayersSettings
{
    public class PlayerSettings : IPlayerSettingsService
    {
        public bool IsUnmuted { get; set; }
        public bool IsThirdPersonCameraActive { get; set; }

        public UnityEvent OnThirdPersonCameraDefaultDampingSet { get; } = new UnityEvent();

        public UnityEvent OnThirdPersonCameraDampingDisabled { get; } = new UnityEvent();

        public void SetThirdPersonCameraDefaultDamping()
        {
            OnThirdPersonCameraDefaultDampingSet?.Invoke();
        }

        public void DisableThirdPersonCameraDamping()
        {
            OnThirdPersonCameraDampingDisabled?.Invoke();
        }
    }
}
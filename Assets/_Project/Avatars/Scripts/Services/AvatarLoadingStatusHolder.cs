using UnityEngine.Events;

namespace Avatars.Services
{
    public sealed class AvatarLoadingStatusHolder : IAvatarLoadingStatusHolderService
    {
        public UnityEvent OnAvatarLoaded { get; set; } = new ();

        public void AvatarLoaded()
        {
            OnAvatarLoaded?.Invoke();
        }
    }
}
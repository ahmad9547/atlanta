using Core.ServiceLocator;
using UnityEngine.Events;

namespace Avatars.Services
{
    public interface IAvatarLoadingStatusHolderService : IService
    {
        public UnityEvent OnAvatarLoaded { get; set; }

        public void AvatarLoaded();
    }
}
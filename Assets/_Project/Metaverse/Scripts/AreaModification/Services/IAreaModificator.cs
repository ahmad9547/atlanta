using Core.ServiceLocator;
using PlayerUIScene.SideMenu.AreaModification;
using UnityEngine.Events;

namespace Metaverse.AreaModification.Services
{
    public interface IAreaModificator : IService
    {
        UnityEvent<ModificationType, bool> OnStateChanged { get; }

        void ChangeState(ModificationType modificationType, bool isOn);
    }
}
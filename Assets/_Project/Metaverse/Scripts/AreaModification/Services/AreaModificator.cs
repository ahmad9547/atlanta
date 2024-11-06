using PlayerUIScene.SideMenu.AreaModification;
using UnityEngine.Events;

namespace Metaverse.AreaModification.Services
{
    public sealed class AreaModificator : IAreaModificator
    {
        public UnityEvent<ModificationType, bool> OnStateChanged { get; } = new UnityEvent<ModificationType, bool>();

        public void ChangeState(ModificationType modificationType, bool isOn)
        {
            OnStateChanged?.Invoke(modificationType, isOn);
        }
    }
}
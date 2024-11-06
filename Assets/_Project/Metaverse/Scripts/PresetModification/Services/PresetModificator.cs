using Metaverse.PresetModification.Interfaces;
using UnityEngine.Events;

namespace Metaverse.PresetModification.Services
{
    public sealed class PresetModificator : IPresetModificationService
    {
        public UnityEvent<PresetsModificationAsset> OnModificationPresetsAssetProvided { get; } = new UnityEvent<PresetsModificationAsset>();

        public UnityEvent<RoomType, int> OnModificationPresetSelected { get; } = new UnityEvent<RoomType, int>();

        public UnityEvent OnPresetsAssetRemoved { get; } = new UnityEvent();

        public void ProvideModificationPresetsAsset(PresetsModificationAsset presetsModificationAsset)
        {
            OnModificationPresetsAssetProvided?.Invoke(presetsModificationAsset);
        }

        public void SelectModificationPreset(RoomType roomType, int presetNumber)
        {
            OnModificationPresetSelected?.Invoke(roomType, presetNumber);
        }

        public void RemovePresetsAsset()
        {
            OnPresetsAssetRemoved?.Invoke();
        }
    }
}
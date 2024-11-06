using Core.ServiceLocator;
using UnityEngine.Events;

namespace Metaverse.PresetModification.Interfaces
{
    public interface IPresetModificationService : IService
    {
        UnityEvent<PresetsModificationAsset> OnModificationPresetsAssetProvided { get; }

        UnityEvent<RoomType, int> OnModificationPresetSelected { get; }

        UnityEvent OnPresetsAssetRemoved { get; }

        void ProvideModificationPresetsAsset(PresetsModificationAsset presetsModificationAsset);

        void SelectModificationPreset(RoomType roomType, int presetNumber);

        void RemovePresetsAsset();
    }
}
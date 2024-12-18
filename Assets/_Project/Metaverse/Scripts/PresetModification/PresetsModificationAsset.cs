using System.Collections.Generic;
using UnityEngine;

namespace Metaverse.PresetModification
{
    [CreateAssetMenu(fileName = "PresetsModificationAsset", menuName = "ScriptableObjects/PresetsModificationAsset")]
    public sealed class PresetsModificationAsset : ScriptableObject
    {
        [SerializeField] private RoomType _roomType;
        [SerializeField] private List<ModificationPreset> _presets = new List<ModificationPreset>();

        public RoomType RoomType => _roomType;
        public List<ModificationPreset> Presets => _presets;
    }
}
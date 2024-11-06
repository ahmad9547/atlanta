using UnityEngine;
using System;

namespace Metaverse.PresetModification
{
    [Serializable]
    public sealed class ModificationPreset
    {
        [SerializeField] private int _presetNumber;
        [SerializeField] private string _presetName;
        [SerializeField] private GameObject _presetPrefab;
        [SerializeField] private Sprite _presetPreviewSprite;

        public int PresetNumber => _presetNumber;
        public string PresetName => _presetName;
        public GameObject PresetPrefab => _presetPrefab;
        public Sprite PresetPreviewSprite => _presetPreviewSprite;
    }
}
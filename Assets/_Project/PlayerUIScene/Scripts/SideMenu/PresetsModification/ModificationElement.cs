using Metaverse.PresetModification;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.SideMenu.PresetsModification
{
    public sealed class ModificationElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _header;
        [SerializeField] private Image _image;

        public void Initialize(ModificationPreset modificationPreset)
        {
            _header.text = modificationPreset.PresetName;
            _image.sprite = modificationPreset.PresetPreviewSprite;
        }
    }
}

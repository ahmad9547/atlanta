using ProjectConfig.Playlist;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.GiftShopMenu
{
    public sealed class MusicClipPreview : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _authorName;
        [SerializeField] private RawImage _preview;

        public void Initialize(MusicClip musicClip)
        {
            _name.text = musicClip.Name;
            _authorName.text = musicClip.Author;
            _preview.texture = musicClip.Preview;
        }
    }
}
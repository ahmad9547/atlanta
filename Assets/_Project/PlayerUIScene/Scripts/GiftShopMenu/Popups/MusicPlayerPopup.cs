using Core.ServiceLocator;
using Metaverse.GiftShop.Interfaces;
using PlayerUIScene.SideMenu.FurnitureArrangementPanel.Scrolling;
using ProjectConfig.Playlist;
using UnityEngine;

namespace PlayerUIScene.GiftShopMenu.Popups
{
    public sealed class MusicPlayerPopup : Popup
    {
        [SerializeField] private Scroller _scroller;
        [SerializeField] private MusicClipPreview _musicClipPreviewPrefab;
        [SerializeField] private RectTransform _musicClipParent;
        [SerializeField] private GameObject _loaderPanel;
        [SerializeField] private GameObject _musicSelectionPanel;

        #region Services

        private IMusicPlayerEventHandler _musicPlayerEventHandlerInstance;
        private IMusicPlayerEventHandler _musicPlayerEventHandler
            => _musicPlayerEventHandlerInstance ??= Service.Instance.Get<IMusicPlayerEventHandler>();

        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            _musicPlayerEventHandler.OnPlaylistDownloaded.AddListener(OnPlaylistDownloaded);
            _musicPlayerEventHandler.OnShowMusicPlayer.AddListener(OnShowMusicSelection);
            _musicPlayerEventHandler.OnHideMusicPlayer.AddListener(OnHideMusicSelection);
            _scroller.OnNewScrollElementSelected.AddListener(OnNewElementSelected);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _musicPlayerEventHandler.OnPlaylistDownloaded.RemoveListener(OnPlaylistDownloaded);
            _musicPlayerEventHandler.OnShowMusicPlayer.RemoveListener(OnShowMusicSelection);
            _musicPlayerEventHandler.OnHideMusicPlayer.RemoveListener(OnHideMusicSelection);
            _scroller.OnNewScrollElementSelected.RemoveListener(OnNewElementSelected);
        }

        private void OnPlaylistDownloaded()
        {
            TogglePanels(true);

            foreach (MusicClip musicClip in _musicPlayerEventHandler.MusicClips)
            {
                MusicClipPreview clipPreview = Instantiate(_musicClipPreviewPrefab, _musicClipParent);
                clipPreview.Initialize(musicClip);
            }

            _scroller.UpdateScroller(_musicPlayerEventHandler.MusicClips.Count);
        }

        private void OnShowMusicSelection()
        {
            ShowPopup();
        }

        private void OnHideMusicSelection()
        {
            ClosePopup();
        }

        private void OnNewElementSelected(int number)
        {
            _musicPlayerEventHandler.SelectMusicClip(number);
        }

        private void TogglePanels(bool isPlaylistDownloaded)
        {
            _loaderPanel.SetActive(!isPlaylistDownloaded);
            _musicSelectionPanel.SetActive(isPlaylistDownloaded);
        }
    }
}
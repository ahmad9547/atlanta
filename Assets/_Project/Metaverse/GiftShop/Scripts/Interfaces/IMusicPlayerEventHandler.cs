using System.Collections.Generic;
using Core.ServiceLocator;
using ProjectConfig.Playlist;
using UnityEngine.Events;

namespace Metaverse.GiftShop.Interfaces
{
    public interface IMusicPlayerEventHandler : IService
    {
        List<MusicClip> MusicClips { get; set; }

        UnityEvent OnPlaylistDownloaded { get; }

        UnityEvent OnShowMusicPlayer { get; }

        UnityEvent OnHideMusicPlayer { get; }

        UnityEvent<int> OnMusicClipSelected { get; }

        void ShowMusicPlayer();

        void HideMusicPlayer();

        void SelectMusicClip(int number);

        void LoadPlaylist();
    }
}
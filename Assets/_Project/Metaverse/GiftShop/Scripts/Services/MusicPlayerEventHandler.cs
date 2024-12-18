using System.Collections.Generic;
using Metaverse.GiftShop.Interfaces;
using ProjectConfig.Playlist;
using UnityEngine.Events;

namespace Metaverse.GiftShop.Services
{
    public sealed class MusicPlayerEventHandler : IMusicPlayerEventHandler
    {
        public List<MusicClip> MusicClips { get; set; } = new List<MusicClip>();

        public UnityEvent OnPlaylistDownloaded { get; } = new UnityEvent();

        public UnityEvent OnShowMusicPlayer { get; } = new UnityEvent();

        public UnityEvent OnHideMusicPlayer { get; } = new UnityEvent();

        public UnityEvent<int> OnMusicClipSelected { get; } = new UnityEvent<int>();

        public void ShowMusicPlayer()
        {
            OnShowMusicPlayer?.Invoke();
        }

        public void HideMusicPlayer()
        {
            OnHideMusicPlayer?.Invoke();
        }

        public void SelectMusicClip(int number)
        {
            OnMusicClipSelected?.Invoke(number);
        }

        public async void LoadPlaylist()
        {
            MusicClips.Clear();

            foreach (MusicClipModel model in PlaylistConfig.MusicClips)
            {
                MusicClip musicClip = new MusicClip();
                await musicClip.Initialize(model);
                MusicClips.Add(musicClip);
            }

            OnPlaylistDownloaded?.Invoke();
        }
    }
}

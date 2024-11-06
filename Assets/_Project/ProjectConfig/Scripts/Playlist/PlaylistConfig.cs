using System.Collections.Generic;

namespace ProjectConfig.Playlist
{
    public static class PlaylistConfig
    {
        private static List<MusicClipModel> _musicClips;

        public static List<MusicClipModel> MusicClips => _musicClips;

        public static void SetupPlaylist(PlaylistConfigModel playlistConfigModel)
        {
            _musicClips = playlistConfigModel.MusicClips;
        }
    }
}

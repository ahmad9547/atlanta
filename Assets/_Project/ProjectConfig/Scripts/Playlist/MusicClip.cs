using System.IO;
using System.Threading.Tasks;
using Core.FilesReading;
using Core.ServiceLocator;
using ProjectConfig.General;
using UnityEngine;

namespace ProjectConfig.Playlist
{
    public sealed class MusicClip
    {
        public string Name;
        public string Author;
        public AudioClip Clip;
        public Texture Preview;

        #region Services

        private IWebRequestsLoaderService _webRequestsLoaderInstance;
        private IWebRequestsLoaderService _webRequestsLoader
            => _webRequestsLoaderInstance ??= Service.Instance.Get<IWebRequestsLoaderService>();

        #endregion

        public async Task Initialize(MusicClipModel musicClipModel)
        {
            Name = musicClipModel.Name;
            Author = musicClipModel.Author;
            Preview = await _webRequestsLoader.DownloadTexture(GetContentPath(musicClipModel.AudioClipPreviewUrl));
            Clip = await _webRequestsLoader.DownloadAudioClip(GetContentPath(musicClipModel.AudioClipUrl));
        }

        private string GetContentPath(string contentName)
        {
            return Path.Combine(GeneralSettings.ContentFolderUrl, contentName);
        }
    }
}
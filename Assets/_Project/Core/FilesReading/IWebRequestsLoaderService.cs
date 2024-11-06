using Core.ServiceLocator;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.FilesReading
{
    public interface IWebRequestsLoaderService : IService
    {
        Task<string> DownloadFileText(string url, bool useTimestamp = true);

        Task<Texture> DownloadTexture(string url, bool useTimestamp = true);

        Task<AudioClip> DownloadAudioClip(string url, bool useTimestamp = true);
    }
}
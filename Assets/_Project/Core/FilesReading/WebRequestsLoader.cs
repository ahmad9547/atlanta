using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.FilesReading
{
    public sealed class WebRequestsLoader : IWebRequestsLoaderService
    {
        private const string UrlTimestamp = "?t=";

        private readonly DateTime _timestampDefaultDateTime = new DateTime(1970, 1, 1);

        /// <summary>
        /// Downloads file via UnityWebRequest using url parameter, modified to have timestamp
        /// if useTimestamp parameter is set to true and returns the text content of the file as a string.
        /// <param name="url"></param>
        /// <param name="useTimestamp"></param>
        /// <returns></returns>
        public async Task<string> DownloadFileText(string url, bool useTimestamp = true)
        {
            string fileUrl = useTimestamp ? GetFileUrlWithTimestamp(url) : url;
            using UnityWebRequest webRequest = UnityWebRequest.Get(fileUrl);
            UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                return webRequest.downloadHandler.text;
            }

            Debug.LogError($"Failed while loading file: {webRequest.error}");
            return null;
        }

        /// <summary>
        /// Downloads texture via UnityWebRequest using url parameter, modified to have timestamp
        /// if useTimestamp parameter is set to true and returns the Texture.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="useTimestamp"></param>
        /// <returns></returns>
        public async Task<Texture> DownloadTexture(string url, bool useTimestamp = true)
        {
            string fileUrl = useTimestamp ? GetFileUrlWithTimestamp(url) : url;
            using UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(fileUrl);
            UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerTexture.GetContent(webRequest);
            }

            Debug.LogError($"Failed while loading {url} file: {webRequest.error}");
            return null;
        }

        /// <summary>
        /// Downloads multimedia via UnityWebRequest using url parameter, modified to have timestamp
        /// if useTimestamp parameter is set to true and returns the AudioClip.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="useTimestamp"></param>
        /// <returns></returns>
        public async Task<AudioClip> DownloadAudioClip(string url, bool useTimestamp = true)
        {
            string fileUrl = useTimestamp ? GetFileUrlWithTimestamp(url) : url;
            using UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.MPEG);
            UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerAudioClip.GetContent(webRequest);
            }

            Debug.LogError($"Failed while loading {url} file: {webRequest.error}");
            return null;
        }

        /// <summary>
        /// Generates unique timestamp by calculating amount of seconds between default DateTime and current DateTime
        /// and adds it as a parameter to specified url.
        /// Adding time stamp is needed in order to create unique URL and to download current version of asset,
        /// since it could be modified in AWS Bucket but listed as cached in client.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetFileUrlWithTimestamp(string url)
        {
            int unixTimestamp = (int)DateTime.UtcNow.Subtract(_timestampDefaultDateTime).TotalSeconds;
            return $"{url}{UrlTimestamp}{unixTimestamp}";
        }
    }
}

using UnityEngine;
using System.Runtime.InteropServices;
using System;
using Core.ServiceLocator;
#if UNITY_WEBGL
using AgoraIO.Media;
#endif

namespace VoiceChat.WebGL
{
    /// <summary>
    /// WebVoiceChat.cs responsible to routs calls to JavaScript library Plugins\WEBGL\VoiceChatAPI.jslib.
    /// This file should be DontDestroyOnLoad and it should be placed as separate gameObject in start scene.
    /// It should be done, because VoiceChat.js file perform sending messages to this script only when it was initialized
    /// in first loaded scene.
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class WebVoiceChat : MonoBehaviour
    {
        private static bool _instancePresentOnScene;

        [DllImport("__Internal")]
        private static extern void SetVolumeForAllUsers(int volume);

        #region Services

        private IWebVoiceChatAPIService _webVoiceChatAPIInstance;
        private IWebVoiceChatAPIService _webVoiceChatAPI
            => _webVoiceChatAPIInstance ??= Service.Instance.Get<IWebVoiceChatAPIService>();

        #endregion

        private void Awake()
        {
            TryCreateInstance();
        }

        private void OnDestroy()
        {
            _webVoiceChatAPI.OnPlayerVolumesReceived?.RemoveAllListeners();
        }

        /// <summary>
        /// This method accessing from VoiceChat.js file with unityInstance.SendMessage
        /// </summary>
        /// <param name="jsonVolumes"></param>
        public void SetVolumesValues(string jsonVolumes)
        {
            try
            {
                PlayersVolume playersVolume = JsonUtility.FromJson<PlayersVolume>(jsonVolumes);
                _webVoiceChatAPI.ReceivePlayerVolumes(playersVolume);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error while getting players volume values: {exception}");
            }
        }

        /// <summary>
        /// This method accessing from VoiceChat.js file with unityInstance.SendMessage
        /// </summary>
        public void JoinedGeneralVoiceChat()
        {
            _webVoiceChatAPI.JoinGeneralVoiceChat();
        }

        /// <summary>
        /// This method accessing from VoiceChat.js file with unityInstance.SendMessage
        /// </summary>
        public void JoinedAdminVoiceChat()
        {
            _webVoiceChatAPI.JoinAdminVoiceChat();

            _webVoiceChatAPI.IsVoiceChatUsageAllowed = true;
            Debug.Log("Voice chat usage is allowed");
        }

        public void SetVoiceVolumeForAllUsers(int volume)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            if (!_webVoiceChatAPI.IsVoiceChatUsageAllowed)
            {
                return;
            }

            SetVolumeForAllUsers(volume);
        }

        private bool TryCreateInstance()
        {
            if (_instancePresentOnScene)
            {
                Destroy(gameObject);
                return false;
            }
            _instancePresentOnScene = true;
            DontDestroyOnLoad(gameObject);
            return true;
        }
    }
}
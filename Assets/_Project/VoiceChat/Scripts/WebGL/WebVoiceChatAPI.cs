using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_WEBGL
using AgoraIO.Media;
#endif

namespace VoiceChat.WebGL
{
    public class WebVoiceChatAPI : IWebVoiceChatAPIService
    {
        private const string AgoraAppId = "963cfd2522d3460988aaa22c5aaf28be";
        private const string AppCertificate = "f05920b75b744f82ae6cbec9853caf8d";

        #region DllImport

        [DllImport("__Internal")]
        private static extern void LeaveVoiceChat();

        [DllImport("__Internal")]
        private static extern void LeaveAdminVoiceChat();

        [DllImport("__Internal")]
        private static extern void MuteMicrophone();

        [DllImport("__Internal")]
        private static extern void UnmuteMicrophone();

        [DllImport("__Internal")]
        private static extern void MuteMicrophoneInAdminChat();

        [DllImport("__Internal")]
        private static extern void UnmuteMicrophoneInAdminChat();

        [DllImport("__Internal")]
        private static extern void SetAdminChatLocalVolume(int volume);

        [DllImport("__Internal")]
        private static extern void SetAdminChatVolumeForAllUsers(int volume);

        [DllImport("__Internal")]
        private static extern void SetRemoteUserVolume(int uid, int volume);

        [DllImport("__Internal")]
        private static extern void JoinVoiceChat(string appid, string channel, string token, int uid);

        [DllImport("__Internal")]
        private static extern void JoinAdminVoiceChat(string appid, string channel, string token, int uid);

        [DllImport("__Internal")]
        private static extern void EnableVolumeIndicator();

        #endregion

        public bool IsVoiceChatUsageAllowed { get; set; } = false;

        public UnityEvent OnJoinedGeneralVoiceChat { get; } = new UnityEvent();
        public UnityEvent OnJoinedAdminVoiceChat { get; } = new UnityEvent();
        public UnityEvent<PlayersVolume> OnPlayerVolumesReceived { get; } = new UnityEvent<PlayersVolume>();

        public void LeaveChannel()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            LeaveVoiceChat();
        }

        public void LeaveAdminChannel()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            LeaveAdminVoiceChat();
        }

        public void MuteInGeneralChannel()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            if (!IsVoiceChatUsageAllowed)
            {
                return;
            }

            MuteMicrophone();
        }

        public void UnmuteInGeneralChannel()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            if (!IsVoiceChatUsageAllowed)
            {
                return;
            }

            UnmuteMicrophone();
        }

        public void MuteInAdminChannel()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            if (!IsVoiceChatUsageAllowed)
            {
                return;
            }

            MuteMicrophoneInAdminChat();
        }

        public void UnmuteInAdminChannel()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            if (!IsVoiceChatUsageAllowed)
            {
                return;
            }

            UnmuteMicrophoneInAdminChat();
        }

        /// <summary>
        /// Level of how other players hear you in admin voice chat. Volume level can be from 0 to 1000
        /// </summary>
        /// <param name="volume"></param>
        public void SetAdminChannelLocalVolume(int volume)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            if (!IsVoiceChatUsageAllowed)
            {
                return;
            }

            SetAdminChatLocalVolume(volume);
        }

        public void SetAdminVoiceChatVolumeForAllUsers(int volume)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            if (!IsVoiceChatUsageAllowed)
            {
                return;
            }

            SetAdminChatVolumeForAllUsers(volume);
        }

        public void SetRemoteUserVoiceVolume(int userId, int volume)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            if (!IsVoiceChatUsageAllowed)
            {
                return;
            }

            SetRemoteUserVolume(userId, volume);
        }

        public void JoinChannel(int playerUid, string channelName)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            Debug.LogWarning("WebGLChat work only in WebGL build");
            return;
#endif

            IsVoiceChatUsageAllowed = false;
            Debug.Log("Voice chat usage is blocked");

            AccessToken accessToken = new AccessToken(AgoraAppId, AppCertificate, channelName, "0");
            string token = accessToken.build();

            JoinVoiceChat(AgoraAppId, channelName, token, playerUid);
        }

        public void JoinAdminChannel(int playerUid, string channelName)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            AccessToken accessToken = new AccessToken(AgoraAppId, AppCertificate, channelName, "0");
            string token = accessToken.build();

            JoinAdminVoiceChat(AgoraAppId, channelName, token, playerUid);
        }

        public void EnablePlayersVolumeIndicator()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#endif

            EnableVolumeIndicator();
        }

        public void JoinGeneralVoiceChat()
        {
            OnJoinedGeneralVoiceChat?.Invoke();
        }

        public void JoinAdminVoiceChat()
        {
            OnJoinedAdminVoiceChat?.Invoke();
        }

        public void ReceivePlayerVolumes(PlayersVolume playersVolume)
        {
            OnPlayerVolumesReceived?.Invoke(playersVolume);
        }
    }
}
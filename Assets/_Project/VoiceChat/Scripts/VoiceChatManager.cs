using VoiceChat.WebGL;
using Photon.Pun;
using Core.ServiceLocator;

namespace VoiceChat
{
    public sealed class VoiceChatManager : MonoBehaviourPunCallbacks
    {
        private const string AdminVoiceChat = "GodVoiceChat";

        private string _roomName;
        private int _localPlayerUid;

        private bool _isJoiningVoiceChannelAllowed = true;


        #region Services

        private IWebVoiceChatAPIService _webVoiceChatAPIService;
        private IWebVoiceChatAPIService _webVoiceChatAPI
            => _webVoiceChatAPIService ??= Service.Instance.Get<IWebVoiceChatAPIService>();

        #endregion

        public override void OnJoinedRoom()
        {
            JoinGeneralVoiceChat();
        }

        public override void OnLeftRoom()
        {
            _webVoiceChatAPI.LeaveChannel();
            _webVoiceChatAPI.LeaveAdminChannel();
        }

        private void JoinGeneralVoiceChat()
        {
            if (!_isJoiningVoiceChannelAllowed)
            {
                return;
            }

            _isJoiningVoiceChannelAllowed = false;

            _roomName = PhotonNetwork.CurrentRoom.Name;
            _localPlayerUid = PhotonNetwork.LocalPlayer.ActorNumber;

            _webVoiceChatAPI.JoinChannel(_localPlayerUid, _roomName);
            SubscribeToVoiceChatConnections();
        }

        private void SubscribeToVoiceChatConnections()
        {
            _webVoiceChatAPI.OnJoinedGeneralVoiceChat.AddListener(JoinedGeneralVoiceChat);
            _webVoiceChatAPI.OnJoinedAdminVoiceChat.AddListener(JoinedAdminVoiceChat);
        }

        private void UnsubscribeFromVoiceChatConnections()
        {
            _webVoiceChatAPI.OnJoinedGeneralVoiceChat.RemoveListener(JoinedGeneralVoiceChat);
            _webVoiceChatAPI.OnJoinedAdminVoiceChat.RemoveListener(JoinedAdminVoiceChat);
        }

        private void JoinedGeneralVoiceChat()
        {
            _webVoiceChatAPI.JoinAdminChannel(_localPlayerUid, $"{_roomName}{AdminVoiceChat}");
        }

        private void JoinedAdminVoiceChat()
        {
            EnablePlayersVolumeIndicator();
            UnsubscribeFromVoiceChatConnections();

            _isJoiningVoiceChannelAllowed = true;
        }

        private void EnablePlayersVolumeIndicator()
        {
            _webVoiceChatAPI.EnablePlayersVolumeIndicator();
        }
    }
}
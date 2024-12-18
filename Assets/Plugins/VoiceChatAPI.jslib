mergeInto(LibraryManager.library, {
	
  
  JoinVoiceChat: function (appid, channel, token, uid) {
  	AgoraJoinVoiceChat(UTF8ToString(appid), UTF8ToString(channel), UTF8ToString(token), uid);
  },

  LeaveVoiceChat: function () {
    AgoraLeaveVoiceChat();
  },

  MuteMicrophone: function() {
    AgoraMute();
  },

  UnmuteMicrophone: function() {
    AgoraUnmute();
  },

  EnableVolumeIndicator: function() {
    AgoraEnableVolumeIndicator();
  },

  GetPlayerVolume: function (playerUid) {
    return AgoraGetPlayerVolume(playerUid);
  },    

  SetVolumeForAllUsers: function (volume) {
    AgoraSetVolumeForAllUsers(volume);
  },

  SetRemoteUserVolume: function (uid, volume) {
    AgoraSetRemoteUserVolume(uid, volume);
  },

  JoinAdminVoiceChat: function (appid, channel, token, uid) {
    AgoraJoinAdminVoiceChat(UTF8ToString(appid), UTF8ToString(channel), UTF8ToString(token), uid);
  },

  LeaveAdminVoiceChat: function () {
    AgoraLeaveAdminVoiceChat();
  },

  MuteMicrophoneInAdminChat: function() {
    AgoraMuteAdminChat();
  },

  UnmuteMicrophoneInAdminChat: function() {
    AgoraUnmuteAdminChat();
  },

  SetAdminChatLocalVolume: function(volume) {
    AgoraSetAdminChatLocalVolume(volume);
  },

  SetAdminChatVolumeForAllUsers: function (volume) {
    AgoraSetAdminChatVolumeForAllUsers(volume);
  }
});
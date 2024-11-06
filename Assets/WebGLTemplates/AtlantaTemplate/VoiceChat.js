var mainClient = null;
var adminChannelClient = null;
var myGameInstance = null;
var localAudioTrack = null;
var adminChatLocalAudioTrack = null;
var remoteUsers = {};
var adminChatRemoteUsers = {};
var remoteUsersTracksDictionary = {};
var adminChatRemoteUsersTracksDictionary = {};


// create Agora client
try {
  mainClient = AgoraRTC.createClient({ mode: "rtc", codec: "vp8" });
  adminChannelClient = AgoraRTC.createClient({ mode: "rtc", codec: "vp8" });
} catch (error) {
  console.error("Try to create AgoraRTC client: " + error);
}

async function AgoraJoinVoiceChat(appid, channel, token, uid) {
    if (mainClient == null) {
        return;
    }
    // add event listener to play remote tracks when remote user publishs.
    mainClient.on("user-published", handleUserPublished);
    mainClient.on("user-unpublished", handleUserUnpublished);

    await mainClient.join(appid, channel, token, uid);
    // Create a local audio track from the audio sampled by a microphone.
    localAudioTrack = await AgoraRTC.createMicrophoneAudioTrack();
    // Publish the local audio tracks to the RTC channel.
    await mainClient.publish([localAudioTrack]);

    AgoraMute();

    myGameInstance.SendMessage('WebVoiceChatAPI', 'JoinedGeneralVoiceChat');
}

async function AgoraLeaveVoiceChat() {
    if (localAudioTrack == null || mainClient == null) {
        return;
    }
    localAudioTrack.close();
    remoteUsers = {};
    remoteUsersTracksDictionary = {};
    await mainClient.leave();
}

async function handleUserPublished(user, mediaType) {
    const id = user.uid;
    remoteUsers[id] = user; 

    await mainClient.subscribe(user, mediaType);

    // If the remote user publishes an audio track.
    if (mediaType === "audio") 
    { 
        // Get the RemoteAudioTrack object in the AgoraRTCRemoteUser object.
        const remoteAudioTrack = user.audioTrack;
        // Play the remote audio track.
        remoteAudioTrack.play();        

        remoteUsersTracksDictionary[id] = remoteAudioTrack;
    }    
}

async function handleUserUnpublished(user) {
    const id = user.uid;
    delete remoteUsers[id];
    delete remoteUsersTracksDictionary[id];
    await mainClient.unsubscribe(user);
}

function AgoraMute(){
    if (localAudioTrack == null) {
        return;
    }
    localAudioTrack.setEnabled(false);
}

function AgoraUnmute(){
    if (localAudioTrack == null) {
        return;
    }
    localAudioTrack.setEnabled(true);
}

function AgoraEnableVolumeIndicator(){
    if (mainClient == null || myGameInstance == null) {
        return;
    }

    mainClient.enableAudioVolumeIndicator();
    mainClient.on("volume-indicator", volumes => {
       myGameInstance.SendMessage('WebVoiceChatAPI', 'SetVolumesValues', JSON.stringify({volumes}));
    })
}

function AgoraSetVolumeForAllUsers(volume) {
    Object.values(remoteUsersTracksDictionary).forEach(remoteTrack => {
        
        if (remoteTrack != null) {
            remoteTrack.setVolume(volume);
        }
    });    
}

function AgoraSetRemoteUserVolume(uid, volume) {
    const remoteTrack = remoteUsersTracksDictionary[uid];
    
    if (remoteTrack != null) {
        remoteTrack.setVolume(volume);
    }    
}

async function AgoraJoinAdminVoiceChat(appId, channel, token, uid) {
    if (adminChannelClient == null) {
        return;
    }
    // add event listener to play remote tracks when remote user publishs.
    adminChannelClient.on("user-published", handleAdminChatUserPublished);
    adminChannelClient.on("user-unpublished", handleAdminChatUserUnpublishedAdmin);

    await adminChannelClient.join(appId, channel, token, uid);
    // Create a local audio track from the audio sampled by a microphone.
    adminChatLocalAudioTrack = await AgoraRTC.createMicrophoneAudioTrack();
    // Publish the local audio tracks to the RTC channel.
    await adminChannelClient.publish([adminChatLocalAudioTrack]);

    AgoraMuteAdminChat();    

    myGameInstance.SendMessage('WebVoiceChatAPI', 'JoinedAdminVoiceChat');
}

async function AgoraLeaveAdminVoiceChat() {
    if (adminChatLocalAudioTrack == null || adminChannelClient == null) {
        return;
    }
    adminChatLocalAudioTrack.close();
    adminChatRemoteUsers = {};
    adminChatRemoteUsersTracksDictionary = {};
    await adminChannelClient.leave();
}

async function handleAdminChatUserPublished(user, mediaType) {
    const id = user.uid;
    adminChatRemoteUsers[id] = user; 

    await adminChannelClient.subscribe(user, mediaType);

    // If the remote user publishes an audio track.
    if (mediaType === "audio") 
    { 
        // Get the RemoteAudioTrack object in the AgoraRTCRemoteUser object.
        const remoteAudioTrack = user.audioTrack;
        // Play the remote audio track.
        remoteAudioTrack.play();        

        adminChatRemoteUsersTracksDictionary[id] = remoteAudioTrack;        
    }    
}

async function handleAdminChatUserUnpublishedAdmin(user) {
    const id = user.uid;
    delete adminChatRemoteUsers[id];
    delete adminChatRemoteUsersTracksDictionary[id];
    await adminChannelClient.unsubscribe(user);
}

function AgoraMuteAdminChat(){
    if (adminChatLocalAudioTrack == null) {
        return;
    }
    adminChatLocalAudioTrack.setEnabled(false);
}

function AgoraUnmuteAdminChat(){
    if (adminChatLocalAudioTrack == null) {
        return;
    }
    adminChatLocalAudioTrack.setEnabled(true);
}

function AgoraSetAdminChatLocalVolume(volume) {
    if (adminChatLocalAudioTrack == null) {
        return;
    }

    adminChatLocalAudioTrack.setVolume(volume);
}

function AgoraSetAdminChatVolumeForAllUsers(volume) {
    Object.values(adminChatRemoteUsersTracksDictionary).forEach(remoteTrack => {
        
        if (remoteTrack != null) {
            remoteTrack.setVolume(volume);
        }
    });    
}

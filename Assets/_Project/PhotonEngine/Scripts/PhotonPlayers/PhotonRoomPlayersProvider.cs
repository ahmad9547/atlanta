using Photon.Pun;
using Photon.Realtime;
using Core.ServiceLocator;

namespace PhotonEngine.PhotonPlayers
{
    public class PhotonRoomPlayersProvider : MonoBehaviourPunCallbacks
    {
        #region Services

        private IPhotonRoomPlayersService _photonRoomPlayersInstance;
        private IPhotonRoomPlayersService _photonRoomPlayers
            => _photonRoomPlayersInstance ??= Service.Instance.Get<IPhotonRoomPlayersService>();

        #endregion

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            _photonRoomPlayers.PhotonPlayersObservers.ForEach(observer => observer.UpdatePlayers());
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            base.OnPlayerEnteredRoom(player);
            _photonRoomPlayers.PhotonPlayersObservers.ForEach(observer => observer.UpdatePlayers());
            _photonRoomPlayers.NewPhotonPlayerObservers.ForEach(observer => observer.NewPlayerEntered(player));
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            base.OnPlayerLeftRoom(player);
            _photonRoomPlayers.PhotonPlayersObservers.ForEach(observer => observer.UpdatePlayers());
            _photonRoomPlayers.PhotonPlayerLeaveObservers.ForEach(observer => observer.PlayerLeftRoom(player));
        }
    }
}
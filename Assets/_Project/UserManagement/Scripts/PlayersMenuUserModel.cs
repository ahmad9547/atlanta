using Photon.Realtime;

namespace UserManagement
{
	[System.Serializable]
	public class PlayersMenuUserModel : UserModel
	{
		public int ActorNumber;
		public Player PhotonPlayer;
	}
}

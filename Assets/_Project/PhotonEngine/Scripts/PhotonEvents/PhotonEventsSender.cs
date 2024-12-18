using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PhotonEngine.PhotonEvents.Enums;

namespace PhotonEngine.PhotonEvents
{
    public class PhotonEventsSender : IPhotonEventsSenderService
    {
        /// <summary>
        /// Send photon event for special players by Player.ActorNumber parameter
        /// </summary>
        /// <param name="code">code (type) of photon event for sending</param>
        /// <param name="targetActorsID">array of Player.ActorNumbers parameters</param>
        /// <param name="content">content for sending</param>
        public void SendPhotonEvent(PhotonEventCode code, int[] targetActorsID, object content = null)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = targetActorsID };
            PhotonNetwork.RaiseEvent((byte)code, content, raiseEventOptions, SendOptions.SendReliable);
        }

        /// <summary>
        /// Send photon event for one special player by Player.ActorNumber parameter
        /// </summary>
        /// <param name="code">code (type) of photon event for sending</param>
        /// <param name="targetActorID">player actor number</param>
        /// <param name="content">content for sending</param>
        public void SendPhotonEvent(PhotonEventCode code, int targetActorID, object content = null)
        {
            int[] targetActors = new int[] { targetActorID };
            SendPhotonEvent(code, targetActors, content);
        }

        /// <summary>
        /// Send photon event for defined group of players
        /// </summary>
        /// <param name="code">code (type) of photon event for sending</param>
        /// <param name="receiverGroup">ReceiverGroup.All means to all players in room including local client. ReceiverGroup.Other means anyone in room except local client</param>
        /// <param name="content">content for sending</param>
        public void SendPhotonEvent(PhotonEventCode code, ReceiverGroup receiverGroup, object content = null)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = receiverGroup };
            PhotonNetwork.RaiseEvent((byte)code, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
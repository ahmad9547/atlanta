using System.Collections.Generic;
using Core.ServiceLocator;
using Photon.Realtime;
using PhotonEngine.PhotonPlayers.Interfaces;
using UserManagement;

namespace PhotonEngine.PhotonPlayers
{
    public interface IPhotonRoomPlayersService : IService
    {
        List<IPhotonPlayersObserver> PhotonPlayersObservers { get; }
        List<INewPhotonPlayerObserver> NewPhotonPlayerObservers { get; }
        List<IPhotonPlayerLeaveObserver> PhotonPlayerLeaveObservers { get; }

        void AddPlayersObserver(IPhotonPlayersObserver observer);
        void RemovePlayersObserver(IPhotonPlayersObserver observer);
        List<PlayersMenuUserModel> GetPlayersModels();
        int NumberOfPlayers();
        List<Player> GetPlayers();

        void AddNewPlayerObserver(INewPhotonPlayerObserver observer);
        void RemoveNewPlayerObserver(INewPhotonPlayerObserver observer);
        void AddPlayerLeaveObserver(IPhotonPlayerLeaveObserver observer);
        void RemovePlayerLeaveObserver(IPhotonPlayerLeaveObserver observer);
    }
}
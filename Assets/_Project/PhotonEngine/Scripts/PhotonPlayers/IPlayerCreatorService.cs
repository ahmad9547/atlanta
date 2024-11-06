using Core.ServiceLocator;
using UnityEngine;

namespace PhotonEngine.PhotonPlayers
{
    public interface IPlayerCreatorService : IService
    {
        delegate GameObject CreatePlayerHandler();
        CreatePlayerHandler CreatePlayerEvent { get; set; }

        GameObject CreatePlayer();
    }
}
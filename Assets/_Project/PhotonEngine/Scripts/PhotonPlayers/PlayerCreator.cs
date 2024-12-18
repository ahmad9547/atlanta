using UnityEngine;

namespace PhotonEngine.PhotonPlayers
{
    public class PlayerCreator : IPlayerCreatorService
    {
        public IPlayerCreatorService.CreatePlayerHandler CreatePlayerEvent { get; set; }

        public GameObject CreatePlayer()
        {
            return CreatePlayerEvent?.Invoke();
        }
    }
}
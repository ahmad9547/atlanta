using Core.ServiceLocator;
using UnityEngine;

namespace Metaverse.Services
{
    public interface ILocalPlayerService : IService
    {
        GameObject LocalPlayer { get; set; }

        bool IsOtherPlayerEqualsLocalPlayer(GameObject otherPlayer);
    }
}
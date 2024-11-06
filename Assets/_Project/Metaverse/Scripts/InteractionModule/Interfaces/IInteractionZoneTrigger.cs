using UnityEngine;

namespace Metaverse.InteractionModule.Interfaces
{
    public interface IInteractionZoneTrigger
    {
        void OnInteractionZoneTriggerEnter(GameObject player);

        void OnInteractionZoneTriggerExit(GameObject player);
    }
}
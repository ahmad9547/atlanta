using UnityEngine;
using UnityEngine.Events;
using Core.Physics;

namespace Metaverse.Teleport.CityGates
{
    public sealed class CityGateTeleportTriggerZone : MonoBehaviour
    {
        private const string PlayerTag = "Player";

        [HideInInspector] public UnityEvent<GameObject> OnEnter;
        [HideInInspector] public UnityEvent<GameObject> OnExit;

        private int _amountOfPlayersInZone;

        private void OnTriggerEnter(Collider other)
        {
            if (!IsPlayer(other))
            {
                return;
            }

            _amountOfPlayersInZone++;
            OnEnter?.Invoke(other.gameObject);

            ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsPlayer(other))
            {
                return;
            }

            _amountOfPlayersInZone = _amountOfPlayersInZone <= 0 ? 0 : _amountOfPlayersInZone - 1;
            OnExit?.Invoke(other.gameObject);

            ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
        }

        public bool IsSomebodyElseInZone()
        {
            return _amountOfPlayersInZone > 1;
        }

        public bool IsNobodyInZone()
        {
            return _amountOfPlayersInZone <= 0;
        }

        private bool IsPlayer(Collider other)
        {
            return other.gameObject.CompareTag(PlayerTag);
        }
    }
}

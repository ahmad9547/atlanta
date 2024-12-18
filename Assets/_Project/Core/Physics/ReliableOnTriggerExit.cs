using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// OnTriggerExit is not called if the triggering object is destroyed, set inactive, or if the collider is disabled. This script fixes that
//
// Usage: Wherever you read OnTriggerEnter() and want to consistently get OnTriggerExit
// In OnTriggerEnter() call ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
// In OnTriggerExit call ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
//
// Algorithm: Each ReliableOnTriggerExit is associated with a collider, which is added in OnTriggerEnter via NotifyTriggerEnter
// Each ReliableOnTriggerExit keeps track of OnTriggerEnter calls
// If ReliableOnTriggerExit is disabled or the collider is not enabled, call all pending OnTriggerExit calls
namespace Core.Physics
{
    public sealed class ReliableOnTriggerExit : MonoBehaviour
    {
        public delegate void OnTriggerExit(Collider collider);

        private Collider _thisCollider;
        private bool _ignoreNotifyTriggerExit = false;

        // Target callback
        private Dictionary<GameObject, OnTriggerExit> _waitingForOnTriggerExit = new Dictionary<GameObject, OnTriggerExit>();

        private void Update()
        {
            UpdateCallbacks();
        }

        private void OnDisable()
        {
            if (!gameObject.activeInHierarchy)
            {
                CallCallbacks();
            }
        }

        public static void NotifyTriggerEnter(Collider collider, GameObject caller, OnTriggerExit onTriggerExit)
        {
            ReliableOnTriggerExit thisComponent = GetThisComponent(collider);

            if (thisComponent == null)
            {
                thisComponent = collider.gameObject.AddComponent<ReliableOnTriggerExit>();
                thisComponent._thisCollider = collider;
            }

            // Unity bug? (!!!!): Removing a Rigidbody while the collider is in contact will call OnTriggerEnter twice, so I need to check to make sure it isn't in the list twice
            // In addition, force a call to NotifyTriggerExit so the number of calls to OnTriggerEnter and OnTriggerExit match up
            if (!thisComponent._waitingForOnTriggerExit.ContainsKey(caller))
            {
                thisComponent._waitingForOnTriggerExit.Add(caller, onTriggerExit);
                thisComponent.enabled = true;

                return;
            }

            thisComponent._ignoreNotifyTriggerExit = true;
            thisComponent._waitingForOnTriggerExit[caller].Invoke(collider);
            thisComponent._ignoreNotifyTriggerExit = false;
        }

        public static void NotifyTriggerExit(Collider collider, GameObject caller)
        {
            if (collider == null)
            {
                return;
            }

            ReliableOnTriggerExit thisComponent = GetThisComponent(collider);

            if (thisComponent == null || thisComponent._ignoreNotifyTriggerExit)
            {
                return;
            }

            thisComponent._waitingForOnTriggerExit.Remove(caller);

            if (thisComponent._waitingForOnTriggerExit.Count == 0)
            {
                thisComponent.enabled = false;
            }
        }

        private static ReliableOnTriggerExit GetThisComponent(Collider collider)
        {
            return collider.gameObject
                .GetComponents<ReliableOnTriggerExit>()
                .FirstOrDefault(components => components._thisCollider == collider);
        }

        private void UpdateCallbacks()
        {
            if (_thisCollider == null)
            {
                // Will GetOnTriggerExit with null, but is better than no call at all
                CallCallbacks();

                Destroy(this);
            }
            else if (!_thisCollider.enabled)
            {
                CallCallbacks();
            }
        }

        private void CallCallbacks()
        {
            _ignoreNotifyTriggerExit = true;

            foreach (OnTriggerExit callback in _waitingForOnTriggerExit
                .Where(v => v.Key != null)
                .Select(v => v.Value))
            {
                callback.Invoke(_thisCollider);
            }

            _ignoreNotifyTriggerExit = false;
            _waitingForOnTriggerExit.Clear();
            enabled = false;
        }
    }
}
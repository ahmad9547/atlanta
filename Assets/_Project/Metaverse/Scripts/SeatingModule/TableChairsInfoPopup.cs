using UnityEngine;

namespace Metaverse.SeatingModule
{
    public sealed class TableChairsInfoPopup : MonoBehaviour
    {
        private GameObject _targetFacingObject;

        private void Update()
        {
            RotateToTargetObject();
        }

        public void SetTargetFacingObject(GameObject targetFacingObject)
        {
            if (_targetFacingObject != null)
            {
                Debug.LogError("Target facing object already set");
                return;
            }

            _targetFacingObject = targetFacingObject;
        }

        public void RemoveTargetFacingObject()
        {
            if (_targetFacingObject == null)
            {
                Debug.LogError("Target facing object reference is null");
                return;
            }

            _targetFacingObject = null;
        }

        private void RotateToTargetObject()
        {
            if (_targetFacingObject == null)
            {
                return;
            }

            Vector3 rotation = Quaternion.LookRotation(transform.position - _targetFacingObject.transform.position).eulerAngles;
            rotation.x = 0f;
            rotation.z = 0f;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}

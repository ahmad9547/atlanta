using UnityEngine;
using Core.ServiceLocator;

namespace Metaverse.PresentationModule
{
    public sealed class SpeakerZone : MonoBehaviour
    {
        [Header("Debug:")]
        [SerializeField] private Color _speakerZoneColor;

        #region Services

        private ISpeakerZoneCheckerService _speakerZoneCheckerService;
        private ISpeakerZoneCheckerService _speakerZoneChecker
            => _speakerZoneCheckerService ??= Service.Instance.Get<ISpeakerZoneCheckerService>();

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            _speakerZoneChecker.SpeakerZoneEntered(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _speakerZoneChecker.SpeakerZoneExited(other);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _speakerZoneColor;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}
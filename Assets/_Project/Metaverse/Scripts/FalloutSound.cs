using Core.ServiceLocator;
using Metaverse.Services;
using UnityEngine;
using UnityEngine.Video;

namespace Metaverse
{
    public class FalloutSound : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _videoPlayer;

        [SerializeField] private Transform _audioSourcePoint;

        [SerializeField] private AnimationCurve _distanceInterpolation = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField] private float _minDistance = 50;
        [SerializeField] private float _maxDistance = 100;

        [SerializeField] private Color _debugSphereColor = new Color(0.5f, 0.5f, 1f, 1f);

        #region Services

        private ILocalPlayerService _localPlayerHolderInstance;
        private ILocalPlayerService _localPlayerHolder
            => _localPlayerHolderInstance ??= Service.Instance.Get<ILocalPlayerService>();

        #endregion

        private Vector3 _audioSourcePointPosition => _audioSourcePoint.position;


        private void Update()
        {
            if (_localPlayerHolder.LocalPlayer == null)
                return;

            float currentDistance = Vector3.Distance(_audioSourcePointPosition,
                _localPlayerHolder.LocalPlayer.transform.position);
            float relativeDistance =
                Mathf.Clamp01((currentDistance - _minDistance) / (_maxDistance - _minDistance));
            float currentVolume = _distanceInterpolation.Evaluate(1 - relativeDistance);
            _videoPlayer.SetDirectAudioVolume(0, currentVolume);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _debugSphereColor;
            Gizmos.DrawWireSphere(_audioSourcePointPosition, _minDistance);
            Gizmos.DrawWireSphere(_audioSourcePointPosition, _maxDistance);
        }
    }
}
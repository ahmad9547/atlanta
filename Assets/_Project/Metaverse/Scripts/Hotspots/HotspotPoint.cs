using UnityEngine;
using Metaverse.Services;
using Core.ServiceLocator;

namespace Metaverse.Hotspots
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class HotspotPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private AudioClip _hotspotClip;

        [Header("Debug:")]
        [SerializeField] private Color _hotspotPointColor;

        #region Services

        private ILocalPlayerService _localPlayerHolderInstance;
        private ILocalPlayerService _localPlayerHolder
            => _localPlayerHolderInstance ??= Service.Instance.Get<ILocalPlayerService>();

        #endregion

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            _content.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_localPlayerHolder.IsOtherPlayerEqualsLocalPlayer(other.gameObject))
            {
                return;
            }

            _audioSource.PlayOneShot(_hotspotClip);
            _content.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_localPlayerHolder.IsOtherPlayerEqualsLocalPlayer(other.gameObject))
            {
                return;
            }

            _content.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _hotspotPointColor;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}
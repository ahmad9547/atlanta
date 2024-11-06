using System.Collections;
using System.Collections.Generic;
using ProjectConfig.General;
using UnityEngine;

namespace Metaverse
{
    public sealed class AmphitheatreWatchers : MonoBehaviour
    {
        [SerializeField] private GameObject _avatarsParent;
        [SerializeField] private List<Animator> _watchersAnimators;

        private const float AnimatorsStartInterval = 0.5f;
        private const string SelectedAvatarTrigger = "Selected";

        private readonly int SelectedAvatarTriggerCached = Animator.StringToHash(SelectedAvatarTrigger);

        private Coroutine _watchersAnimatorsCoroutine;

        private void Start()
        {
            if (!GeneralSettings.IsAmphitheatreWatchersEnabled)
            {
                Destroy(gameObject);
                return;
            }

            _avatarsParent.SetActive(true);

            if (_watchersAnimatorsCoroutine != null)
            {
                StopCoroutine(_watchersAnimatorsCoroutine);
                _watchersAnimatorsCoroutine = null;
            }

            _watchersAnimatorsCoroutine = StartCoroutine(StartWatchersAnimators());
        }

        private IEnumerator StartWatchersAnimators()
        {
            foreach (Animator animator in _watchersAnimators)
            {
                animator.SetTrigger(SelectedAvatarTriggerCached);
                yield return new WaitForSeconds(AnimatorsStartInterval);
            }
        }

        private void OnDestroy()
        {
            if (_watchersAnimatorsCoroutine != null)
            {
                StopCoroutine(_watchersAnimatorsCoroutine);                
            }
        }
    }
}

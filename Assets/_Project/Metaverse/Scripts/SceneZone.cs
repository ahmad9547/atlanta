using System.Collections.Generic;
using UnityEngine;
using Core.ServiceLocator;
using UserManagement;

namespace Metaverse
{
    public sealed class SceneZone : MonoBehaviour
    {
        [SerializeField] private List<Collider> _sceneZoneColliders;

        #region Services

        private IUserProfileService _userProfileService;
        private IUserProfileService _userProfile
            => _userProfileService ??= Service.Instance.Get<IUserProfileService>();

        #endregion

        private void Start()
        {
            _sceneZoneColliders.ForEach(collider => collider.enabled = !_userProfile.IsAdmin);
        }
    }
}
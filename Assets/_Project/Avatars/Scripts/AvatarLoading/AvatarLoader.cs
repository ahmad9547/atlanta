using System;
using System.Diagnostics.Tracing;
using System.Linq;
using Avatars.Data;
using Avatars.Services;
using Avatars.WebGLMovement;
using Common.PlayerInput.Services;
using Core.ServiceLocator;
using LoadingScreenScene;
using Photon.Pun;
using PhotonEngine.CustomProperties;
using PhotonEngine.CustomProperties.Enums;
using PhotonEngine.PhotonPlayers;
using ProjectConfig.Session.Services;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UserManagement;

namespace Avatars.AvatarLoading
{
    public sealed class AvatarLoader : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private Animator _feminineAvatarAnimator;
        [SerializeField] private Animator _mascularAvatarAnimator;
        [SerializeField] private AvatarsDatabase _avatarsDatabase;
        private AvatarObjectLoader _avatarObjectLoader;
        private PhotonAnimatorView _animatorView;
        public Controller controller;
        #region Services

        private IUserProfileService _userProfileService;
        private IUserProfileService _userProfile
            => _userProfileService ??= Service.Instance.Get<IUserProfileService>();

        private ILoadingScreenService _loadingScreenInstance;
        private ILoadingScreenService _loadingScreen
            => _loadingScreenInstance ??= Service.Instance.Get<ILoadingScreenService>();

        private IAvatarLoadingStatusHolderService _avatarLoadingStatusHolderInstance;
        private IAvatarLoadingStatusHolderService _avatarLoadingStatusHolder
            => _avatarLoadingStatusHolderInstance ??= Service.Instance.Get<IAvatarLoadingStatusHolderService>();

        private IPlayerCustomPropertiesService _playerCustomPropertiesService;
        private IPlayerCustomPropertiesService _playerCustomProperties
            => _playerCustomPropertiesService ??= Service.Instance.Get<IPlayerCustomPropertiesService>();

        private IPhotonRoomPlayersService _photonRoomPlayersInstance;
        private IPhotonRoomPlayersService _photonRoomPlayers
            => _photonRoomPlayersInstance ??= Service.Instance.Get<IPhotonRoomPlayersService>();

        private static ISessionConfigHandler _sessionConfigHandlerService;
        private static ISessionConfigHandler _sessionConfigHandler
            => _sessionConfigHandlerService ??= Service.Instance.Get<ISessionConfigHandler>();

        #endregion

        
        private void Awake()
        {
            _avatarObjectLoader = new AvatarObjectLoader();
            
        }

        public override void OnEnable()
        {
            _avatarObjectLoader.OnCompleted += OnAvatarLoaded;
            _avatarObjectLoader.OnFailed += OnAvatarLoadingFailed;
            if (_photonView.IsMine)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        //public override void OnLeftRoom()
        //{
        //    if (photonView.IsMine)
        //    {
        //        controller.loading?.SetActive(false);
        //        PhotonNetwork.Destroy(this.gameObject);
                
        //    }
        //}

        private void Start()
        {
            PhotonNetwork.AddCallbackTarget(this);
            if (SceneManager.GetActiveScene().buildIndex ==2)
            {
                TunnelSceneFlowManager g = FindObjectOfType<TunnelSceneFlowManager>();
                transform.position = g.pos;
            }
            LoadAvatar();
        }

        private void OnDisable()
        {
            _avatarObjectLoader.OnCompleted -= OnAvatarLoaded;
            _avatarObjectLoader.OnFailed -= OnAvatarLoadingFailed;
        }

        private void LoadAvatar()
        {
            LoadCachedAvatar(_avatarsDatabase.GetDefaultAvatarEntry());
            return;
            string avatarId = _photonView.IsMine ? _userProfile.AvatarLink : GetOtherPlayerAvatarId();

            if (String.IsNullOrEmpty(avatarId))
            {
                _sessionConfigHandler.AddErrorMessage("Avatar ID is empty. Loading Default avatar.");
                LoadPredefinedAvatar(_avatarsDatabase.GetDefaultAvatarEntry());
                return;
            }

            if (TryGetPredefinedAvatar(avatarId, out AvatarEntry avatarEntry))
            {
                LoadPredefinedAvatar(avatarEntry);
                return;
            }

            LoadUniqueAvatar(avatarId);
        }

        private void LoadPredefinedAvatar(AvatarEntry avatarEntry)
        {
            /*  if (CheckIfAvatarAlreadyLoaded(avatarEntry.AvatarLink))
              {
                  LoadCachedAvatar(avatarEntry);
                  return;
              }

              LoadUniqueAvatar(avatarEntry.AvatarLink);*/

            LoadCachedAvatar(avatarEntry);

        }

        private void LoadUniqueAvatar(string avatarId)
        {
            _avatarObjectLoader.LoadAvatar(avatarId);
        }

        private bool TryGetPredefinedAvatar(string avatarId, out AvatarEntry avatarEntry)
        {
            avatarEntry = _avatarsDatabase.GetAvatarEntryByLink(avatarId);
            return avatarEntry != null;
        }

        private void LoadCachedAvatar(AvatarEntry avatarEntry)
        {
            GameObject avatar = Instantiate(avatarEntry.AvatarPrefab);
            SetupAvatar(avatar.transform, avatarEntry.OutfitGender);
            controller.Start1();
        }

        private void OnAvatarLoaded(object sender, CompletionEventArgs eventArgs)
        {
            SetupAvatar(eventArgs.Avatar.transform, eventArgs.Metadata.OutfitGender);
            controller.Start1();
        }

        private void SetupAvatar(Transform avatarTransform, OutfitGender outfitGender)
        {
            Animator newAnimator = outfitGender == OutfitGender.Feminine
                ? _feminineAvatarAnimator
                : _mascularAvatarAnimator;

            Animator obsoleteAnimator = outfitGender == OutfitGender.Feminine
                ? _mascularAvatarAnimator
                : _feminineAvatarAnimator;

            Destroy(obsoleteAnimator.gameObject);
            _playerAnimator.SetupAnimator(newAnimator);

            avatarTransform.parent = newAnimator.transform;
            avatarTransform.localPosition = Vector3.zero;
            avatarTransform.localRotation = Quaternion.identity;

            newAnimator.gameObject.SetActive(true);

            SkinnedMeshRenderer skinnedMeshRenderer = avatarTransform.GetComponentInChildren<SkinnedMeshRenderer>();
            skinnedMeshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;

            if (_photonView.IsMine)
            {
                _avatarLoadingStatusHolder.AvatarLoaded();
                _loadingScreen.Hide();
                AvatarTransform = avatarTransform.gameObject;
            }

            
        }

        public static GameObject AvatarTransform;

        private void OnAvatarLoadingFailed(object sender, FailureEventArgs eventArgs)
        {
            _sessionConfigHandler.AddErrorMessage($"Avatar failed to load with error - {eventArgs.Message}. Setting default avatar.");
            LoadPredefinedAvatar(_avatarsDatabase.GetDefaultAvatarEntry());
        }

        private string GetOtherPlayerAvatarId()
        {
            return (string)_playerCustomProperties.GetSpecialPlayerCustomProperty(_photonView.Controller,
                PlayerCustomPropertyKey.AvatarLink);
        }

        private bool CheckIfAvatarAlreadyLoaded(string avatarId)
        {
            return _photonRoomPlayers.GetPlayersModels().Any(model => model.AvatarId == avatarId);
        }
    }
}
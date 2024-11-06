using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UserManagement;
using Core.UI;
using ReadyPlayerMe.Core;

namespace PlayerUIScene.SideMenu.UserItems
{
    public class UserItem : MonoBehaviour
    {
        private const float UserSpritePixelsPerUnity = 100f;

        [Header("Common UserItem settings")]
        [SerializeField]private TextMeshProUGUI _nicknameLabel;
        [SerializeField] private Image _userPictureImage;

        protected UIAnimator _uiAnimator = new UIAnimator();
        protected PlayersMenuUserModel _playerModel;

        private AvatarRenderLoader _renderLoader;
        private Vector2 _userSpritePivot = new Vector2(0.5f, 0.5f);

        public virtual void InitializeItem(PlayersMenuUserModel playerModel)
        {
            _playerModel = playerModel;
            SetUserPicture(playerModel);
            SetNicknameLabel(playerModel);
        }

        public virtual void UpdateItem() { }

        private void Awake()
        {
            _renderLoader = new AvatarRenderLoader();
        }

        protected virtual void OnEnable()
        {
            _renderLoader.OnCompleted += OnRenderLoaded;
            _renderLoader.OnFailed += OnRenderLoadingFailed;
        }

        protected virtual void OnDisable()
        {
            _renderLoader.OnCompleted -= OnRenderLoaded;
            _renderLoader.OnFailed -= OnRenderLoadingFailed;
        }

        private void SetNicknameLabel(PlayersMenuUserModel playerModel)
        {
            _nicknameLabel.text = playerModel.Nickname;
        }

        private void SetUserPicture(PlayersMenuUserModel playerModel)
        {
            _renderLoader.LoadRender(playerModel.AvatarId, AvatarRenderScene.FullbodyPortraitTransparent);
        }

        private void OnRenderLoaded(Texture2D texture)
        {
            Sprite sprite = Sprite.Create(texture,
                new Rect(0f, 0f, texture.width, texture.height), _userSpritePivot,
                UserSpritePixelsPerUnity);
            _userPictureImage.overrideSprite = sprite;
        }
        
        private void OnRenderLoadingFailed(FailureType failureType, string message)
        {
            Debug.LogWarning($"Avatar render failed to load with error - {failureType}. Setting default avatar render.");
        }
    }
}
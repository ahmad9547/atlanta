using Core.ServiceLocator;
using Metaverse.Banners.Services;
using ProjectConfig.Banners;
using UnityEngine;
using UnityEngine.UI;

namespace Metaverse.Banners
{
    public class Banner : MonoBehaviour
    {
        [SerializeField] private int _bannerId;
        [SerializeField] private BannerVideoPlayer _bannerVideoPlayer;
        [SerializeField] private RectTransform _bannerRectTransform;
        [SerializeField] private RawImage _permanentImage;
        [SerializeField] private RawImage _temporaryImage;
        [SerializeField] private GameObject _bannerParentObject;
        [SerializeField] private GameObject _permanentImageParent;
        [SerializeField] protected GameObject _temporaryImageParent;

        #region Services

        private IBannerContentLoaderService _bannerContentLoaderInstance;
        private IBannerContentLoaderService BannerContentLoader
            => _bannerContentLoaderInstance ??= Service.Instance.Get<IBannerContentLoaderService>();

        #endregion

        protected virtual void OnEnable()
        {
            BannerContentLoader.OnTexturesLoaded.AddListener(SetupBanner);
        }

        protected virtual void OnDisable()
        {
            BannerContentLoader.OnTexturesLoaded.RemoveListener(SetupBanner);
        }

        private void SetupBanner()
        {
            if (!TryFindBannerModel(out BannerModel bannerModel))
            {
                return;
            }

            if (!bannerModel.IsActive)
            {
                Destroy(_bannerParentObject);
                return;
            }

            SetupBannerVisual(bannerModel);
        }

        private bool TryFindBannerModel(out BannerModel bannerModel)
        {
            if (BannersConfig.BannersList.Count <= _bannerId)
            {
                Debug.LogError($"Banner index {_bannerId} is out of range for BannerUrls list in Banners file");
                bannerModel = null;
                return false;
            }

            bannerModel = BannersConfig.BannersList[_bannerId];
            return true;
        }

        private void SetupBannerVisual(BannerModel bannerModel)
        {
            SetupBannerResolution(bannerModel.BannerResolutionModel);
            SetupTemporaryImage();

            if (!bannerModel.IsPermanent)
            {
                _permanentImageParent.SetActive(false);
                return;
            }

            if (bannerModel.IsImage)
            {
                SetupPermanentImage(bannerModel.ImageId);
                return;
            }

            _bannerVideoPlayer.PrepareVideo(BannersConfig.VideosUrl[bannerModel.VideoId]);
        }

        private void SetupBannerResolution(BannerResolutionModel resolutionModel)
        {
            _bannerRectTransform.sizeDelta = new Vector2(resolutionModel.Width, resolutionModel.Height);
        }

        private void SetupPermanentImage(int imageId)
        {
            _permanentImage.texture = BannerContentLoader.BannerTextures[imageId];
        }

        private void SetupTemporaryImage()
        {
            _temporaryImage.texture = BannerContentLoader.TemporaryTexture;
        }
    }
}
using Core.FilesReading;
using Core.ServiceLocator;
using System;
using System.IO;
using System.Threading.Tasks;
using Core.JwtManagement.Services;
using Core.UrlParametersParsing;
using Metaverse.ErrorHandling;
using Metaverse.ErrorHandling.Services;
using ProjectConfig.AdminCredentials;
using ProjectConfig.Banners;
using ProjectConfig.General;
using ProjectConfig.Playlist;
using ProjectConfig.PresentationSlides;
using ProjectConfig.Session;
using ProjectConfig.Session.Services;
using ProjectConfig.Videos;
using UnityEngine;

namespace ProjectConfig
{
    public sealed class ConfigReader : MonoBehaviour
    {
        private const string GeneralSettingsFileName = "generalSettings.json";
        private const string AdminUsersFileName = "adminUsers.json";
        private const string BannersFileName = "banners.json";
        private const string PresentationSlidesFileName = "presentationSlides.json";
        private const string VideosFileName = "videos.json";
        private const string PlaylistFileName = "playlist.json";
        private const string SessionConfigFileName = "sessionConfig.json";
        private const string UrlParameterName = "hashcode";

        #region Services

        private IWebRequestsLoaderService _webRequestsLoaderInstance;
        private IWebRequestsLoaderService _webRequestsLoader
            => _webRequestsLoaderInstance ??= Service.Instance.Get<IWebRequestsLoaderService>();

        private ISessionConfigHandler _sessionConfigHandlerService;
        private ISessionConfigHandler _sessionConfigHandler
            => _sessionConfigHandlerService ??= Service.Instance.Get<ISessionConfigHandler>();

        private IUrlParameterParserService _urlParameterParserInstance;
        private IUrlParameterParserService _urlParameterParser
            => _urlParameterParserInstance ??= Service.Instance.Get<IUrlParameterParserService>();

        private IJwtDecoderService _jwtDecoderService;
        private IJwtDecoderService _jwtDecoder
            => _jwtDecoderService ??= Service.Instance.Get<IJwtDecoderService>();

        #endregion

        private void Start()
        {
            LoadGeneralSettings();
            LoadSessionConfig();
            LoadAdminUsersList();
            LoadBannersList();
            LoadPresentationSlidesList();
            LoadVideoList();
            LoadPlaylist();
        }

        private async Task<string> LoadJson(string fileName)
        {
            string url = Path.Combine(Application.streamingAssetsPath, fileName);

#if !UNITY_EDITOR
            return await _webRequestsLoader.DownloadFileText(url);
#else
            return await _webRequestsLoader.DownloadFileText(url, false);
#endif
        }

        private async void LoadGeneralSettings()
        {
            string generalSettingsJson = await LoadJson(GeneralSettingsFileName);

            try
            {
                GeneralSettingsModel generalSettingsModel = JsonUtility.FromJson<GeneralSettingsModel>(generalSettingsJson);
                GeneralSettings.SetupGeneralSettings(generalSettingsModel);

                Debug.Log("-----------------------------------------------");
                Debug.Log($"Current version: {generalSettingsModel.Version}");
                Debug.Log("-----------------------------------------------");
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error while deserializing GeneralSettingsModel: {exception.Message}");
            }
        }

        private async void LoadSessionConfig()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            string hashcode = _urlParameterParser.GetUrlParameter(UrlParameterName);
            string sessionJson = _jwtDecoder.DecodeToken(hashcode);
#else
            string sessionJson = await LoadJson(SessionConfigFileName);
#endif

            try
            {
                SessionConfigModel sessionConfigModel = JsonUtility.FromJson<SessionConfigModel>(sessionJson);
                SessionConfig.SetupSessionConfig(sessionConfigModel);
                _sessionConfigHandler.Initialize();
            }
            catch (Exception exception)
            {
                ErrorLogger.LogError(ErrorType.Error, $"Error while deserializing SessionConfigModel: {exception.Message}", GeneralSettings.GoBackOnErrorDefaultUrl);
            }
        }

        private async void LoadAdminUsersList()
        {
            string adminUsersJson = await LoadJson(AdminUsersFileName);

            try
            {
                AdminUsersModel adminUsersModel = JsonUtility.FromJson<AdminUsersModel>(adminUsersJson);
                AdminUsersConfig.SetupAdminUsers(adminUsersModel);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error while deserializing AdminUsersModel: {exception.Message}");
            }
        }

        private async void LoadBannersList()
        {
            string bannersJson = await LoadJson(BannersFileName);

            try
            {
                BannersConfigModel bannersModel = JsonUtility.FromJson<BannersConfigModel>(bannersJson);
                BannersConfig.SetupBanners(bannersModel);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error while deserializing BannersConfigModel: {exception.Message}");
            }
        }

        private async void LoadPresentationSlidesList()
        {
            string presentationSlidesJson = await LoadJson(PresentationSlidesFileName);

            try
            {
                PresentationSlidesModel presentationSlidesModel = JsonUtility.FromJson<PresentationSlidesModel>(presentationSlidesJson);
                PresentationSlidesConfig.SetupPresentationSlides(presentationSlidesModel);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error while deserializing PresentationSlidesModel: {exception.Message}");
            }
        }

        private async void LoadVideoList()
        {
            string videosJson = await LoadJson(VideosFileName);

            try
            {
                VideosModel videosModel = JsonUtility.FromJson<VideosModel>(videosJson);
                VideosConfig.SetupVideos(videosModel);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error while deserializing VideosModel: {exception.Message}");
            }
        }

        private async void LoadPlaylist()
        {
            string playlistJson = await LoadJson(PlaylistFileName);

            try
            {
                PlaylistConfigModel playlistConfigModel = JsonUtility.FromJson<PlaylistConfigModel>(playlistJson);
                PlaylistConfig.SetupPlaylist(playlistConfigModel);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error while deserializing PlaylistConfigModel: {exception.Message}");
            }
        }
    }
}
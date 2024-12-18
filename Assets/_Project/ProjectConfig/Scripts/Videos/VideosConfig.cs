using System.IO;
using ProjectConfig.General;

namespace ProjectConfig.Videos
{
    public static class VideosConfig
    {
        private static string _visitCenterVideoUrl;

        public static string VisitCenterVideoUrl => _visitCenterVideoUrl;

        public static void SetupVideos(VideosModel videosModel)
        {
            _visitCenterVideoUrl = Path.Combine(GeneralSettings.ContentFolderUrl, videosModel.VisitCenterVideoUrl);
        }
    }
}
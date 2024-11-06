using Metaverse;
using ProjectConfig.Videos;
using UnityEngine;

namespace _Project.Metaverse
{
    public class MetaverseVideos : MonoBehaviour
    {
        [SerializeField] private TVVideoPlayer _visitCenterVideoPlayer;

        private void Start()
        {
            StartMetaverseVideos();
        }

        private void StartMetaverseVideos()
        {
            _visitCenterVideoPlayer.StartVideo(VideosConfig.VisitCenterVideoUrl);
        }
    }
}

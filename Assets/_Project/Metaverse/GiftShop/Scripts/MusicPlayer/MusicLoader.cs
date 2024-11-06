using Core.ServiceLocator;
using Metaverse.GiftShop.Interfaces;
using UnityEngine;

namespace Metaverse.GiftShop.MusicPlayer
{
    public sealed class MusicLoader : MonoBehaviour
    {
        #region Services

        private IMusicPlayerEventHandler _musicPlayerEventHandlerInstance;
        private IMusicPlayerEventHandler _musicPlayerEventHandler
            => _musicPlayerEventHandlerInstance ??= Service.Instance.Get<IMusicPlayerEventHandler>();

        #endregion

        private void Start()
        {
            _musicPlayerEventHandler.LoadPlaylist();
        }
    }
}
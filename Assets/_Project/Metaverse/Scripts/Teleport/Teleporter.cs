using Core.ServiceLocator;
using Metaverse.Services;
using System.Collections;
using Metaverse.PlayersSettings;
using UnityEngine;

namespace Metaverse.Teleport
{
    public class Teleporter : MonoBehaviour
    {
        private const float PlayerTeleportCameraDampingDelay = 0.5f;

        #region Services

        private ILocalPlayerService _localPlayerHolderInstance;
        private ILocalPlayerService _localPlayerHolder
            => _localPlayerHolderInstance ??= Service.Instance.Get<ILocalPlayerService>();

        private IPlayerSettingsService _playerSettingsInstance;
        private IPlayerSettingsService _playerSettings
            => _playerSettingsInstance ??= Service.Instance.Get<IPlayerSettingsService>();

        #endregion

        private Coroutine _teleportCoroutine;

        protected void DoTeleport(Pose targetPose)
        {
            if (_teleportCoroutine != null)
            {
                StopCoroutine(_teleportCoroutine);
            }

            _playerSettings.DisableThirdPersonCameraDamping();
            _teleportCoroutine = StartCoroutine(TeleportCoroutine(_localPlayerHolder.LocalPlayer, targetPose));
        }

        private IEnumerator TeleportCoroutine(GameObject player, Pose targetPose)
        {
            yield return new WaitForEndOfFrame();

            player.transform.SetPositionAndRotation(targetPose.position, targetPose.rotation);

            _teleportCoroutine = null;

            yield return new WaitForSeconds(PlayerTeleportCameraDampingDelay);
            _playerSettings.SetThirdPersonCameraDefaultDamping();
        }
    }
}
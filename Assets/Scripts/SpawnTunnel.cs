using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace GrafittiController
{
    public class SpawnTunnel : MonoBehaviour
    {
        [SerializeField] public GameObject PlayerAvatar;
        [SerializeField] private Transform PlayerAvatarTeleportPosition;
        [SerializeField] private GameObject TrainingRoom;
        [SerializeField] private AssetReference AssetReference;
        [SerializeField] private Slider ProgressBar;
        [SerializeField] private CanvasGroup ProgressBarGroup;
        //[SerializeField] private MeshTextureDetection MeshTextureDetection;
        [SerializeField] private TextMeshProUGUI TunnelDownloadStatusText;
        [SerializeField] private TextMeshProUGUI TunnelDownloadPercentageText;
        [SerializeField] private GameObject TunnelTrigger;
        [SerializeField] private CameraSwitchInTunner CameraSwitcher;
        private void Start()
        {
            Addressables.LoadAssetAsync<GameObject>(AssetReference).Completed += OnTunnelReceived;
        }
        public void TeleportToTunnel()
        {
            TrainingRoom.SetActive(false);
            StartCoroutine(FadeOutProgressBar());
            // CameraSwitcher.SwitchToThirdPerson();
        }
        private void Update()
        {
            if (!Addressables.LoadAssetAsync<GameObject>(AssetReference).IsDone)
            {
                ProgressBar.value = Addressables.LoadAssetAsync<GameObject>(AssetReference).PercentComplete;
                TunnelDownloadPercentageText.text = $"{(int)(ProgressBar.value * 100f)}%";
            }
        }
        private void OnTunnelReceived(AsyncOperationHandle<GameObject> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Instantiate(handle.Result);
                //MeshTextureDetection.SetEnvironmentReference(handle.Result);
                TunnelDownloadStatusText.text = $"Tunnel Preparation Complete! Head to the door";
                TunnelDownloadPercentageText.text = $"{100}%";
                TunnelTrigger.SetActive(true);
            }
            else
            {
                Debug.Log($"Asset did not load");
            }
        }
        private IEnumerator FadeOutProgressBar()
        {
            float timeElapsed = 0;
            float timeForFade = 1f;
            while (timeElapsed < timeForFade)
            {
                ProgressBarGroup.alpha = Mathf.Lerp(1f, 0f, timeElapsed / timeForFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            ProgressBarGroup.alpha = 0f;
        }
    }
}
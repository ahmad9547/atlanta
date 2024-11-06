using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LoadTunnel : MonoBehaviour
{
	public static GameObject TunnelLoaded;

	[SerializeField] private AssetReference tunnelAsset;
	public GameObject tunnelTrigger;
	public Transform tunnelParent;
	public AutoRoomTest autoRoomTest;
	public PaintStart paintStart;
	public static bool isTunnelLoaded;
	public GameObject tunnelLoaded;
	public InstantiatePlayer instantiatePlayer;
	public UnityEngine.UI.Slider progressBar;
	public TextMeshProUGUI loadingText;
	private void Start()
	{
		//PhotonNetwork.SendRate = 1;
	}

    private void OnDestroy()
    {
        Addressables.LoadAssetAsync<GameObject>(tunnelAsset).Completed -= OnTunnelReceived;
    }

    private void Update()
    {
        if (!Addressables.LoadAssetAsync<GameObject>(tunnelAsset).IsDone)
        {
            progressBar.value = Addressables.LoadAssetAsync<GameObject>(tunnelAsset).PercentComplete;
            loadingText.text = $"{(int)(progressBar.value * 100f)}%";
        }
    }

    public void LoadTunnelCall()
	{
		if (isTunnelLoaded)
		{
			progressBar.value = 1;
			loadingText.text = "100%";
            LoadedTunnelCalls(TunnelLoaded);
			return;
		}
        Addressables.LoadAssetAsync<GameObject>(tunnelAsset).Completed += OnTunnelReceived;
    }

	private void OnTunnelReceived(AsyncOperationHandle<GameObject> handle)
	{
		if (handle.Status == AsyncOperationStatus.Succeeded)
		{
			TunnelAndDataLoaded(handle);
        }
		else
		{
			autoRoomTest.ShowError("Not able to load addressable Server error. Press Ok TO Reload Game");
			Debug.Log($"Asset did not load");
		}
	}

	public void InstantiateTunnel()
	{
        tunnelLoaded = Instantiate(tunnelLoaded, tunnelParent);
    }

	public void LoadedTunnelCalls(GameObject tunnel)
	{
        tunnelLoaded = tunnel;
        DataManager.Instance.tunnel = tunnelLoaded;
        TunnelLoaded = tunnelLoaded;
        isTunnelLoaded = true;
        autoRoomTest.isTunnelLoaded = true;
        tunnelTrigger.SetActive(true);
    }

	private void TunnelAndDataLoaded(AsyncOperationHandle<GameObject> handle)
	{
		LoadedTunnelCalls(handle.Result);
        //      if (DataManager.Instance.isJsonLoaded)
        //      {

        //	//DataManager.Instance.StartPainting();
        //      }
        //else
        //{
        //	this.Invoke(() => { TunnelAndDataLoaded(handle); }, 0.5f);
        //}
    }

	public void JsonLoaded()
	{

        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelLoaded : MonoBehaviour
{
	public GameObject spawnPos;

	private void Start()
	{
        this.Invoke(() => {
			TunnelSceneFlowManager g = FindObjectOfType<TunnelSceneFlowManager>();
			g.TunnelLoadedStartPainting(spawnPos.transform.position); 
		}, 1.0f);
	}
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToTunnel : Singleton<TeleportToTunnel>
{
	public bool isTeleported;
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		if (!other.GetComponent<PhotonView>().IsMine) return;
        DataManager.Instance.isPainter = !other.GetComponent<PlayerModeSwitch>().cameraSwitcher.isThirdPerson;
		//DataManager.Instance.isPainter = DataManager.Instance.controller.GetComponent<PlayerModeSwitch>().cameraSwitcher.isThirdPerson;
		DataManager.Instance.isTeleportedToTunnel = true;
		isTeleported = true;
		PhotonNetwork.Destroy(InstantiatePlayer.PlayerRef);
		DataManager.Instance.controller?.ShowCursor();
        SceneManager.LoadScene(2);
		//GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
		//InstantiatePlayer.PlayerRef.transform.position = spawnPoint.transform.position;
	}
}

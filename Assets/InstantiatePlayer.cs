using Common.PlayerInput;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class InstantiatePlayer : MonoBehaviour
{
    public static GameObject PlayerRef;

    private PlayerInput _playerInput;
    private PhotonView _photonViewOfPlayer;
    public GameObject loadingObject;
    public LoadTunnel loadTunnel;
    public bool isPlayerLoaded;

    public void InstantiatePlayerLocal()
    {
        var playerRef = PhotonNetwork.Instantiate("Avatar_New", transform.position, quaternion.identity, data: new object[]
        {
            DataManager.Instance.isPainter
        });
		PlayerRef = playerRef;
        _playerInput = playerRef.GetComponent<PlayerInput>();
        _photonViewOfPlayer = playerRef.GetComponent<PhotonView>();
        AvatarInit avatarInit = playerRef.GetComponent<AvatarInit>();
        avatarInit.loadTunnel = loadTunnel;
        Controller controller = avatarInit.controller;
        controller.loading = loadingObject;
    }

    public void DestroyPlayer()
    {
        Destroy(PlayerRef);
    }

    public void InstantiateToASpecificPostion(Vector3 pos)
    {
        if (isPlayerLoaded)
        {
            return;
        }
        isPlayerLoaded = true;
        var playerRef = PhotonNetwork.Instantiate("Avatar_New", pos, quaternion.identity, data: new object[]
        {
            DataManager.Instance.isPainter
        });
        PlayerRef = playerRef;
        _playerInput = playerRef.GetComponent<PlayerInput>();
        _photonViewOfPlayer = playerRef.GetComponent<PhotonView>();
        AvatarInit avatarInit = playerRef.GetComponent<AvatarInit>();
        avatarInit.loadTunnel = loadTunnel;
        Controller controller = avatarInit.controller;
        controller.loading = loadingObject;
        
    }

}
using TMPro;
using Photon.Pun;
using UnityEngine;

namespace Avatars.Player
{
    public sealed class PlayerNickname : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nicknameField = default;

        private PhotonView _photonView;

        private void Awake()
        {
            GetPhotonView();
        }

        private void Start()
        {           
            _nicknameField.text = _photonView.Owner.NickName;
        }

        private void GetPhotonView()
        {
            _photonView = GetComponentInParent<PhotonView>();

            if (_photonView == null)
            {
                Debug.LogError("Reference on PhotonView component in parent object is null");
            }
        }
    }
}

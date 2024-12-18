using UnityEngine;
using Core.UI;
using Core.ServiceLocator;
using TMPro;
using PhotonEngine.PhotonPlayers;
using PhotonEngine.PhotonPlayers.Interfaces;

namespace PlayerUIScene.SideMenu.Controllers
{
    public sealed class VisitorsDisplay : UIController, IPhotonPlayersObserver
    {
        [SerializeField] private TextMeshProUGUI _visitorsText;

        private const string VisitorText = "Visitor";
        private const string VisitorsText = "Visitors";

        #region Services

        private IPhotonRoomPlayersService _photonRoomPlayersInstance;
        private IPhotonRoomPlayersService _photonRoomPlayers
            => _photonRoomPlayersInstance ??= Service.Instance.Get<IPhotonRoomPlayersService>();

        #endregion

        private void OnEnable()
        {
            _photonRoomPlayers.AddPlayersObserver(this);
        }

        protected override void Start()
        {
            UpdatePlayers();
            Show();
        }

        private void OnDisable()
        {
            _photonRoomPlayers.RemovePlayersObserver(this);
        }

        public void UpdatePlayers()
        {
            int numberOfPlayers = _photonRoomPlayers.NumberOfPlayers();
            _visitorsText.text = numberOfPlayers == 1 ? $"{numberOfPlayers} {VisitorText}" : $"{numberOfPlayers} {VisitorsText}";
        }
    }
}
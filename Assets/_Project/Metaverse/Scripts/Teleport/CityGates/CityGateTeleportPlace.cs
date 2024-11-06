using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Metaverse.Services;
using Core.ServiceLocator;
using Core.Extensions;
using Metaverse.Teleport.Interfaces;

namespace Metaverse.Teleport.CityGates
{
    public sealed class CityGateTeleportPlace : MonoBehaviour
    {
        private const int NumberOfClosestLocationsToShow = 4;
        private const float TriggerZonesActivationDelay = 3f;

        [SerializeField] private TeleportPointType _teleportPoint;

        [SerializeField] private CityGatePortalEffect _cityGatePortalEffect;
        [SerializeField] private CityGateUI _cityGateUI;

        [Header("Trigger zones")]
        [SerializeField] private Transform _triggerZonesParent;
        [SerializeField] private CityGateTeleportTriggerZone _firstTriggerZone;
        [SerializeField] private CityGateTeleportTriggerZone _secondTriggerZone;
        [SerializeField] private CityGateTeleportTriggerZone _thirdTriggerZone;

        #region Services

        private ILocalPlayerService _localPlayerHolderInstance;
        private ILocalPlayerService _localPlayerHolder 
            => _localPlayerHolderInstance ??= Service.Instance.Get<ILocalPlayerService>();

        private ITeleportControllerService _cityGatesTeleportControllerInstance;
        private ITeleportControllerService _cityGatesTeleportController
            => _cityGatesTeleportControllerInstance ??= Service.Instance.Get<ITeleportControllerService>();

        #endregion

        private List<TeleportPoint> _otherCityGatesPoints = new List<TeleportPoint>();
        private List<TeleportPoint> _closestLocationPoints = new List<TeleportPoint>();
        private List<TeleportPoint> _allAvailablePoints = new List<TeleportPoint>();

        private TeleportPoint _selectedTeleportPoint;

        private Coroutine _triggerZonesActivationCoroutine;

        private void Awake()
        {
            _cityGateUI.HideGateName();
            _cityGateUI.HidePopup();
        }

        private void OnEnable()
        {
            SubscribeOnTriggerZones();
        }

        private void Start()
        {
            GetOtherCityGatePoints();
            GetClosestLocationPoints();
            GetAllAvailableTeleportPoints();
            _cityGateUI.FillPopupWithTeleportPoints(_allAvailablePoints);
            ActivateTriggerZones();
        }

        private void OnDisable()
        {
            UnsubscribeFromTriggerZones();
        }

        public void SelectTargetTeleportPoint(TeleportPoint teleportPoint)
        {
            _selectedTeleportPoint = teleportPoint;
        }

        private void SubscribeOnTriggerZones()
        {
            _firstTriggerZone.OnEnter.AddListener(OnFirstTriggerZoneEnter);
            _firstTriggerZone.OnExit.AddListener(OnFirstTriggerZoneExit);
            _secondTriggerZone.OnEnter.AddListener(OnSecondTriggerZoneEnter);
            _secondTriggerZone.OnExit.AddListener(OnSecondTriggerZoneExit);
            _thirdTriggerZone.OnEnter.AddListener(OnThirdTriggerZoneEnter);
        }

        private void UnsubscribeFromTriggerZones()
        {
            _firstTriggerZone.OnEnter.RemoveListener(OnFirstTriggerZoneEnter);
            _firstTriggerZone.OnExit.RemoveListener(OnFirstTriggerZoneExit);
            _secondTriggerZone.OnEnter.RemoveListener(OnSecondTriggerZoneEnter);
            _secondTriggerZone.OnExit.RemoveListener(OnSecondTriggerZoneExit);
            _thirdTriggerZone.OnEnter.RemoveListener(OnThirdTriggerZoneEnter);
        }

        private void OnFirstTriggerZoneEnter(GameObject player)
        {
            if (_firstTriggerZone.IsSomebodyElseInZone())
            {
                return;
            }

            _cityGatePortalEffect.Show();
            _cityGateUI.ShowGateName();
        }

        private void OnFirstTriggerZoneExit(GameObject player)
        {
            if (!_firstTriggerZone.IsNobodyInZone())
            {
                return;
            }

            _cityGatePortalEffect.Hide();
            _cityGateUI.HideGateName();
        }

        private void OnSecondTriggerZoneEnter(GameObject player)
        {
            if (!IsPlayerLocal(player))
            {
                return;
            }

            _cityGateUI.ShowPopup();
            SelectRandomCityGatePoint();
        }

        private void OnSecondTriggerZoneExit(GameObject player)
        {
            if (!IsPlayerLocal(player))
            {
                return;
            }

            _cityGateUI.HidePopup();
            _selectedTeleportPoint = null;
        }

        private void OnThirdTriggerZoneEnter(GameObject player)
        {
            _cityGatesTeleportController.TeleportToPoint(_selectedTeleportPoint);
        }

        private void GetOtherCityGatePoints()
        {
            _otherCityGatesPoints = _cityGatesTeleportController.GetCityGatesTeleportPoints()
                .Where(point => point.TeleportPointType.PointType != _teleportPoint.PointType)
                .ToList();
        }

        private void GetClosestLocationPoints()
        {
            _closestLocationPoints = _cityGatesTeleportController.GetLocationTeleportPoints()
                .OrderBy(DistanceToTeleportPoint)
                .Take(NumberOfClosestLocationsToShow)
                .ToList();
        }

        private void GetAllAvailableTeleportPoints()
        {
            _allAvailablePoints = _otherCityGatesPoints
                .Concat(_closestLocationPoints)
                .ToList();
        }

        private float DistanceToTeleportPoint(TeleportPoint point)
        {
            return Vector3.Distance(transform.position, point.Position);
        }

        private bool IsPlayerLocal(GameObject player)
        {
            return _localPlayerHolder.IsOtherPlayerEqualsLocalPlayer(player);
        }

        private void SelectRandomCityGatePoint()
        {
            _selectedTeleportPoint = _otherCityGatesPoints.PickRandom();
        }

        private void ActivateTriggerZones()
        {
            if (_triggerZonesActivationCoroutine != null)
            {
                StopCoroutine(_triggerZonesActivationCoroutine);
                _triggerZonesActivationCoroutine = null;
            }

            _triggerZonesActivationCoroutine = StartCoroutine(ActivateTriggerZonesWithDelay());
        }

        private IEnumerator ActivateTriggerZonesWithDelay()
        {
            yield return new WaitForSeconds(TriggerZonesActivationDelay);
            _triggerZonesParent.gameObject.SetActive(true);
            _triggerZonesActivationCoroutine = null;
        }
    }
}
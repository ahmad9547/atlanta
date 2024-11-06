using Core.ServiceLocator;
using LocationsManagement.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace LocationsManagement
{
    public sealed class LocationPresetProvider : MonoBehaviour
    {
        [SerializeField] private List<Location> _locations;

        private ILocationsHolderService _locationsHolderService => Service.Instance.Get<ILocationsHolderService>();

        private void Start()
        {
            _locationsHolderService.SetLocations(_locations);
        }
    }
}

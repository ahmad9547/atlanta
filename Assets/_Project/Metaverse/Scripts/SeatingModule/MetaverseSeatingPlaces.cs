using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Metaverse.SeatingModule
{
    public sealed class MetaverseSeatingPlaces : MonoBehaviour
    {
        [SerializeField] private List<SeatingPlace> _seatingPlaces;

        [SerializeField] private MetaverseSeatingPlacesSynch _metaverseSeatingPlacesSynch;

        private void Awake()
        {
            InitializeSeatingPlaces();
        }

        public void SetSeatingPlaceOccupied(SeatingPlace seatingPlace)
        {
            _metaverseSeatingPlacesSynch.SendOccupiedSeatingPlaceId(seatingPlace.Id);            
        }

        public void SetSeatingPlaceUnoccupied(SeatingPlace seatingPlace)
        {
            _metaverseSeatingPlacesSynch.SendFreeSeatingPlaceId(seatingPlace.Id);            
        }

        public void SetSeatingPlaceFreeState(int seatingPlaceIndex, bool state)
        {
            _seatingPlaces[seatingPlaceIndex].IsFree = state;
        }        

        private void InitializeSeatingPlaces()
        {            
            foreach ((SeatingPlace seatingPlace, int index) in _seatingPlaces.Select((place, index) => (place, index)))
            {                
                seatingPlace.Initialize(this, index);
            }
        }
    }
}
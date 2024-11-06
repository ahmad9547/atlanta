using Core.ServiceLocator;
using Metaverse.PresentationModule.Interfaces;
using UnityEngine;

namespace Metaverse.PresentationModule
{
    public interface ISpeakerZoneCheckerService : IService
    {
        void AddSpeakerZoneObserver(ISpeakerZoneObserver observer);
        void RemoveSpeakerZoneObserver(ISpeakerZoneObserver observer);
        void SpeakerZoneEntered(Collider other);
        void SpeakerZoneExited(Collider other);
    }
}
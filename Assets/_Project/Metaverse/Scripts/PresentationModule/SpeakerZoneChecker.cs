using System.Collections.Generic;
using Core.ServiceLocator;
using Metaverse.PresentationModule.Interfaces;
using Metaverse.Services;
using UnityEngine;
using UserManagement;

namespace Metaverse.PresentationModule
{
    public class SpeakerZoneChecker : ISpeakerZoneCheckerService
    {
        private readonly List<ISpeakerZoneObserver> _observers = new List<ISpeakerZoneObserver>();

        #region Services

        private ILocalPlayerService _localPlayerHolderInstance;
        private ILocalPlayerService _localPlayerHolder
            => _localPlayerHolderInstance ??= Service.Instance.Get<ILocalPlayerService>();

        private IUserProfileService _userProfileService;
        private IUserProfileService _userProfile
            => _userProfileService ??= Service.Instance.Get<IUserProfileService>();

        #endregion
        public void AddSpeakerZoneObserver(ISpeakerZoneObserver observer)
        {
            if (_observers.Contains(observer))
            {
                return;
            }

            _observers.Add(observer);
        }

        public void RemoveSpeakerZoneObserver(ISpeakerZoneObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                return;
            }

            _observers.Remove(observer);
        }

        public void SpeakerZoneEntered(Collider other)
        {
            if (!CheckSpeakerPlayer(other.gameObject))
            {
                return;
            }

            if (other.gameObject.GetComponent<SpeakerAnimator>())
            {
                return;
            }

            SpeakerAnimator playerSpeakerMode = other.gameObject.AddComponent<SpeakerAnimator>();
            playerSpeakerMode.SetupSpeakerAnimatorMode();

            _observers.ForEach(observer => observer.SpeakerZoneEntered());
        }

        public void SpeakerZoneExited(Collider other)
        {
            if (!CheckSpeakerPlayer(other.gameObject))
            {
                return;
            }

            SpeakerAnimator playerSpeakerMode = other.gameObject.GetComponent<SpeakerAnimator>();

            if (playerSpeakerMode == null)
            {
                return;
            }

            playerSpeakerMode.StopSpeakerAnimatorMode();
            Object.Destroy(playerSpeakerMode);

            _observers.ForEach(observer => observer.SpeakerZoneLeft());
        }

        private bool CheckSpeakerPlayer(GameObject player)
        {
            return _localPlayerHolder.IsOtherPlayerEqualsLocalPlayer(player) && _userProfile.IsAdmin;
        }
    }
}
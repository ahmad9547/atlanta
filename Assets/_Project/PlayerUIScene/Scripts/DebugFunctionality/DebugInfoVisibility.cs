using ProjectConfig.General;
using ProjectConfig.Session;
using TMPro;
using UnityEngine;

namespace PlayerUIScene.DebugFunctionality
{
    public sealed class DebugInfoVisibility : MonoBehaviour
    {
        [SerializeField] private TMP_Text _versionLabel;
        [SerializeField] private GameObject _fpsIndicator;

        private void Start()
        {
            SetDebugInfoVisibility();
        }

        private void SetDebugInfoVisibility()
        {
            if (GeneralSettings.ShowDebugInfo)
            {
                _fpsIndicator.SetActive(true);
                _versionLabel.gameObject.SetActive(true);
                _versionLabel.text = $"ver.: {GeneralSettings.Version} | {SessionConfig.RoomId}";
                return;
            }

            _fpsIndicator.SetActive(false);
            _versionLabel.gameObject.SetActive(false);
        }
    }
}
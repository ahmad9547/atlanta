using UnityEngine;
using Core.ServiceLocator;
using TMPro;

namespace Metaverse.Teleport.Database
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class TeleportPointNameApplier : MonoBehaviour
    {
        [SerializeField] private TeleportPointType _teleportPointType;

        [SerializeField] private string _prefixPart;
        [SerializeField] private string _suffixPart;

        private TextMeshProUGUI _teleportPointNameField;

        #region Services

        private ITeleportPointNamesService _teleportPointNamesInstance;
        private ITeleportPointNamesService _teleportPointNames
            => _teleportPointNamesInstance ??= Service.Instance.Get<ITeleportPointNamesService>();

        #endregion

        private void Awake()
        {
            _teleportPointNameField = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            if (_teleportPointType == null)
            {
                return;
            }

            SetTeleportName();
        }

        public void SetNameType(TeleportPointType teleportPointNameType)
        {
            _teleportPointType = teleportPointNameType;
            SetTeleportName();
        }

        private void SetTeleportName()
        {
            string text = $"{_prefixPart}{_teleportPointNames.GetPointNameByType(_teleportPointType.PointType)}{_suffixPart}";
            _teleportPointNameField.SetText(text);
        }
    }
}
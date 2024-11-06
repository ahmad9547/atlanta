using System;
using System.Collections;
using Core.ServiceLocator;
using Metaverse.AreaModification.Services;
using Metaverse.PresetModification.Interfaces;
using Photon.Pun;
using PlayerUIScene.SideMenu.AreaModification;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Metaverse.PresetModification.Network
{
    public sealed class ModificationNetworkProvider : MonoBehaviourPunCallbacks
    {
        #region Services

        private IPresetModificationService _presetModificatorInstance;
        private IPresetModificationService _presetModificator
            => _presetModificatorInstance ??= Service.Instance.Get<IPresetModificationService>();

        private IAreaModificator _areaModificatorInstance;
        private IAreaModificator _areaModificator
            => _areaModificatorInstance ??= Service.Instance.Get<IAreaModificator>();

        #endregion

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            foreach (DictionaryEntry property in propertiesThatChanged)
            {
                if (Enum.TryParse(property.Key.ToString(), out RoomType roomType))
                {
                    _presetModificator.SelectModificationPreset(roomType, Convert.ToInt32(property.Value));
                }

                if (Enum.TryParse(property.Key.ToString(), out ModificationType toggleType))
                {
                    _areaModificator.ChangeState(toggleType, Convert.ToBoolean(property.Value));
                }
            }
        }
    }
}
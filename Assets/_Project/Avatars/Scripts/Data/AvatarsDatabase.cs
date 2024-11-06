using System.Linq;
using UnityEngine;

namespace Avatars.Data
{
    [CreateAssetMenu(fileName = "AvatarsDatabase", menuName = "ScriptableObjects/AvatarsDatabase")]
    public class AvatarsDatabase : ScriptableObject
    {
        [SerializeField] private AvatarEntry[] _entries;

        public AvatarEntry GetAvatarEntryByLink(string avatarLink)
        {
            return _entries.ToList().Find(entry => entry.AvatarLink == avatarLink);
        }

        public AvatarEntry GetDefaultAvatarEntry()
        {
            return _entries[0];
        }
    }
}
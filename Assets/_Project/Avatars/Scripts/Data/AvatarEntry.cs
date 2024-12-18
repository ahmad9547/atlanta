using System;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace Avatars.Data
{
	[Serializable]
	public class AvatarEntry
	{
		public string AvatarLink;
		public GameObject AvatarPrefab;
		public OutfitGender OutfitGender;
	}
}
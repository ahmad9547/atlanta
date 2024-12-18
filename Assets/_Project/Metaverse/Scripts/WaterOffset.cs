using UnityEngine;

namespace Metaverse
{
	public sealed class WaterOffset : MonoBehaviour
	{
		[SerializeField] private Material _waterMaterial;
		[SerializeField] private float _scrollSpeed = 0.15f;
		[SerializeField] private Vector2 _scrollOffsetDirection;

		private const string MainTexture = "_MainTex";	

		private void Update()
		{			 			
			_waterMaterial.SetTextureOffset(MainTexture, _scrollOffsetDirection * Time.time * _scrollSpeed);
		}

		private void OnDisable()
		{
			_waterMaterial.SetTextureOffset(MainTexture, new Vector2(0, 0));
		}
	}
}

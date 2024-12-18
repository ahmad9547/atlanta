using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MeshPainter
{
	[RequireComponent(typeof(MeshRenderer))]
	public class MeshPaint : MonoBehaviour
	{
		[System.Serializable]
		public class Detail
		{
			[SerializeField] Texture2D _texture;
			[SerializeField] Texture2D _normal;
			[SerializeField] Vector2 _tiling;

			public Texture2D Texture { get { return _texture; } set { _texture = value; } }
			public Texture2D Normal { get { return _normal; } set { _normal = value; } }
			public Vector2 Tiling { get { return _tiling; } set { _tiling = value; } }
		}

	#if UNITY_EDITOR
		[SerializeField] List<Detail> _details = new List<Detail>();
		[SerializeField] List<Texture2D> _splats = new List<Texture2D>();
		[SerializeField, HideInInspector] UndoManager _undoManager;
		[SerializeField] Texture2D _splatMap;
		[SerializeField] float _scale;
		[SerializeField] float _hardness;
		[SerializeField] float _fillThreshold;

		public List<Detail> Details { get { return _details; } }

		public List<Texture2D> Splats { get { return _splats; } }
	
		//public Texture2D CurrentSplat { get { return _splatMap; } set { _splatMap = value; } }

		public int SelectedBrushIndex { get; set; }

		public int Tool { get; set; }

		public float SelectedBrushScale { get { return _scale; } set { _scale = value; } }

		public float SelectedBrushHardness { get { return _hardness; } set { _hardness = value; } }

		public float SelectedFillThreshold { get { return _fillThreshold; } set { _fillThreshold = value; } }

		public int SelectedDetailIndex { get; set; }

		public MeshCollider PaintMeshCollider { get; set; }

		public Dictionary<Collider,bool> OriginalColliders { get; set; }

		public int OriginalLayer { get; set; }

		public bool SplatPainted { get; set; }

		public bool SettingsEnabled { get; set; }

		public Vector2 BrushesScrollPosition { get; set; }

		public UndoManager UndoManager { get { return _undoManager; } }

		public bool PaintStatusPainting  { get; set; }

		public Vector3 PaintStatusPosition  { get; set; }

		public Vector3 PaintStatusNormal  { get; set; }
	
		void OnDrawGizmos ()
		{
			if (PaintStatusPainting) {
				Gizmos.DrawRay (PaintStatusPosition, PaintStatusNormal);
			}
		}

	#endif
	}

}
	
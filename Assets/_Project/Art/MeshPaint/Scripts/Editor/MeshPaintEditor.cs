using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace MeshPainter
{
	[CustomEditor(typeof(MeshPaint))]
	public class MeshPaintEditor : Editor
	{
		MeshPaint _target;
		Material _targetMaterial;
		bool _canPaint;
		bool _painting;
		bool _painted;
		PreviewManager _previewManager;
		Brush _currentBrush;
		List<Brush> _brushes;
		const int ButtonSize = 40;
		const int GuiSpace = 10;
		const int DetailPerSplat = 4;
		const int PaintLayer = 6;
		public const string AutoCreateMeshColliderSaveKey = "AutoCreateMeshColliderSaveKey";
		public const string ImportUserBrushesSaveKey = "ImportUserBrushesSaveKey";
		const string ShaderDetailProperty = "_Splat";
		const string ShaderControlProperty = "_Control";
		const string ShaderNormalProperty = "_Normal";
		GUIStyle _textBoxStyle;

		public const int ToolNone = -1;
		public const int ToolBrush = 0;
		public const int ToolFill = 1;

		Texture2D _fillIcon;
		Texture2D _brushIcon;
		Texture2D _fillPreview;
		Brush _fillBrush;

		void OnEnable ()
		{
			_target = target as MeshPaint;
			_targetMaterial = _target.GetComponent<MeshRenderer> ().sharedMaterial;
			_previewManager = new PreviewManager ();

			_target.SplatPainted = false;

			Undo.undoRedoPerformed += UndoCallback;

			CheckCanPaint ();

			_previewManager.CreateProjector (_target.GetComponent<MeshFilter> (), _target);

			_brushes = new List<Brush> ();

			var thisPath = CurrentScriptPath();
			
			_fillIcon = LoadTexture(thisPath + "/Icons/Fill.png");
			_brushIcon = LoadTexture(thisPath + "/Icons/Brush.png");
			_fillPreview = LoadTexture(thisPath + "/Icons/FillPreview.png");

			_fillBrush = new Brush (_fillPreview);

			Settings.Init ();

			if (SavePreprocessor.BeforeSaveEvent == null) {
				SavePreprocessor.BeforeSaveEvent += BeforeSave;
			}
		}

		void OnDisable ()
		{
			Undo.undoRedoPerformed -= UndoCallback;
			SavePreprocessor.BeforeSaveEvent -= BeforeSave;
		
			_previewManager.DestoryProjector ();
		
			if (_brushes != null) {
				foreach (Brush b in _brushes) {
					b.Destroy ();
				}
			}
		
			if (SelectionChanged ()) {
				RestoreColider ();
				_target.UndoManager.Step = -1;
				_target.SelectedBrushIndex = -1;
				_target.Tool = ToolBrush;
				_target.SettingsEnabled = false;

				RestoreLayer();
			
				WriteTexture ();
			}
		}

		void RestoreLayer()
		{
			if (_target != null && _target.gameObject.layer == PaintLayer)
				_target.gameObject.layer = _target.OriginalLayer;
		}

		void BeforeSave ()
		{
			_target.UndoManager.HasUndoRedoPerformed = true;
			RestoreColider ();
			EditorUtility.SetDirty (_target);
			WriteTexture ();
		}

		void WriteTexture ()
		{
			if (_target.Splats.Count > 0 && _target.SplatPainted) {

				_target.SplatPainted = false;

				foreach (Texture2D splat in _target.Splats) {
					if (splat != null && (splat.format == TextureFormat.ARGB32 || splat.format == TextureFormat.RGBA32)) {
						var path = AssetDatabase.GetAssetPath (splat);
						if (path.Length > 0) {
							var textureBytes = splat.EncodeToPNG ();
							System.IO.File.WriteAllBytes (path, textureBytes);
						}

					}
				}
			}

			AssetDatabase.Refresh ();
		}

		void SetupColiders ()
		{
			if (!Settings.AutoCreateMeshCollider)
				return; //TODO maybe cache this

			if (_target.PaintMeshCollider == null) {
				Collider[] colliders = _target.GetComponents<Collider> ();

				if (colliders != null && colliders.Length > 0) {
					_target.OriginalColliders = new Dictionary<Collider, bool> ();

					foreach (Collider c in colliders) {
						_target.OriginalColliders.Add (c, c.enabled);
						c.enabled = false;
					}
				}

				_target.PaintMeshCollider = _target.gameObject.AddComponent<MeshCollider> ();
			}
		}

		void RestoreColider ()
		{	
			if (_target.OriginalColliders != null) {
				foreach (KeyValuePair<Collider,bool> c in _target.OriginalColliders) {
					if (c.Key != null) {
						c.Key.enabled = c.Value;
					}
				}

				_target.OriginalColliders.Clear ();
			}

			if (_target.PaintMeshCollider != null) {
				DestroyImmediate (_target.PaintMeshCollider);
			}
		}

		void CheckCanPaint ()
		{
			_canPaint = !Application.isPlaying && CheckMaterial () && _target.Details.Count > 0 && _target.Splats.Count > 0;
		}

		bool SelectionChanged ()
		{
			return _target == null || Selection.activeObject != _target.gameObject;
		}

		void UndoCallback ()
		{
			_target.UndoManager.UndoRedoPerformed ();
		}
	
		public override void OnInspectorGUI ()
		{
			_textBoxStyle = GUI.skin.GetStyle ("HelpBox");

			KeyBoardShortcutsGUI ();
			CheckCanPaint ();

			if (_canPaint) {
				switch (Event.current.type) {
				case EventType.Layout:

					if (_brushes.Count == 0) {
						LoadBrushes ();
					}

					break;
				}

				if (_target.SettingsEnabled) {
					SettingsGUI ();
				} else {
					PaintGUI ();
				}
			} else {

				if (_target.SelectedBrushScale < 0.1f)
					_target.SelectedBrushScale = 1f;
			
				if (_target.SelectedBrushHardness < 0.1f)
					_target.SelectedBrushHardness = 1f;

				if (_target.SelectedFillThreshold < 0.1f)
					_target.SelectedFillThreshold = 0.2f;

				if (!CheckMaterial ()) {
					MaterialGUI ();
				} else {
					CopyFromMaterial ();
					CheckCanPaint ();

					_target.SettingsEnabled = true;
					SettingsGUI ();
				}
			}
		}

		void MaterialGUI ()
		{
			GUILayout.Label ("Please select a supported shader", _textBoxStyle);
		}

		void SettingsGUI ()
		{
			if (_canPaint) {
				EditorGUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Paint")) {
					_target.SettingsEnabled = false;
					UpdateMaterial ();
				}
				EditorGUILayout.EndHorizontal ();
			} else {
				GUILayout.Label ("fill the required fields", _textBoxStyle);
			}

			Texture2D source = null;

			GUILayout.Label ("*Splatmap", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal ();

			for (int i = 0; i < _target.Splats.Count; i++) {
				source = (Texture2D)EditorGUILayout.ObjectField (_target.Splats [i], typeof(Texture2D), false, GUILayout.Width (ButtonSize), GUILayout.Height (ButtonSize));

				if (source != _target.Splats [i]) {
					Undo.RecordObject (_target, "Change Splatmap");					
					_target.Splats [i] = source;
					UpdateMaterial ();
				}
			}

			if (_target.Splats.Count < CountShaderSplats()) {
				EditorGUILayout.BeginVertical (GUILayout.Width (ButtonSize));
				source = (Texture2D)EditorGUILayout.ObjectField (null, typeof(Texture2D), false, GUILayout.Width (ButtonSize), GUILayout.Height (ButtonSize));
				EditorGUILayout.EndVertical ();
				if (source != null) {

					Undo.RecordObject (_target, "Add Splat Texture");
					
					_target.Splats.Add (source);
					
					UpdateMaterial ();
				}
			}

			if (GUILayout.Button ("New", GUILayout.Width (ButtonSize), GUILayout.Height (ButtonSize))) {
				var window = EditorWindow.GetWindow<CreateSplatmap> ("Create Splat", true);
				window.EventCreated = (t) => {
					if (t != null) {

						for(int i=0;i<CountShaderSplats();i++)
						{
							if(i < _target.Splats.Count)
							{
								if(_target.Splats[i] == null)
								{
									_target.Splats[i] = t;
									break;
								}
							}
							else
							{
								_target.Splats.Add(t);
								break;
							}
						}

						UpdateMaterial ();
					} };
				window.Show ();
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.Label ("*Detail", EditorStyles.boldLabel);

			bool lineBreak = (_target.Details.Count % DetailPerSplat) == 0;

			for (int i = 0; i < _target.Details.Count; i++) {

				bool newLine = (i%DetailPerSplat) == 0;

				if(newLine){
					if(i>0)
						EditorGUILayout.EndHorizontal ();

					EditorGUILayout.BeginHorizontal ();
				}


				EditorGUILayout.BeginVertical (GUILayout.Width (ButtonSize));
				if(i>=DetailPerSplat) GUILayout.Space (20);
				source = (Texture2D)EditorGUILayout.ObjectField (_target.Details [i].Texture, typeof(Texture2D), false, GUILayout.Width (ButtonSize), GUILayout.Height (ButtonSize));

				Texture2D normal = null;

				if(HasNormalTexture(i))
				{
					GUILayout.Label ("Normal", EditorStyles.miniLabel);
					normal = (Texture2D)EditorGUILayout.ObjectField (_target.Details [i].Normal, typeof(Texture2D), false, GUILayout.Width (ButtonSize), GUILayout.Height (ButtonSize));

					if (normal != _target.Details [i].Normal) {
						Undo.RecordObject (_target, "Detail Normal");
						
						_target.Details [i].Normal = normal;
						
						UpdateMaterial ();
					}


				}

				GUILayout.Label ("Tiling xy", EditorStyles.miniLabel);
				float tilingx = EditorGUILayout.FloatField (_target.Details [i].Tiling.x, GUILayout.Width (ButtonSize));
				float tilingy = EditorGUILayout.FloatField (_target.Details [i].Tiling.y, GUILayout.Width (ButtonSize));

				if (tilingx != _target.Details [i].Tiling.x || tilingy != _target.Details [i].Tiling.y) {
					Undo.RecordObject (_target, "Detail Tiling");

					_target.Details [i].Tiling = new Vector2 (tilingx, tilingy);

					UpdateMaterial ();
				}

				if (source != _target.Details [i].Texture) {
					Undo.RecordObject (_target, "Detail Texture");

					_target.Details [i].Texture = source;

					UpdateMaterial ();
				}

				EditorGUILayout.EndVertical ();

				if(i+1 == _target.Details.Count && lineBreak){
					EditorGUILayout.EndHorizontal ();
				}
			}

			if (lineBreak) {
				EditorGUILayout.BeginHorizontal ();
			}

			if (_target.Details.Count < CountShaderDetails()) {
				EditorGUILayout.BeginVertical (GUILayout.Width (ButtonSize));
				if(_target.Details.Count>=DetailPerSplat) GUILayout.Space (20);
				source = (Texture2D)EditorGUILayout.ObjectField (null, typeof(Texture2D), false, GUILayout.Width (ButtonSize), GUILayout.Height (ButtonSize));
				EditorGUILayout.EndVertical ();
				if (source != null) {
					MeshPaint.Detail detail = new MeshPaint.Detail ();
					detail.Texture = (Texture2D)source;
					detail.Tiling = Vector2.one;

					Undo.RecordObject (_target, "Add Detail Texture");

					_target.Details.Add (detail);

					UpdateMaterial ();
				}
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.Label ("Options", EditorStyles.boldLabel);

			if (AutoCreateMeshColliderOptionItemGUI ("Auto create mesh collider"))
				SetupColiders ();
			if (ImportUserBrushesOptionItemGUI ("Import custom brushes"))
				LoadBrushes ();
			if (UseGPUScaleOptionItemGUI ("Use GPU brush scale"))
				LoadBrushes ();
		}

		bool AutoCreateMeshColliderOptionItemGUI (string title)
		{
			bool current = Settings.AutoCreateMeshCollider;
			EditorGUILayout.BeginHorizontal ();
			bool result = EditorGUILayout.Toggle (current, GUILayout.Width (20));
			Settings.AutoCreateMeshCollider = result;
			GUILayout.Label (title, EditorStyles.label);
			EditorGUILayout.EndHorizontal ();

			return result != current;
		}

		bool ImportUserBrushesOptionItemGUI (string title)
		{
			bool current = Settings.ImportUserBrushes;
			EditorGUILayout.BeginHorizontal ();
			bool result = EditorGUILayout.Toggle (current, GUILayout.Width (20));
			Settings.ImportUserBrushes = result;
			GUILayout.Label (title, EditorStyles.label);
			EditorGUILayout.EndHorizontal ();
		
			return result != current;
		}

		bool UseGPUScaleOptionItemGUI (string title)
		{
			bool current = Settings.UseGPUScale;
			EditorGUILayout.BeginHorizontal ();
			#if !(UNITY_5 || UNITY_5_3_OR_NEWER)
			GUI.enabled = false;
			#endif
			bool result = EditorGUILayout.Toggle (current, GUILayout.Width (20));
			#if !(UNITY_5 || UNITY_5_3_OR_NEWER)
			GUI.enabled = true;
			#endif
			Settings.UseGPUScale = result;
			#if !(UNITY_5 || UNITY_5_3_OR_NEWER)
			GUILayout.Label (title + " (Unity 5+ only)", EditorStyles.label);
			#else
			GUILayout.Label (title, EditorStyles.label);
			#endif
			EditorGUILayout.EndHorizontal ();
		
			return result != current;
		}

		string GetToolStartText()
		{
			if(_target.Tool == ToolFill)
				return GetToolUseText();

			return "Select a Brush to start painting";;
		}

		string GetToolUseText()
		{
			if (Application.platform == RuntimePlatform.OSXEditor) {
				if(_target.Tool == ToolBrush)
					return "Hold Cmd to start painting";
				else if(_target.Tool == ToolFill)
					return "Hold Cmd and click left mouse button to fill";
			} else {
				if(_target.Tool == ToolBrush)
					return "Hold Ctrl to start painting";
				else if(_target.Tool == ToolFill)
					return "Hold Ctrl and click left mouse button to fill";
			}

			return "Hold Ctrl/Cmd to start";
		}

		string SelectionStyle(bool selected)
		{
			//return selected ? "TL SelectionButton PreDropGlow" : "box";

			//return selected ? "U2D.createRect" : "box";

			//return selected ? "SelectionRect" : "box";

			//return selected ? "ProgressBarBar" : "box";

			return selected ? "LightmapEditorSelectedHighlight" : "box";

		}

		void PaintGUI ()
		{
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Setings")) {
				_target.SettingsEnabled = true;
				UpdateDetails ();
			}
			EditorGUILayout.EndHorizontal ();

			if (_canPaint) {
				if (_currentBrush == null) {
					GUILayout.Label (GetToolStartText(), _textBoxStyle);
				} else if (_target.GetComponent<MeshCollider> () == null) {
					GUILayout.Label ("Please add a MeshColider to painting", _textBoxStyle);
				} else {
					GUILayout.Label (GetToolUseText(), _textBoxStyle);
				}
			}

			GUILayout.Label ("Detail", EditorStyles.boldLabel);

			for (int i = 0; i < _target.Details.Count; i++) {

				bool newLine = (i%DetailPerSplat) == 0;

				if(newLine){
					if(i>0)
					{
						EditorGUILayout.EndHorizontal ();
						GUILayout.Space (4);
					}

					EditorGUILayout.BeginHorizontal ();
					GUILayout.Space (4);
				}

				if (GUILayout.Button (new GUIContent(_target.Details [i].Texture), SelectionStyle( i == _target.SelectedDetailIndex ), GUILayout.Width (ButtonSize), GUILayout.Height (ButtonSize))) {
					_target.SelectedDetailIndex = i;
				}

				Rect rect = GUILayoutUtility.GetLastRect();
				rect.x+=2;
				rect.y+=2;
				rect.width-=4;
				rect.height-=4;

				GUI.DrawTexture(rect,_target.Details [i].Texture,ScaleMode.ScaleToFit,false);

				if(i+1==_target.Details.Count){
					EditorGUILayout.EndHorizontal ();
				}
			}

			GUILayout.Label ("Tools", EditorStyles.boldLabel);
			
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Space (4);


			if (GUILayout.Button (_brushIcon, SelectionStyle(_target.Tool == ToolBrush), GUILayout.Width (ButtonSize), GUILayout.Height (ButtonSize))) {
				_target.Tool = ToolBrush;

				if(_target.SelectedBrushIndex > 0)
					SelectBrush (_target.SelectedBrushIndex, _target.SelectedBrushScale);
				else
					SelectBrush (0, 1);
			}

			if (GUILayout.Button (_fillIcon, SelectionStyle(_target.Tool == ToolFill), GUILayout.Width (ButtonSize), GUILayout.Height (ButtonSize))) {
				SelectFill();
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.Label ("Brushes", EditorStyles.boldLabel);
		
			int maxPerLine = Screen.width / (ButtonSize + GuiSpace + 4);
			int buttonsPerLine = Mathf.Min (maxPerLine, _brushes.Count);
			int lines = Mathf.CeilToInt ((float)_brushes.Count / (float)buttonsPerLine);
		
			_target.BrushesScrollPosition = GUILayout.BeginScrollView (_target.BrushesScrollPosition, GUILayout.Height ((ButtonSize + GuiSpace) * 3));
		
			GUILayout.BeginVertical ();
		
			for (int i = 0; i < lines; i++) {
				GUILayout.Space (4);
				GUILayout.BeginHorizontal ();
			
				for (int j = 0; j < buttonsPerLine; j ++) {
					int index = (i * buttonsPerLine) + j;
				
					if (index < _brushes.Count) {
						GUILayout.Space (4);
						if (GUILayout.Button (_brushes [index].OriginalTexture, SelectionStyle (_brushes [index] == _currentBrush), GUILayout.Width (ButtonSize), GUILayout.Height (ButtonSize))) {
							SelectBrush (index, 1f);
						}
					}
				}
			
				GUILayout.EndHorizontal ();		
			}
		
			GUILayout.EndVertical ();

			GUILayout.EndScrollView ();
		
			if (_target.Tool == ToolBrush) {
				GUILayout.Label ("Scale", EditorStyles.boldLabel);
		
				float scale = EditorGUILayout.Slider (_target.SelectedBrushScale, 0.1f, 10f);
				if (!Mathf.Approximately (scale, _target.SelectedBrushScale)) {
					Undo.RecordObject (_target, "Brush Scale");
					_target.SelectedBrushScale = scale;

					if (_currentBrush != null) {
						_currentBrush.Scale = scale;
						_previewManager.PreviewBrush (_currentBrush);
					}
				}
			}
		
			GUILayout.Label ("Softness", EditorStyles.boldLabel);

			float hardness = EditorGUILayout.Slider (_target.SelectedBrushHardness, 0.1f, 1f);

			if (!Mathf.Approximately (hardness, _target.SelectedBrushHardness)) {
				Undo.RecordObject (_target, "Brush Softness");		
				_target.SelectedBrushHardness = hardness;
			}


			if (_target.Tool == ToolFill) {
				GUILayout.Label ("Fill Threshold", EditorStyles.boldLabel);
			
				float threshold = EditorGUILayout.Slider (_target.SelectedFillThreshold, 0.1f, 0.8f);
			
				if (!Mathf.Approximately (threshold, _target.SelectedFillThreshold)) {
					Undo.RecordObject (_target, "Fill Threshold");
					_target.SelectedFillThreshold = threshold;
				}
			}

		}

		void OnSceneGUI ()
		{
			var ctrlID = GUIUtility.GetControlID ("MeshPainterProjector".GetHashCode (), FocusType.Passive);
		
			KeyBoardShortcutsGUI ();

			if (_canPaint) {
				switch (Event.current.type) {
				case EventType.MouseUp:

					if (_painting && _target.Tool == ToolFill) {

						FloodFill();
					}

					if (_painted) {
						RecordUndo ();
					}
					_painted = false;
					break;								
				case EventType.MouseDown:
				case EventType.MouseDrag:
					if (_painting && _target.Tool == ToolBrush) {
						UpdatePaint ();
						Event.current.Use ();
					}
					break;
				case EventType.MouseMove:
					HandleUtility.Repaint ();
					break;
				case EventType.Layout:
					if (Event.current.control || Event.current.command) {
						HandleUtility.AddDefaultControl (ctrlID);
					}

					break;			
				}

				UpdatePreview ();
			}
		}

		void KeyBoardShortcutsGUI ()
		{
			if (_canPaint) {
				switch (Event.current.type) {
				case EventType.KeyDown:
				
					if (Event.current.keyCode == (KeyCode.LeftBracket)) {
						ModifyBrushScale (-0.1f);
						Event.current.Use ();
						EditorUtility.SetDirty (_target);
					} else if (Event.current.keyCode == (KeyCode.RightBracket)) {
						ModifyBrushScale (0.1f);
						Event.current.Use ();
						EditorUtility.SetDirty (_target);
					}

					break;
				case EventType.Layout:

					bool paint = (Event.current.control || Event.current.command) && !Event.current.alt && _currentBrush != null;

					if (paint != _painting) {
						_painting = paint;
						StartStopPaint (_painting);
					}

					break;
				}
			}
		}

		void StartStopPaint (bool painting)
		{
			_target.PaintStatusPainting = painting;
		}

		bool CheckSplatFormat()
		{
			foreach (Texture2D splat in _target.Splats) {
				if (splat != null && ((splat.format != TextureFormat.ARGB32 && splat.format != TextureFormat.RGBA32) || !CreateSplatmap.isTextureReadable (splat))) {
					if (EditorUtility.DisplayDialog ("Wrong format", "The splatmap format must be ARGB32 and Read/Write must be enabled to start painting. \n\n Switch format now?", "Yes", "Cancel")) {
						CreateSplatmap.StartPaintTextureFormat (splat);
					} else {
						_target.SettingsEnabled = true;
						_currentBrush = null;
						Debug.LogWarning ("please set splatmap to ARGB32 and Read/Write enabled in import options", splat);
						return false;
					}
				}
			}

			return true;
		}

		void PrepareMesh()
		{
			if (_target.UndoManager.Step == -1) { //record original state
				RecordUndo ();
			}
			
			if (_target.gameObject.layer != PaintLayer) {
				_target.OriginalLayer = _target.gameObject.layer;
				_target.gameObject.layer = PaintLayer;
			}
		}

		void SelectFill()
		{
			if (!CheckSplatFormat())
			{
				return;
			}

			if (!EditorApplication.isCompiling) {
				_target.Tool = ToolFill;
				_currentBrush = _fillBrush;
				_currentBrush.Scale = 1;
				_previewManager.PreviewBrush (_currentBrush);
				
				PrepareMesh();
			}

			SetupColiders ();
		}

		void SelectBrush (int index, float scale)
		{
			if (!CheckSplatFormat())
			{
				return;
			}

			if (!EditorApplication.isCompiling && index < _brushes.Count) {
				_target.Tool = ToolBrush;
				_target.SelectedBrushIndex = index;
				_currentBrush = _brushes [index];
				_currentBrush.Scale = _target.SelectedBrushScale;
				_previewManager.PreviewBrush (_currentBrush);

				PrepareMesh();
			}

			SetupColiders ();
		}

		void ModifyBrushScale (float add)
		{
			if (_target.Tool == ToolBrush && _currentBrush != null) {
				_currentBrush.Scale += add;
				_target.SelectedBrushScale = _currentBrush.Scale;
				_previewManager.PreviewBrush (_currentBrush);
			}
		}

		void LoadBrushes ()
		{
			if (_brushes != null)
				_brushes.Clear ();

			Texture2D texture = null;
			int num = 1;
			do {

				texture = (Texture2D)EditorGUIUtility.Load ("Brushes/builtin_brush_" + num + ".png");

				if (texture != null) {
					_brushes.Add (new Brush (texture));
				}

				num++;

			} while(texture != null);

			if (Settings.ImportUserBrushes) {

				var pathOnly = CurrentScriptPath();

				num = 0;

				do {

					texture = LoadTexture(pathOnly + "/CustomBrushes/brush_" + num + ".png");

					if (texture != null) {
						_brushes.Add (new Brush (texture));
					}

					num++;

				} while(texture != null);
			}

			if (_target != null) {
				if (_target.UndoManager!=null && _target.UndoManager.HasUndoRedoPerformed || _target.PaintMeshCollider != null) {
					_target.UndoManager.HasUndoRedoPerformed = false;

					if(_target.Tool == ToolBrush) {
						if (_target.SelectedBrushIndex > -1 && _brushes[_target.SelectedBrushIndex] != _currentBrush) {
							SelectBrush (_target.SelectedBrushIndex, _target.SelectedBrushScale);
						}
					} else if(_target.Tool == ToolFill) {
						SelectFill();
					}
				}
			}

			UpdateMaterial ();
		}

		string CurrentScriptPath()
		{
			var script = MonoScript.FromScriptableObject (this);
			var path = AssetDatabase.GetAssetPath (script);
			var pathOnly = path.Substring (0, path.LastIndexOf ("/"));

			return pathOnly;
		}

		Texture2D LoadTexture(string path)
		{
			#if UNITY_5 || UNITY_5_3_OR_NEWER

				#if UNITY_5_0_0 || UNITY_5_0_1
					return Resources.LoadAssetAtPath<Texture2D> (path);
				#else
					return AssetDatabase.LoadAssetAtPath<Texture2D> (path);
				#endif

			#else
				return Resources.LoadAssetAtPath<Texture2D> (path);
			#endif
		}

		void RecordUndo ()
		{
			Undo.RecordObject (_target, "Paint");
			_target.UndoManager.Record (_target.Splats);
		}

		void PaintSplat (Texture2D splatMap, Texture2D brush, Vector2 textCoord, float hardness)
		{
			int brushWidth = brush.width;
			int brushHeight = brush.height;

			int x = Mathf.FloorToInt (textCoord.x * splatMap.width) - brushWidth / 2;
			int y = Mathf.FloorToInt (textCoord.y * splatMap.height) - brushHeight / 2;

			int xOffset = CalculateOffset (ref brushWidth, x, splatMap.width);
			int yOffset = CalculateOffset (ref brushHeight, y, splatMap.height);

			x = Mathf.Clamp (x, 0, splatMap.width);
			y = Mathf.Clamp (y, 0, splatMap.height);

			Color[] srcPixels = splatMap.GetPixels (x, y, brushWidth, brushHeight, 0);

			Color targetColor = GetTargetColor (_target.SelectedDetailIndex);

			int splatIndex = _target.Splats.IndexOf (splatMap);
			int detailSplat = Mathf.CeilToInt (_target.SelectedDetailIndex / DetailPerSplat);
			
			if (splatIndex != detailSplat) {
				targetColor = new Color (0, 0, 0, 0);
			}

			for (int i = 0; i < srcPixels.Length; i++) {
				int px = i % brushWidth;
				int py = Mathf.FloorToInt (i / brushWidth);

				float blendFactor = brush.GetPixel (px + xOffset, py + yOffset).a * hardness;
				srcPixels [i] = Color.Lerp (srcPixels [i], targetColor, blendFactor);
			}
			splatMap.SetPixels (x, y, brushWidth, brushHeight, srcPixels, 0);
			splatMap.Apply ();
		}

		int CalculateOffset (ref int brushSize, int pos, int sizeLimit)
		{
			int xOffset = 0;
			if (pos < 0) {
				xOffset = -pos;
				brushSize -= -pos;
			} else if (pos + brushSize > sizeLimit) {
				brushSize -= pos + brushSize - sizeLimit;
			}
		
			return xOffset;
		}

		Color GetTargetColor (int detailIndex)
		{
			switch (detailIndex%DetailPerSplat) {
			case 0:
				return new Color (1, 0, 0, 0);
			case 1:
				return new Color (0, 1, 0, 0);
			case 2:
				return new Color (0, 0, 1, 0);
			case 3:
				return new Color (0, 0, 0, 1);
			}

			return new Color (0, 0, 0, 0);
		}

		bool CoordRangeValid(Vector2 coord)
		{
			return coord.x >= 0f && coord.x <= 1f && coord.y >= 0f && coord.y <= 1f;
		}

		void UpdatePaint ()
		{
			Ray ray = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 1000f, 1 << PaintLayer) && hit.collider.gameObject == _target.gameObject) {
				if(CoordRangeValid(hit.textureCoord)) {
					foreach(Texture2D splat in _target.Splats){
						PaintSplat (splat, _currentBrush.PaintTexture, hit.textureCoord, _target.SelectedBrushHardness);
					}
					_painted = true;
					_target.SplatPainted = true;
				} else {
					Debug.LogWarning("mesh uv coordinates must be between 0 and 1");
				}
			}
		}

		void FloodFill()
		{
			Ray ray = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 1000f, 1 << PaintLayer) && hit.collider.gameObject == _target.gameObject) {
				if(CoordRangeValid(hit.textureCoord)) {

					Color targetColor = GetTargetColor (_target.SelectedDetailIndex);
					int detailSplat = Mathf.CeilToInt (_target.SelectedDetailIndex / DetailPerSplat);

					PaintUtils.FloodFillArea1 (detailSplat, _target.Splats, hit.textureCoord, targetColor, _target.SelectedBrushHardness, _target.SelectedFillThreshold);

					_painted = true;
					_target.SplatPainted = true;
				} else {
					Debug.LogWarning("mesh uv coordinates must be between 0 and 1");
				}
			}
		}

		void UpdatePreview ()
		{

			Vector2 newMousePostion = Event.current.mousePosition;

			#if UNITY_5_5_OR_NEWER
			Ray ray = HandleUtility.GUIPointToWorldRay (newMousePostion);
			#else
			newMousePostion.y = Screen.height - (newMousePostion.y + 36);
			Ray ray = Camera.current.ScreenPointToRay (newMousePostion);
			#endif

			RaycastHit hit = new RaycastHit();

			_previewManager.Enable = _currentBrush != null && Physics.Raycast (ray, out hit, 1000f, 1 << PaintLayer) && hit.collider.gameObject == _target.gameObject;

			if (_previewManager.Enable) {
				_previewManager.TransformNow (hit.point + (hit.normal * 100), Quaternion.LookRotation (hit.normal));
				_target.PaintStatusPosition = hit.point;
				_target.PaintStatusNormal = hit.normal;
			}
		}
	
		bool CheckMaterial ()
		{
			if (_targetMaterial == null)
				return false;

			int details = CountShaderDetails ();
			if (details == 0)
				return false;

			int contols = CountShaderSplats ();

			return (details/DetailPerSplat) <= contols;
		}

		int CountShaderSplats()
		{
			int control = 0;

			while (_targetMaterial.HasProperty(ShaderControlProperty + control)) {
				control++;
			}

			return control;
		}

		int CountShaderDetails()
		{
			int detail = 0;
			
			while (_targetMaterial.HasProperty(ShaderDetailProperty + detail)) {
				detail++;
			}
			
			return detail;
		}

		bool HasNormalTexture(int detailIndex)
		{
			for (int i=0; i < ShaderUtil.GetPropertyCount (_targetMaterial.shader); i++) {
				if(ShaderUtil.GetPropertyName(_targetMaterial.shader, i) == (ShaderNormalProperty + detailIndex))
				{
					return true;
				}
			}

			return false;
		}

		Texture2D GetSplatForDetal(int index)
		{
			return _target.Splats[ Mathf.CeilToInt (index / DetailPerSplat) ];
		}

		void CopyFromMaterial ()
		{
			if (_targetMaterial == null || _target.Details.Count > 0 || !CheckMaterial ())
				return;

			int propertyIndex = 0;

			_target.Splats.Clear ();

			while (_targetMaterial.HasProperty(ShaderControlProperty + propertyIndex) && _targetMaterial.GetTexture (ShaderControlProperty + propertyIndex)!=null) {
				_target.Splats.Add((Texture2D)_targetMaterial.GetTexture (ShaderControlProperty + propertyIndex));
				propertyIndex++;
			}

			propertyIndex = 0;

			while (_targetMaterial.HasProperty(ShaderDetailProperty + propertyIndex)) {

				if(_targetMaterial.GetTexture (ShaderDetailProperty + propertyIndex) != null)
				{
					MeshPaint.Detail detail = new MeshPaint.Detail ();
					detail.Texture = (Texture2D)_targetMaterial.GetTexture (ShaderDetailProperty + propertyIndex);
					detail.Tiling = _targetMaterial.GetTextureScale (ShaderDetailProperty + propertyIndex);

					if(_targetMaterial.GetTexture (ShaderNormalProperty + propertyIndex) != null)
					{
						detail.Normal =  (Texture2D)_targetMaterial.GetTexture (ShaderNormalProperty + propertyIndex);
					}

					_target.Details.Add (detail);
				}

				propertyIndex++;
			}
		}

		void UpdateDetails ()
		{
			for (int i = 0; i < _target.Details.Count; i++) {
				_target.Details [i].Tiling = _targetMaterial.GetTextureScale (ShaderDetailProperty + i);
			}
		}

		void UpdateMaterial ()
		{
			if (_targetMaterial == null || _target == null) {
				return;
			}

			//_target.Details.RemoveAll (d => d.Texture == null);

			int shaderDetailCount = CountShaderDetails ();

			if (_target.Details.Count > shaderDetailCount) {
				int diff = _target.Details.Count - shaderDetailCount;
				_target.Details.RemoveRange(shaderDetailCount,diff);
			}

			for (int i = 0; i < shaderDetailCount; i++) {
					if(i<_target.Details.Count)
					{
						_targetMaterial.SetTexture (ShaderDetailProperty + i, _target.Details [i].Texture);
						_targetMaterial.SetTextureScale (ShaderDetailProperty + i, _target.Details [i].Tiling);
						_targetMaterial.SetTexture (ShaderNormalProperty + i, _target.Details [i].Normal);
					}
					else
					{
						_targetMaterial.SetTexture (ShaderDetailProperty + i, null);
						_targetMaterial.SetTextureScale (ShaderDetailProperty + i, Vector2.one);
						_targetMaterial.SetTexture (ShaderNormalProperty + i, null);
					}
			}

			for (int j = 0; j < _target.Splats.Count; j++) {
				if(_targetMaterial.HasProperty(ShaderControlProperty + j))
				{
					_targetMaterial.SetTexture (ShaderControlProperty + j, _target.Splats[j]);
				}
				else
				{
					_target.Splats[j] = null;
				}
			}

			_target.Splats.RemoveAll (c => c == null);

			CheckCanPaint ();
		}
	}

}

using PaintIn3D;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaintableRigger
{
	[MenuItem("GameObject/RigObject", false, 10)]
	static void RigObject()
	{
		// Make your changes to the object here
		foreach (GameObject go in Selection.gameObjects)
		{
			if (go.GetComponent<MeshRenderer>() == null) continue;

			Undo.RecordObject(go, $"Rig {go.name} Paintable");

			go.GetOrAddComponent<CwPaintableMesh>();
			var paintable = go.GetOrAddComponent<CwPaintableMeshTexture>();
			paintable.Width = 1024;
			paintable.Height = 1024;
			paintable.Slot = new PaintCore.CwSlot()
			{
				Index = 0,
				Name = "_BaseMap"
			};
			go.GetOrAddComponent<UndoRedo>();
			var col = go.GetComponent<MeshCollider>();
			if (col == null)
			{
				col = go.AddComponent<MeshCollider>();
			}
			go.layer = LayerMask.NameToLayer("Paintable");

			// Check if the object is a prefab instance
			if (PrefabUtility.IsPartOfPrefabInstance(go))
			{
				// Record prefab instance property modifications
				PrefabUtility.RecordPrefabInstancePropertyModifications(go);
				// Mark the prefab as dirty
				EditorUtility.SetDirty(PrefabUtility.GetPrefabParent(go));
			}
			else
			{
				// Mark the scene as dirty
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}
	}
}
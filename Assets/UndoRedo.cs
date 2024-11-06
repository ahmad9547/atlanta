using PaintIn3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoRedo : MonoBehaviour
{
    public CwPaintableMesh paintableMesh;
    public CwPaintableMeshTexture meshTexture;
    public Controller controller;

    private void Start()
    {
        paintableMesh = GetComponent<CwPaintableMesh>();
        meshTexture = GetComponent<CwPaintableMeshTexture>();
        StoreState();
        //meshTexture.OnModified += MeshTexture_OnModified;
    }

    private void MeshTexture_OnModified(bool obj)
    {
        if (!paintableMesh.IsActivated) return;
        controller.currentMeshTexture = meshTexture.gameObject;
    }

    public void StoreState()
    {
        meshTexture.StoreState();
    }
}
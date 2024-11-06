using System.Collections;
using UnityEngine;
using CW.Common;
using PaintCore;
using Photon.Pun;
using System;
using PlayerUIScene.SideMenu;
using UnityEngine.UIElements;

namespace PaintIn3D
{
    [ExecuteInEditMode]
    [HelpURL(CwCommon.HelpUrlPrefix + "CwPaintMultiplayer")]
    [AddComponentMenu(PaintCore.CwCommon.ComponentHitMenuPrefix + "Paint Multiplayer")]
    public class PaintSynchScript : MonoBehaviour, IHitPoint, IHitLine
    {
        public PhotonView photonView;
        public CwPaintSphere CwPaintSphere;
        public Controller controller;



        public void HandleHitLine(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip, string name = "")
        {

            //Debug.Log("Line");
            Color color = controller.paintSphere.Color;
            float radius = controller.paintSphere.Radius;
            ColorData colorData = new ColorData(color.r, color.g, color.b, color.a);
            int paintPriority = PaintData.Instance.GetPriority(name);
            HitlineClass hitlineClass = new HitlineClass(true, preview, priority, pressure, seed, position, rotation, endPosition, colorData, photonView.ViewID.ToString(), radius, clip, name, paintPriority);

            string s = JsonUtility.ToJson(hitlineClass);
            DataManager.Instance.AddToHitLine(hitlineClass);
            photonView.RPC(nameof(GetLine), RpcTarget.OthersBuffered, s);
        }

        public void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation, string name = "")
        {

            //Debug.Log("Point");
            Color color = controller.paintSphere.Color;
            float radius = controller.paintSphere.Radius;
            ColorData colorData = new ColorData(color.r, color.g, color.b, color.a);
            int paintPriority = PaintData.Instance.GetPriority(name);
            HitlineClass hitpointClass = new HitlineClass(false, preview, priority, pressure, seed, position, rotation, Vector3.zero, colorData, photonView.ViewID.ToString(), radius, false, name, paintPriority);
            string s = JsonUtility.ToJson(hitpointClass);
            DataManager.Instance.AddToHitLine(hitpointClass);
            photonView.RPC(nameof(GetLine), RpcTarget.OthersBuffered, s);
        }

        [PunRPC]
        public void GetLine(string s)
        {
            HitlineClass hitlineClass = JsonUtility.FromJson<HitlineClass>(s);
            DataManager.Instance.AddToHitLine(hitlineClass);
            Color color = new Color(hitlineClass.colorData.r, hitlineClass.colorData.g, hitlineClass.colorData.b, hitlineClass.colorData.a);
            controller.SetColor(color);
            controller.SetDrawingRadius(hitlineClass.radius);
            if (hitlineClass.isLine)
            {
                // var hitLine = GetComponentsInChildren<IHitLine>()[0];
                // hitLine.HandleHitLine(hitlineClass.preview, hitlineClass.priority, hitlineClass.pressure, hitlineClass.seed, hitlineClass.position, hitlineClass.endPosition, hitlineClass.rotation, hitlineClass.clip);

                foreach (var hitLine in GetComponentsInChildren<IHitLine>())
                {
                    if ((UnityEngine.Object)hitLine != this)
                    {
                        hitLine.HandleHitLine(hitlineClass.preview, hitlineClass.priority, hitlineClass.pressure, hitlineClass.seed, hitlineClass.position, hitlineClass.endPosition, hitlineClass.rotation, hitlineClass.clip);
                    }
                }
            }
            else
            {
                GetPoint(s);
            }

        }



        public void GetPoint(string s)
        {
            HitlineClass hitpointClass = JsonUtility.FromJson<HitlineClass>(s);
            foreach (var hitPoint in GetComponentsInChildren<IHitPoint>())
            {
                if ((UnityEngine.Object)hitPoint != this)
                {
                    hitPoint.HandleHitPoint(hitpointClass.preview, hitpointClass.priority, hitpointClass.pressure, hitpointClass.seed, hitpointClass.position, hitpointClass.rotation);
                }
                // Submit the hit point
            }
        }

        public void SynchColor(Color32 color)
        {
            if (photonView.IsMine)
            {
                Color newColor = color;
                photonView.RPC(nameof(SynchColorRPC), RpcTarget.OthersBuffered, newColor.r, newColor.g, newColor.b, newColor.a);
            }
        }

        [PunRPC]
        public void SynchColorRPC(float r, float g, float b, float a)
        {
            Color color = new Color(r, g, b, a);
            Color32 newColor = color;
            controller.SetColor(newColor);
        }
    }
}

[Serializable]
public class ColorData
{
    public float r;
    public float g;
    public float b;
    public float a;
    public ColorData(float _r, float _g, float _b, float _a)
    {
        r = _r;
        g = _g;
        b = _b;
        a = _a;
    }
}


[Serializable]
public class HitlineClass
{
    public bool isLine;
    public bool preview;
    public int priority;
    public float pressure;
    public int seed;
    public Vector3 position;
    public Vector3 endPosition;
    public Quaternion rotation;
    public float radius;
    public bool clip;
    public ColorData colorData;
    public string PlayerID;
    public string Name;
    public int PaintPriority;
    public HitlineClass(bool _isLine, bool _prevu, int _prior, float _press, int _seed, Vector3 pos, Quaternion rot, Vector3 endPOs, ColorData _color, string Pid, float radius_, bool clp = false, string name = "", int paintPiority = 0)
    {
        radius = radius_;
        PlayerID = Pid;
        colorData = _color;
        isLine = _isLine;
        preview = _prevu;
        priority = _prior;
        pressure = _press;
        seed = _seed;
        position = pos;
        endPosition = endPOs;
        rotation = rot;
        clip = clp;
        Name = name;
        PaintPriority = paintPiority;
    }
}
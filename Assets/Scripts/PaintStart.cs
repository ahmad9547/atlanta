using PaintCore;
using PaintIn3D;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaintStart : MonoBehaviour
{
    public CwPaintSphere paintSphere;
    public TunnelSceneFlowManager tunnelSceneFlowManager;

    private void Start()
    {
    }

    public void StartPaint(List<HitlineClass> items)
    {
        // StartCoroutine(StartPainting(items));

        var classes = GroupPaintData(items);

        foreach (var list in classes)
        {
            StartCoroutine(StartPaintingAsync(list));
        }

    }

    private List<List<HitlineClass>> GroupPaintData(List<HitlineClass> items)
    {
        List<List<HitlineClass>> hitlines = new();
        items = items.OrderBy(c => c.PaintPriority).ToList();

        hitlines.Add(items.FindAll(x => x.Name.StartsWith("SideWall")));
        hitlines.Add(items.FindAll(x => x.Name.StartsWith("Crate")));
        hitlines.Add(items.FindAll(x => x.Name.StartsWith("Wall1")));       //TODO: Divide into two or more
        hitlines.Add(items.FindAll(x => x.Name.StartsWith("Wall2")));       //TODO: Divide into two or more
        hitlines.Add(items.FindAll(x => x.Name.StartsWith("Pillars")));
        hitlines.Add(items.FindAll(x => x.Name.StartsWith("SideRoad1")));   //TODO: Divide into two or more
        hitlines.Add(items.FindAll(x => x.Name.StartsWith("SideRoad2")));   //TODO: Divide into two or more
        hitlines.Add(items.FindAll(x => x.Name.StartsWith("Roof")));

        return hitlines;
    }

    public IEnumerator StartPainting(List<HitlineClass> items)
    {
        float i = 0.0f;
        float count = items.Count;
        DataManager.Instance.endDictionary = 0;
        DataManager.Instance.startDictionary = 0;
        DataManager.Instance.indexI = 0;
        // DataManager.Instance.DataPair.Clear();
        foreach (HitlineClass s in items)
        {
            string Data = JsonUtility.ToJson(s);
            DataManager.Instance.UpdateOldDictionary(Data);
            GetLine(s);
            i = i + 1.0f;
            float val = (i / count) * 100.0f;
            tunnelSceneFlowManager.UpdatePaintText((int)val);
            if (i % 250 == 0)
            {
                yield return new WaitForSeconds(0.01f);
            }
        }

        DataManager.Instance.EndLoadingConnect();
    }

    public IEnumerator StartPaintingAsync(List<HitlineClass> items)
    {
        float count = items.Count;
        Debug.Log($"[TEST] Count: {count}");
        for (int i = 0; i < count; i++)
        {
            HitlineClass s = items[i];
            string data = JsonUtility.ToJson(s);
            DataManager.Instance.UpdateOldDictionary(data);

            Color color = new Color(s.colorData.r, s.colorData.g, s.colorData.b, s.colorData.a);
            paintSphere.Color = color;
            paintSphere.Radius = s.radius;
            if (s.isLine)
            {
                GetLine(s);
            }
            else
            {
                GetPoint(s);
            }

            yield return new WaitForSeconds(0.015f);
        }
    }

    public void GetLine(HitlineClass hitlineClass)
    {
        var iHitlines = GetComponentsInChildren<IHitLine>();
        foreach (var hitLine in iHitlines)
        {
            if ((UnityEngine.Object)hitLine != this)
            {
                hitLine.HandleHitLine(hitlineClass.preview, hitlineClass.priority, hitlineClass.pressure,
                    hitlineClass.seed, hitlineClass.position, hitlineClass.endPosition, hitlineClass.rotation,
                    hitlineClass.clip);
            }
        }
    }


    public void GetPoint(HitlineClass hitpointClass)
    {
        foreach (var hitPoint in GetComponentsInChildren<IHitPoint>())
        {
            if ((UnityEngine.Object)hitPoint != this)
            {
                hitPoint.HandleHitPoint(hitpointClass.preview, hitpointClass.priority, hitpointClass.pressure,
                    hitpointClass.seed, hitpointClass.position, hitpointClass.rotation);
            }
            // Submit the hit point
        }
    }
}
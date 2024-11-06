using Newtonsoft.Json;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class ListHitLines
{
    public List<HitlineClass> hitlineClasses;
    public string _id;

    public ListHitLines()
    {
        hitlineClasses = new List<HitlineClass>();
    }
}


public class LoadData
{
    public string hitlineClasses;
    public string _id;
}

public class DataToSend
{
    public List<String> hitlineClasses;

    public DataToSend()
    {
        hitlineClasses = new List<string>();
    }
}

public class CompressedDataToSend
{
    public string hitlineClasses;
}


public class DataManager : Singleton<DataManager>, IPunObservable
{
    string URL = "https://amw.kiwicreations.io:3000/paint";
    public int indexI = 0;
    public Dictionary<String, String> OldDataPair = new Dictionary<string, string>();
    public Dictionary<String, String> NewDataPair = new Dictionary<string, string>();
    public ListHitLines hitlines;
    string Key = "SaveData";
    public bool isPainter;
    public bool isSelectionDone;
    public AutoRoomTest roomTest;
    public PaintStart paintStart;
    public bool isJsonLoaded;
    public AutoRoomTest autoRoomTest;
    public bool isTeleportedToTunnel;
    public Controller controller;

    public GameObject tunnel;
    public Transform tunnelParent;

    public LoadTunnel LoadTunnel;
    public int startDictionary = 0;
    public int endDictionary = 0;

    public void LoadTunnelCall()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            return;
        }

        //if (isTeleportedToTunnel)
        //{
        //    return;
        //}
        Debug.Log("Data manager: load tunnel called");
        if (LoadTunnel == null)
        {
            LoadTunnel = FindAnyObjectByType<LoadTunnel>();
        }

        LoadTunnel.LoadTunnelCall();
    }

    int i = 0;
    public void UpdateOldDictionary(string s)
    {
        if (OldDataPair.ContainsKey(i.ToString()))
        {
            OldDataPair[i.ToString()] = s;
        }
        else
        {
            OldDataPair.Add(i.ToString(), s);
            indexI++;
            i++;
        }
        // string temp = PaintData.AddPrefixValue(DataPair[(indexI - 1).ToString()]);

        Debug.Log(i);
        Debug.Log($"hitline size = {hitlines.hitlineClasses.Count}");
        Debug.Log($"Dictionary size = {OldDataPair.Count}");
    }

    int j = 0;
    public void UpdateNewDictionary(string s)
    {
        if (NewDataPair.ContainsKey(j.ToString()))
        {
            NewDataPair[j.ToString()] = s;
        }
        else
        {
            NewDataPair.Add(j.ToString(), s);
            j++;
        }
    }


    public void SaveFile(List<String> strings)
    {
        string path = Path.Combine(Application.persistentDataPath, "Test.txt");
        Debug.Log(path);
        using (StreamWriter writer = new StreamWriter(path))
        {
            foreach (string s in strings)
            {
                writer.WriteLine(s);
            }
        }

        Debug.Log("strings saved");
    }

    public void InstantiateTunnel()
    {
        tunnel = Instantiate(tunnel, tunnelParent);
    }

    public void SetController(Controller _controller)
    {
        controller = _controller;
    }

    public void SaveJsonData()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            return;
        }

        if (isAPICalled)
        {
            this.Invoke(() =>
            {
                Debug.Log("Recall For Save Data");
                SaveJsonData();
            }, 1.0f);
            return;
        }

        CallApi();
    }

    public void SaveDataPlayerLeft(Action func = null)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            return;
        }

        Debug.Log("Player Left API Call Save Data");
        PhotonNetwork.RemoveBufferedRPCs();
        if (PhotonNetwork.PlayerList.Length < 2)
        {
            if (isAPICalled)
            {
                this.Invoke(() =>
                {
                    Debug.Log("Player Left API Call Save Data re call");
                    SaveDataPlayerLeft(func);
                }, 1.0f);
                return;
            }

            CallApi(func);
        }
        else
        {
            this.Invoke(() => { func?.Invoke(); }, 5.0f);
        }
    }


    public bool isAPICalled;
    int callCount = 0;

    // ReSharper disable Unity.PerformanceAnalysis
    [ContextMenu(nameof(CallApi))]
    public async void CallApi(Action func = null)
    {
        if (isAPICalled)
        {
            Debug.Log("Return from save Data");
            return;
        }

        Debug.Log($"StartDictionary: {startDictionary} || Datapair Count: {NewDataPair.Count}");
        if (startDictionary >= NewDataPair.Count)
        {
            Debug.Log($"Start Greater then end save return");
            func?.Invoke();
            return;
        }

        Dictionary<String, String> dataSender = new Dictionary<string, string>();
        //int index = indexI;
        int index = NewDataPair.Count;
        for (int i = startDictionary; i < index; i++)
        {
            dataSender.Add(i.ToString(), NewDataPair[i.ToString()]);
        }

        //foreach (var item in NewDataPair.Values)
        //{
        //    dataSender.Add(i.ToString(), item);
        //}


        Debug.Log($"Data Created Calling APi");
        isAPICalled = true;
        await ApiHelper.PostFormAsync( /*"https://dev.kiwicreations.io:3000/paint"*/URL,
            success =>
            {
                callCount = 0;
                func?.Invoke();
                isAPICalled = false;
                Debug.Log("Data Posted");
            },
            error =>
            {
                Debug.Log($"Data Error posting {error}");
                if (func != null)
                {
                    callCount++;
                    if (callCount == 3)
                    {
                        if (SceneManager.GetActiveScene().buildIndex == 2)
                        {
                            TunnelSceneFlowManager g = FindObjectOfType<TunnelSceneFlowManager>();
                            g.ShowError($"API Error {error}.\n " +
                                        $"We are retrying to save data do you wish to continue without saving your progress?");
                        }
                    }
                }

                isAPICalled = false;
                CallApi(func);
                Debug.LogError("Not able to post data");
            },
            dataSender, index);
    }

    public void AddToHitLine(HitlineClass hitline)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            return;
        }

        string data = JsonUtility.ToJson(hitline);
        UpdateNewDictionary(data);
        hitlines.hitlineClasses.Add(hitline);
    }

    public void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.LoadScene(1);
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(gameObject);
    }

    public void Start_Call()
    {
        hitlines = new ListHitLines();
        LoadJsonData();
    }

    public void GetstringData(string s)
    {
        string a = CompressString.StringCompressor.CompressString(s);
        Debug.Log(a);
    }

    public void LoadJsonData()
    {
        isJsonLoaded = false;
        StartCoroutine(ApiHelper.GetWithPercentage( /*"https://dev.kiwicreations.io:3000/paint"*/URL, (string data) =>
        {
            isJsonLoaded = true;
            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            hitlines = new ListHitLines();
            hitlines = JsonConvert.DeserializeObject<ListHitLines>(data);

            //Debug.Log($"[Test] Total Count = {hitlines.hitlineClasses.Count}");
            //hitlines.hitlineClasses = RemoveDuplicates(hitlines.hitlineClasses);
            //Debug.Log($"[Test] New Count = {hitlines.hitlineClasses.Count}");

            StartPainting();
        }, (string error) => { autoRoomTest?.ShowError(error + "Not able to load API Data Press ok to refresh game"); }));
    }

    private List<HitlineClass> RemoveDuplicates(List<HitlineClass> list)
    {
        List<HitlineClass> temp = new();

        foreach (var item in list)
        {
            if (!temp.Contains(item))
            {
                temp.Add(item);
            }
        }

        return temp;
    }

    public void UpdateLoadingPercentage(float perc)
    {
        if (paintStart == null)
        {
            paintStart = FindAnyObjectByType<PaintStart>();
        }

        paintStart.tunnelSceneFlowManager.UpdateLoadText((int)perc);
    }

    public void StartPainting()
    {
        endDictionary = 0;
        startDictionary = 0;
        indexI = 0;
        j = 0;
        OldDataPair.Clear();
        NewDataPair.Clear();
        if (hitlines == null)
        {
            EndLoadingConnect();
            Paintedunnel();
            return;
        }

        if (hitlines.hitlineClasses.Count == 0)
        {
            EndLoadingConnect();
            Paintedunnel();
            return;
        }

        if (paintStart == null)
        {
            paintStart = FindAnyObjectByType<PaintStart>();
        }

        // paintStart.StartPaint(hitlines.hitlineClasses);
        Paintedunnel();
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(3);
        paintStart.StartPaint(hitlines.hitlineClasses);
    }

    public void EndLoadingConnect()
    {
        //NewDataPair.AddRange(OldDataPair);
        //startDictionary = DataPair.Count;
        //if (paintStart != null) Destroy(paintStart.gameObject);
        //hitlines.hitlineClasses.Clear();
        //tunnelEnterance.SetActive(true);
        // TunnelSceneFlowManager g = FindObjectOfType<TunnelSceneFlowManager>();
        // g.TunnelPainted();
        // Paintedunnel();
    }

    public void Paintedunnel()
    {
        TunnelSceneFlowManager g = FindObjectOfType<TunnelSceneFlowManager>();
        g.TunnelPainted();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(startDictionary);
            stream.SendNext(isAPICalled);
            stream.SendNext(indexI);
            Debug.Log($"updating Start DIC: {startDictionary}");
        }
        else
        {
            startDictionary = (int)stream.ReceiveNext();
            isAPICalled = (bool)stream.ReceiveNext();
            indexI = (int)stream.ReceiveNext();
            Debug.Log($"Start DIC: {startDictionary}");
        }
    }
}
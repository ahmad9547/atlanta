using Avatars.WebGLMovement.MouseControll;
using PaintCore;
using PaintIn3D;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    #region Singleton
    //public static Controller Instance { get; private set; }

    private int origFR;
    public PhotonView photonView;
    public PaintSynchScript paintSynchScript;
    public  bool isAvatarSetupDone;
    private void Awake()
    {
        //Instance = this;

        origFR = Application.targetFrameRate;

        Application.targetFrameRate = 20;
    }

    private void OnDestroy()
    {
        Application.targetFrameRate = origFR;
    }

    #endregion

    public GameObject loading;

    [Header("Paint Components")]
    [SerializeField] public CwPaintSphere paintSphere;
    [SerializeField] private CwPointerMouse mouseControl;

    [Header("Spray Can Components")]
    [SerializeField] public SprayCanVisual SprayCanVisual;
    [SerializeField] public SprayCanMovement SprayCanMovement;

    [Header("Flow Settings")]
    public float minFlowValue;
    public float defaultFlowValue;
    public float maxFlowValue;

    [Header("Read Only")]
    [SerializeField] private float currentFlowValue;
    [SerializeField] private bool _hasTripped = false;

    public List<GameObject> lastDrawnGameObjects = new();
    public GameObject currentMeshTexture = null;
    public MaxStack<List<GameObject>> undoStack = new(10);
    public MaxStack<List<GameObject>> redoStack = new(10);

    protected bool HasTripped
    {
        get => _hasTripped;
        set
        {
            _hasTripped = value;
            Debug.Log($"HasTripped: {_hasTripped}");
        }
    }

    public void SaveData()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                DataManager.Instance.SaveJsonData();
            }
            this.Invoke(() => { SaveData(); }, 5f);
        }
        
    }

    public void Start1()
    {
        //yield return new WaitForSecondsRealtime(0.35f);
        if (photonView.IsMine)
        {
            loading?.SetActive(true);
            EventManager.Instance.SetControllerFunction(this);
        }
        if (photonView.IsMine)
        {
            loading?.SetActive(false);
            HideCursor();
            SetPaintFlow(defaultFlowValue);
            StopDrawing();
        }
        this.Invoke(() => { SaveData(); }, 10f);
        isAvatarSetupDone = true;
    }

    public void SetPaintFlow(float flowValue)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        currentFlowValue = flowValue;
        SetDrawingRadius(Mathf.Clamp(currentFlowValue, minFlowValue, maxFlowValue));
    }

    public void SetDrawingRadius(float radius)
    {
        paintSphere.Radius = radius;
    }

    public void Trip()
    {
        HasTripped = true;
        StopDrawing();
    }

    public bool IsDrawing()
    {
        return mouseControl.enabled;
    }

    public static Coroutine drawRoutine = null;

    public void StartDrawing()
    {
        if (isWorking) return;

        if (drawRoutine != null)
        {
            StopCoroutine(drawRoutine);
            drawRoutine = null;
        }

        drawRoutine = StartCoroutine(Draw());
    }

    private IEnumerator Draw()
    {
        SprayCanVisual?.ShowFingerAnim();
        //yield return new WaitForSecondsRealtime(0.2f);
        ShowVFX();
        yield return new WaitForSecondsRealtime(0.01f);
        ShowSpot();
    }

    private void ShowVFX()
    {
        SprayCanVisual?.EnableSprayVFX();
    }

    private void ShowSpot()
    {
        mouseControl.enabled = true;
    }

    public void StopDrawing()
    {
        if (drawRoutine != null)
        {
            StopCoroutine(drawRoutine);
            drawRoutine = null;
        }

        mouseControl.enabled = false;
        SprayCanVisual?.DisableSprayVFX();
    }

    public void SetColor(Color32 color)
    {
        paintSphere.Color = color;
        if (photonView.IsMine)
        {
            SprayCanVisual?.SetColor(color);
            paintSynchScript.SynchColor(color);
        }
    }

    bool isWorking = false;

    public void Undo()
    {
        if (photonView.IsMine)
        {
            if (loading.activeSelf) return;

            photonView.RPC(nameof(UndoRPCCall), RpcTarget.All);
        }
    }

    [PunRPC]
    public void UndoRPCCall()
    {
        if (undoStack.Count > 0)
        {
            StartCoroutine(UndoRoutine());
        }
    }

    private IEnumerator UndoRoutine()
    {
        isWorking = true;

        Trip();
        GameObject.FindObjectOfType<PlayerMouseLook>().enabled = false;

        loading?.SetActive(true);
        var item = undoStack.Pop();
        yield return new WaitForEndOfFrame();
        item.ForEach(x => x.GetComponent<CwPaintableMeshTexture>().Undo());
        yield return new WaitForSecondsRealtime(1.0f);
        GameObject.FindObjectOfType<PlayerMouseLook>().enabled = true;

        loading?.SetActive(false);
        redoStack.Push(item);

        isWorking = false;
    }

    public void Redo()
    {
        if (photonView.IsMine)
        {
            if (loading.activeSelf) return;

            photonView.RPC(nameof(RedoRPCCall), RpcTarget.All);

        }
    }

    [PunRPC]
    public void RedoRPCCall()
    {
        if (redoStack.Count > 0)
        {
            StartCoroutine(RedoRoutine());
        }
    }

    private IEnumerator RedoRoutine()
    {
        isWorking = true;

        Trip();
        GameObject.FindObjectOfType<PlayerMouseLook>().enabled = false;

        loading.SetActive(true);
        var item = redoStack.Pop();
        yield return new WaitForEndOfFrame();
        item.ForEach(x => x.GetComponent<CwPaintableMeshTexture>().Redo());
        yield return new WaitForSecondsRealtime(1.0f);
        GameObject.FindObjectOfType<PlayerMouseLook>().enabled = true;
        undoStack.Push(item);
        loading.SetActive(false);

        isWorking = false;

    }

    public bool isCursorLocked = false;

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorLocked = false;
        Trip();
        GetComponent<CwHitScreen>().enabled = false;
        GameObject.FindObjectOfType<PlayerMouseLook>().enabled = false;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
        Trip();
        GetComponent<CwHitScreen>().enabled = true;
        GameObject.FindObjectOfType<PlayerMouseLook>().enabled = true;
    }

    float undoGracePeriod = 1.0f;
    float undoGracePeriodValue = 0.0f;

    private void Update()
    {
        if (!isCursorLocked) return;
        if (isWorking) return;
        if (loading.activeSelf) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (lastDrawnGameObjects.Count > 0 && lastDrawnGameObjects[0] != null)
            {
                var com = lastDrawnGameObjects[0].GetComponent<CwPaintableMeshTexture>();
                if (com != null)
                    com.StoreState();
            }
            //StartDrawing();
        }

        if (Input.GetMouseButtonUp(0)/* && IsDrawing()*/)
        {
            GetComponent<CwHitScreen>().ResetConnections();
            StopDrawing();
            undoStack.Push(lastDrawnGameObjects); 
            lastDrawnGameObjects = new();
        }

        undoGracePeriodValue -= Time.deltaTime;
        if (undoGracePeriodValue < 0)
        {
            undoGracePeriodValue = 0;
        }

        bool isCtrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
#if UNITY_EDITOR
        isCtrl = true;
#endif
        if (isCtrl && Input.GetKeyDown(KeyCode.Z) && undoGracePeriodValue <= 0)
        {
            undoGracePeriodValue = undoGracePeriod;
            Undo();
        }

        if (isCtrl && Input.GetKeyDown(KeyCode.Y) && undoGracePeriodValue <= 0)
        {
            undoGracePeriodValue = undoGracePeriod;
            Redo();
        }
    }
}
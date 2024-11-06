using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearObjects : Singleton<ClearObjects>
{
    //public GameObject eventManager;
    //public GameObject loadTUnnel;
    public GameObject datamanager;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void ClearAll()
    {
        Destroy(datamanager);
        DataManager.Instance = null;
        Destroy(gameObject);
    }
}

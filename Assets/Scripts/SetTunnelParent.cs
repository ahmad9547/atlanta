using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTunnelParent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.tunnelParent = this.gameObject.transform;
    }

  
}

using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
public class PodiumTrigger : MonoBehaviour
{
    [SerializeField] public UISetup InstructionPanel;
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (InstructionPanel.helpPanelRostrum.activeInHierarchy)
            {
                return;
            }
            if (other.transform.GetComponent<PhotonView>().IsMine)
            {
                InstructionPanel.SetRostrumPanel();
            }
        }
    }
    
}
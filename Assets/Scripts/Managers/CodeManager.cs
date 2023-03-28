using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CodeManager : NetworkBehaviour
{
    public static CodeManager Instance { get; private set; }

    public override void OnNetworkSpawn()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void DebugMessage(string message)
    {
        Debug.Log(message);
    }

    public void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    public void Test()
    {
        Debug.Log("Test");
    }
}

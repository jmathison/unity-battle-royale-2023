using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class GameObjectEvent : UnityEvent<GameObject> { }
public class GameManager : NetworkBehaviour
{
    public GameObject localCharacter;
    private string playerName;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        DontDestroyOnLoad(this);
    }
    public void HealOwner(float amount)
    {
        Debug.Log("Healing: " + localCharacter.name + " the amount: " + amount );
        IDamageable damageable = localCharacter.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.Heal(amount);
        else
            Debug.LogWarning("No IDamageable on the gameobject: " + localCharacter.name);
    }

    public void DamageOwner(float amount)
    {
        Debug.Log("Damaging: " + localCharacter.name + " the amount: " + amount);
        IDamageable damageable = localCharacter.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.Heal(amount);
        else
            Debug.LogWarning("No IDamageable on the gameobject: " + localCharacter.name);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeNameServerRpc(ulong clientId, string name)
    {
        Debug.Log("Changing that name");
        Transform playerTransform = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.transform;
        playerTransform.Find("Canvas").Find("Name").GetComponent<TMP_Text>().text = name;
    }

    public void Test()
    {
        ChangeNameServerRpc(NetworkManager.Singleton.LocalClientId, playerName);
    }

    public void ChangeName(string newName)
    {
        playerName = newName;
        Debug.Log(playerName);
    }

    public void SetPlayerCharacter(GameObject character)
    {
        localCharacter = character;
    }
}
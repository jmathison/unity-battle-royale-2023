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

    private IDamageable findIDamageable(Transform search)
    {
        IDamageable damageTarget = null;
        // Search up the tree to find damage script
        while (search && damageTarget == null)
        {
            damageTarget = search.gameObject.GetComponent<IDamageable>();
            search = search.parent;
        }
        return damageTarget;
    }

    public void HealOwner(float amount)
    {
        Debug.Log("Healing: " + localCharacter.name + " the amount: " + amount);
        IDamageable target = findIDamageable(localCharacter.transform);
        if (target != null)
            target.Heal(amount);
        else
            Debug.LogWarning("No IDamageable on the gameobject: " + localCharacter.name);
    }

    public void DamageOwner(float amount)
    {
        Debug.Log("Damaging: " + localCharacter.name + " the amount: " + amount);
        IDamageable target = findIDamageable(localCharacter.transform);
        if (target != null)
            target.Damage(amount);
        else
            Debug.LogWarning("No IDamageable on the gameobject: " + localCharacter.name);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeNameServerRpc(ulong clientId, string name)
    {
        Transform playerTransform = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.transform;
        playerTransform.gameObject.name = name;
        playerTransform.Find("Canvas").Find("Name").GetComponent<TMP_Text>().text = name;
    }

    public void Test()
    {
        ChangeNameServerRpc(NetworkManager.Singleton.LocalClientId, playerName);
    }

    public void ChangeName(string newName)
    {
        if (IsClient && IsOwner)
        {
            playerName = newName;
        }
    }

    public void SetPlayerCharacter(GameObject character)
    {
        localCharacter = character;
    }
}
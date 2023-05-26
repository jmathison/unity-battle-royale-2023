using Unity.Netcode;
using UnityEngine;

public class InventoryManager : NetworkBehaviour
{
    public static InventoryManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void Add(PlayerInventory inventory, Transform itemObject)
    {
        Item item = itemObject.GetComponent<BaseInteractor>().item;
        inventory.Add(item, itemObject);
    }

    public void Add(Transform player, Transform itemObject)
    {
        Item item = itemObject.GetComponent<BaseInteractor>().item;
        player.GetComponent<PlayerInventory>().Add(item, itemObject);
    }

    public void Add(Interactor player, Transform itemObject)
    {
        Item item = itemObject.GetComponent<BaseInteractor>().item;
        player.GetComponent<PlayerInventory>().Add(item, itemObject);
    }

    public void Remove(PlayerInventory inventory, Item item)
    {
        inventory.Remove(item);
    }

    public void Remove(Transform player, Item item)
    {
        player.GetComponent<PlayerInventory>().Remove(item);
    }

    public void Remove(Interactor player, Item item)
    {
        player.GetComponent<PlayerInventory>().Remove(item);
    }

    [ServerRpc]
    public void EquipToPlayerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("Client Id: " + serverRpcParams.Receive.SenderClientId);
    }

    public void EquipWeapon(Transform weaponObject)
    {
        
        Item weapon = weaponObject.GetComponent<BaseInteractor>().item;
        //Get player inventory:
        if (IsClient && IsOwner)
        {
            EquipToPlayerServerRpc();

        }
        //inventory.EquipWeapon(weapon, weaponObject);
    }
}

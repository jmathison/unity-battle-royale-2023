using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
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
        Item item = itemObject.GetComponent<ItemInteractor>().item;
        inventory.Add(item, itemObject);
    }

    public void Add(Transform player, Transform itemObject)
    {
        Item item = itemObject.GetComponent<ItemInteractor>().item;
        player.GetComponent<PlayerInventory>().Add(item, itemObject);
    }

    public void Add(Interactor player, Transform itemObject)
    {
        Item item = itemObject.GetComponent<ItemInteractor>().item;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    [Header("Don't Edit:")]
    public Transform itemContent;
    public Transform inventory;
    public GameObject inventoryItemPrefab;
    private Transform _backPack;

    private void Start()
    {
        _backPack = transform.Find("Backpack");
    }
    public void DisplayItems()
    {
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in items)
        {
            GameObject obj = Instantiate(inventoryItemPrefab, itemContent);
            TMP_Text itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            Image itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            //obj.GetComponent<Button>().onClick.AddListener(() => { GameObject.Find(item.objectName).SendMessage(item.message); } );
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            obj.GetComponent<Button>().onClick = _backPack.Find(itemName.text).GetComponent<ItemInteractor>().onClick;
        }
    }

    public void Add(Item item, Transform itemTransform)
    {
        //Add the item to the list of items.
        items.Add(item);
        itemTransform.SetParent(_backPack);
        itemTransform.name = item.name;
        //Hide the item from visibility and interaction
        itemTransform.position = new Vector3(2500, 2500, 2500);
        if (itemTransform.GetComponent<Renderer>())
            itemTransform.GetComponent<Renderer>().enabled = false;
        //Display the items
        if (itemContent.gameObject.activeSelf)
            DisplayItems();
    }
    public void Remove(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<ItemInteractor>().item;
        items.Remove(item);
        Destroy(itemObject);
        if (itemContent.gameObject.activeSelf)
            DisplayItems();
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        //This system does not work correctly. We must associate the game object specifically that we want to delete.
        Destroy(_backPack.Find(item.name).gameObject);
        if (itemContent.gameObject.activeSelf)
            DisplayItems();
    }

    private void OnInventory()
    {
        if (inventory.gameObject.activeSelf)
        {
            inventory.gameObject.SetActive(false);
            transform.GetComponent<StarterAssets.ThirdPersonController>().LockCameraPosition = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            inventory.gameObject.SetActive(true);
            DisplayItems();
            transform.GetComponent<StarterAssets.ThirdPersonController>().LockCameraPosition = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

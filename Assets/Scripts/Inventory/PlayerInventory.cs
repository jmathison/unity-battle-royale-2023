using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerInventory : NetworkBehaviour
{
    public List<Item> items = new List<Item>();

    [Header("Don't Edit:")]
    public Transform itemContent;
    public Transform inventory;
    public Transform grip;
    public GameObject inventoryItemPrefab;
    private Transform _backPack;
    private GameObject _weaponObject;

    private void Start()
    {
        _backPack = transform.Find("Backpack");
        if (grip.GetComponentInChildren<WeaponController>() != null )
            _weaponObject = grip.GetComponentInChildren<WeaponController>().gameObject;
    }
    private void DisplayItems()
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
            if (_backPack.Find(itemName.text)) {
                if (_backPack.Find(itemName.text).GetComponent<BaseInteractor>())
                {
                    obj.GetComponent<Button>().onClick = _backPack.Find(itemName.text).GetComponent<BaseInteractor>().onClick;
                }
            }
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
        Item item = itemObject.GetComponent<BaseInteractor>().item;
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

    public void EquipWeapon(Weapon weapon, Transform weaponTransform)
    {
        WeaponInteractor interactor = weaponTransform.GetComponent<WeaponInteractor>();
        GameObject weaponPrefab = interactor.item.weaponPrefab;
        if (_weaponObject)
        {
            Destroy(_weaponObject);
        }
        _weaponObject = Instantiate(weaponPrefab);
        weaponTransform.SetParent(grip, false);
        weaponTransform.position = new Vector3(0, 0, 0);
    }

    private void OnInventory()
    {
        inventory.gameObject.SetActive(!inventory.gameObject.activeSelf);
        if (inventory.gameObject.activeSelf)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}

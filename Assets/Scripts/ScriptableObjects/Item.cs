using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
[IconAttribute("ItemIcon")]
//This should be converted to a Consumable and Item should be the main derivative.
public class Item : ScriptableObject
{
    public string id = Guid.NewGuid().ToString();
    public string itemName;
    public Sprite icon;
}

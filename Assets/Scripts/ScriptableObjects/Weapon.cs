using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Create New Weapon")]
[IconAttribute("ItemIcon")]
public class Weapon : Item
{
    public GameObject weaponPrefab;
}

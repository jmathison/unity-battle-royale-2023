using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum PowerupType
{
    None,
    Health,
    Damage,
    Armor
}

[CreateAssetMenu(fileName = "New Powerup", menuName = "Powerup/Create New Powerup")]
public class Powerup : ScriptableObject
{
    public int id;
    public string powerupName;
    public Sprite icon;
    public PowerupType powerupType = PowerupType.None;
}

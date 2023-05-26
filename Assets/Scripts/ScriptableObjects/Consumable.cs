using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This should be converted to a Consumable and Item should be the main derivative.
public abstract class Consumable : Item
{
    public int uses = 1;
}

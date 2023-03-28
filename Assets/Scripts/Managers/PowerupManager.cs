using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance { get; private set; }
    public List<Powerup> Powerups = new List<Powerup>();

    public Interactor interactor;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void Add(Powerup powerup) {
        Powerups.Add(powerup);
    }

    public void Remove(Powerup powerup) {
        Powerups.Remove(powerup);
    }
}

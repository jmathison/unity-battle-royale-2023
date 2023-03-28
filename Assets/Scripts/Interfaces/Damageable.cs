using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public interface IDamageable
{
    public void Damage(float damage);
    public void Heal(float damage);
    public bool IsDead();
    public float GetHealth();
    public void SetHealth(float newHealth);
}

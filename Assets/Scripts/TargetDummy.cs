using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour, IDamageable
{
    void IDamageable.Damage(float damage)
    {
        Debug.Log(damage);
    }

    float IDamageable.GetHealth()
    {
        return 0;
    }

    void IDamageable.Heal(float damage)
    {
        Debug.Log(damage);
    }

    bool IDamageable.IsDead()
    {
        return false;
    }

    void IDamageable.SetHealth(float newHealth)
    {
        Debug.Log(newHealth);
    }

}

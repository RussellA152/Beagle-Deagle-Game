using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    protected float currentHealth;
    protected bool isDead;

    [Range(0f, 2500f)]
    [SerializeField] protected float maxHealth;

    protected virtual void OnEnable()
    {
        isDead = false;
        currentHealth = maxHealth;
    }

    public virtual float GetCurrentHealth()
    {
        return currentHealth;
    }

    public virtual float GetMaxHealth()
    {
        return maxHealth;
    }

    // add or subtract from health count
    public virtual void ModifyHealth(float amount)
    {
        // if this health modification will exceed the max potential health, then just set the current health to max
        if (currentHealth + amount > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // if this health modification will drop the health to 0 or below, then call OnDeath()
        else if (currentHealth + amount <= 0f)
        {
            currentHealth = 0;
            isDead = true;
        }

        else
        {
            currentHealth += amount;
        }

    }

    // do something when this entity dies
    public bool IsDead()
    {
        return isDead;
    }
}

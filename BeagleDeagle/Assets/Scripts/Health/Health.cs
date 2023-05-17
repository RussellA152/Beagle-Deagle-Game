using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health: MonoBehaviour
{
    private float currentHealth;
    private bool isDead;

    [Range(0f, 2500f)]
    [SerializeField] private float maxHealth;

    private void OnEnable()
    {
        isDead = false;
        currentHealth = maxHealth;
    }

    // add or subtract from health count
    public void ModifyHealth(float amount)
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
    public bool IsDead() {
        return isDead;
    }
}

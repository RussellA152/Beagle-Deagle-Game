using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour, IHealth, IEnemyDataUpdatable
{
    protected float currentHealth;
    protected bool isDead;

    [SerializeField]
    protected EnemyData enemyData;

    protected virtual void OnEnable()
    {
        isDead = false;
        currentHealth = enemyData.maxHealth;
    }

    public virtual float GetCurrentHealth()
    {
        return currentHealth;
    }

    public virtual float GetMaxHealth()
    {
        return enemyData.maxHealth;
    }

    // add or subtract from health count
    public virtual void ModifyHealth(float amount)
    {
        // if this health modification will exceed the max potential health, then just set the current health to max
        if (currentHealth + amount > enemyData.maxHealth)
        {
            currentHealth = enemyData.maxHealth;
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

    public virtual void ModifyMaxHealth(float amount)
    {
        //enemyData.maxHealth += amount;
    }

    // do something when this entity dies
    public bool IsDead()
    {
        return isDead;
    }

    public void UpdateScriptableObject(EnemyData scriptableObject)
    {
        enemyData = scriptableObject;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : Health, IDataUpdatable<EnemyData>
{
    [SerializeField]
    private EnemyData enemyScriptableObject;

    protected override void OnEnable()
    {
        isDead = false;
        currentHealth = enemyScriptableObject.maxHealth;
    }

    // add or subtract from health count
    public override void ModifyHealth(float amount)
    {
        // if this health modification will exceed the max potential health, then just set the current health to max
        if (currentHealth + amount > enemyScriptableObject.maxHealth)
        {
            currentHealth = enemyScriptableObject.maxHealth;
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
    public void UpdateConfiguration(EnemyData scriptableObject)
    {
        enemyScriptableObject = scriptableObject;
    }
}

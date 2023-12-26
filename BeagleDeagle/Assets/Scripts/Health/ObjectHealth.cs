using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// ObjectHealth are health scripts placed on non-living beings like barrels or defended objects during objectives.
/// The difference between this and PlayerHealth or EnemyHealth is that this script does not feature any bonus max health modifiers.
/// 
public class ObjectHealth : MonoBehaviour, IHealth
{
    // The current health of this enemy
    private float _currentHealth;

    [SerializeField, Range(1f, 5000f)] 
    private float maxHealth;
    
    public event Action onDeath;

    private bool _isDead;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        _currentHealth = maxHealth;
        _isDead = false;
    }
    

    public virtual void ModifyHealth(float amount)
    {
        // Calculate the potential new health value
        float newHealth = _currentHealth + amount;

        // Clamp the new health value between 0 and the maximum potential health (including any max health modifiers)
        newHealth = Mathf.Clamp(newHealth, 0f, maxHealth);

        // Check if the new health value is zero or below
        if (newHealth <= 0f)
        {
            _currentHealth = 0f;

            _isDead = true;
            
            InvokeDeathEvent();
            
        }
        else
        {
            _currentHealth = newHealth;
        }
    }
    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void InvokeDeathEvent()
    {
        onDeath?.Invoke();
    }

    public bool IsDead()
    {
        return _isDead;
    }
}

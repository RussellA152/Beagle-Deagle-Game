using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HealOnCollision))]
public class FoodPickup : MonoBehaviour
{
    [SerializeField] private LayerMask whoCanPickup;

    private IStatusEffect[] _statusEffects;
    [SerializeField] private StatusEffectTypes statusEffectData;

    // What does this food do to who ever picked it up?
    [SerializeField] private UnityEvent<GameObject> onFoodPickup;

    private void Awake()
    {
        _statusEffects = GetComponents<IStatusEffect>();
    }

    private void Start()
    {
        foreach (IStatusEffect statusEffect in _statusEffects)
        {
            statusEffect.UpdateWeaponType(statusEffectData);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Activate all effects on food pickup
        if ((whoCanPickup.value & (1 << other.gameObject.layer)) > 0)
        {
            onFoodPickup.Invoke(other.gameObject);
            
            gameObject.SetActive(false);
        }
    }
}


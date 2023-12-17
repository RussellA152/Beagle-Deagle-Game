using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealEffect", menuName = "ScriptableObjects/StatusEffects/Heal")]
public class HealData : StatusEffectData
{
    // If a food pick up, use a sprite 
    public Sprite healItemSprite;
    
    [Range(1f, 5000f)]
    // The amount to heal a player or enemy
    public float healAmount;
}

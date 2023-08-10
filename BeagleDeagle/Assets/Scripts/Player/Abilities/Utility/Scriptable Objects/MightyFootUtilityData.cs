using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMightyFoot", menuName = "ScriptableObjects/Ability/Utility/MightyFoot")]
public class MightyFootUtilityData : UtilityAbilityData
{
    [Range(100, 300)]
    // Number of enemies that the mighty foot can hit
    // This is inside of the utility data because we won't be changing it with different variants of a mighty foot
    public int numberOfEnemiesCanHit;
    
    // Prefab of mighty foot will come from BulletType
    public BulletTypeData bulletType;
    
    [Header("Offset From Player Position")]
    public Vector2 offset; // Offset applied to Mighty Foot projectile when this ability is activated
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMightyFoot", menuName = "ScriptableObjects/Ability/Utility/MightyFoot")]
public class MightyFootUtilityData : UtilityAbilityData
{
    [Header("Projectile Data")]
    public MightyFootBulletData mightyFootData;

    [Header("Offset From Player Position")]
    public Vector2 offset; // Offset applied to Mighty Foot projectile when this ability is activated
    
}

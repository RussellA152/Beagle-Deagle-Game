using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMightyFoot", menuName = "ScriptableObjects/Ability/Utility/MightyFoot")]
public class MightyFootUtilityData : UtilityAbilityData
{
    [Header("Projectile Data")]
    [RestrictedPrefab(typeof(MightyFootBullet))]
    public GameObject mightyFootPrefab;
    
    public MightyFootBulletData mightyFootData;
    public StunData stunData;

    [Header("Offset From Player Position")]
    public Vector2 offset; // Offset applied to Mighty Foot projectile when this ability is activated
    
}

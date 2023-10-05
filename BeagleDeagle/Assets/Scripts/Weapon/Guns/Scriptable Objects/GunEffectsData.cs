using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewGunEffects", menuName = "ScriptableObjects/Weapon/Effects")]
public class GunEffectsData : ScriptableObject
{
    [Header("Weapon Appearance")]
    public Sprite weaponSprite;
    
    [Header("Muzzle Flash")]
    public Sprite muzzleFlashSprite;
    public Vector2 muzzleFlashPosition;
    [Range(0.01f, 0.25f)] public float muzzleFlashDuration;
}

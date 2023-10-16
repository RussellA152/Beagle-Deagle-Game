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

    [Header("Gun Sounds")]
    public AudioClip[] fireClips;
    

    public AudioClip reloadStartClip;

    public AudioClip reloadFinishedClip;

    // How long after playing the reload finished clip should this weapon wait to play its slide sound clip
    public AudioClip reloadSlideClip;
    
    [Range(0.1f, 1f)] public float fireSoundVolume;
    [Range(0.1f, 1f)] public float reloadSoundVolume;
}

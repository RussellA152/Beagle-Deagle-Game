using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public AudioClip[] reloadLoopClips;

    public AudioClip reloadFinishedClip;

    // How long after playing the reload finished clip should this weapon wait to play its cock sound clip
    //[Range(0.25f, 1.25f)] public float reloadCockSoundDelay = 0.25f;
    public AudioClip reloadCockClip;
}

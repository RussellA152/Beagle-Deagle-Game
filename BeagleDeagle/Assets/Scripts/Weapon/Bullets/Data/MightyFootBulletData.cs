using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "ScriptableObjects/Projectile/MightyFootBullet")]
public class MightyFootBulletData : BulletData
{
    [Header("Utility Ability That Activates This")]
    [SerializeField]
    private UtilityAbilityData utilityAbilityData;

    [Range(0f, 150f)]
    public int numEnemiesCanHit;
    
    public override float GetLifeTime()
    {
        return utilityAbilityData.duration;
    }

}

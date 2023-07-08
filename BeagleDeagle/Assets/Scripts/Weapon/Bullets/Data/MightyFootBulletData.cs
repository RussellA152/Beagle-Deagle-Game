
using UnityEngine;

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

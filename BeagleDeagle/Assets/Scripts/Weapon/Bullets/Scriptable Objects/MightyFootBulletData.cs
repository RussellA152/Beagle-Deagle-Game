
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "ScriptableObjects/Projectile/MightyFootBullet")]
public class MightyFootBulletData : BulletData
{
    [Header("Utility Ability That Activates This")]
    [SerializeField] private UtilityAbilityData mightyFootAbilityData;

    [Header("Screen Shake")]
    public ScreenShakeData screenShakeData;
    
    [Range(100, 300)]
    // Number of enemies that the mighty foot can hit
    public int numberOfEnemiesCanHit;
    
    public override float GetLifeTime()
    {
        return mightyFootAbilityData.duration;
    }

}

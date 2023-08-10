
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "ScriptableObjects/Projectile/MightyFootBullet")]
public class MightyFootBulletData : BulletData
{
    [Header("Utility Ability That Activates This")]
    [SerializeField] private UtilityAbilityData mightyFootAbilityData;

    public override float GetLifeTime()
    {
        return mightyFootAbilityData.duration;
    }

}

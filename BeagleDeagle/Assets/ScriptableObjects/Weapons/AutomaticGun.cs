using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon/AutomaticGun")]
public class AutomaticGun : GunData
{
    private float lastFireTime;

    public override void OnEnable()
    {
        base.OnEnable();
        lastFireTime = 0f;
    }
    public override void Fire(Bullet bullet, float fireRate, float spread)
    {
        SpawnBullet(bullet, bulletSpawnPoint, spread);
        lastFireTime = Time.time;


    }

    public override bool CheckIfCanFire()
    {
        if (Time.time - lastFireTime > 1f / fireRate)
        {
            return true;

        }
        else
        {
            return false;
        }
    }
}

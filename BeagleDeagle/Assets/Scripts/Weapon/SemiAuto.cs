using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAuto : GunWeapon
{
    private float lastFireTime = 0f;


    public override void Fire()
    {
        if (Time.time - lastFireTime > 1f / fireRate)
        {
            base.SpawnBullet();
            lastFireTime = Time.time;

        }
        
    }
}

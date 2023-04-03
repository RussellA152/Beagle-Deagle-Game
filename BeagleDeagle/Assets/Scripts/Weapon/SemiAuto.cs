using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAuto : GunWeapon
{
    private float lastFireTime = 0f;


    public override void FireRate()
    {
        if (Time.time - lastFireTime > 1f / fireRate)
        {
            // Fire the gun
            base.Shoot();

            lastFireTime = Time.time;

        }
    }
}

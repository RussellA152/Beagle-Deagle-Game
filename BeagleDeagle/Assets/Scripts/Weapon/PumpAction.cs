using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpAction : GunWeapon
{
    public bool commitToReload;

    public override void FireRate()
    {
        throw new System.NotImplementedException();
    }

    public override void Shoot()
    {
        // if player has no ammo in reserve, force them to reload
        if (bulletsLoaded <= 0f)
        {
            Debug.Log("RELOADING!");
            Reload();
        }
        // if this weapon doesn't need to commit to a reload, then let the player shoot
        else if (!commitToReload && bulletsLoaded >= 0)
        {
            Debug.Log("SHOOT!");

            //CancelReload();

            SpawnBullet();
            bulletsShot++;
            bulletsLoaded--;
        }
        else if (commitToReload && !isReloading)
        {
            Debug.Log("SHOOT!");

            SpawnBullet();
            bulletsShot++;
            bulletsLoaded--;
        }
    }

    public void CancelReload()
    {
        // this won't work until we add a coroutine variable * 
        StopCoroutine(WaitReload());
    }
}

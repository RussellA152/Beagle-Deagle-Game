//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PumpAction : GunWeapon<GunData>
//{
//    public bool commitToReload;

//    public override void Fire()
//    {
//        throw new System.NotImplementedException();
//    }

//    public override bool CheckIfCanShoot()
//    {
//        // if player has no ammo in reserve, force them to reload
//        if (bulletsLoaded <= 0f)
//        {
//            Debug.Log("RELOADING!");
//            Reload();

//            return false;
//        }
//        // if this weapon doesn't need to commit to a reload, then let the player shoot
//        else if (!commitToReload && bulletsLoaded >= 0)
//        {
//            Debug.Log("SHOOT!");

//            CancelReload();

//            SpawnBullet();

//            return true;
//        }
//        else if (!isReloading)
//        {
//            Debug.Log("SHOOT!");

//            SpawnBullet();

//            return true;
//        }
//        return false;
//    }

//    public void CancelReload()
//    {
//        // this won't work until we add a coroutine variable * 
//        StopCoroutine(WaitReload());
//    }
//}

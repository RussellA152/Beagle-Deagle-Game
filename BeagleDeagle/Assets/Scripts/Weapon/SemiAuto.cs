using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAuto : GunWeapon
{
    public override void Reload()
    {
        if (!isReloading)
            StartCoroutine(WaitReload());
    }

    public override void Shoot()
    {
        base.Shoot();

        GameObject bulletInst = Instantiate(bulletPrefab, bulletSpawnPoint.position, this.transform.rotation);
    }

    IEnumerator WaitReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(totalReloadTime);
        bulletsLoaded = magazineSize;
        isReloading = false;
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using static UnityEngine.InputSystem.InputAction;

//public abstract class GunWeapon<T> : MonoBehaviour, IGunDataUpdatable where T : GunData
//{
//    private PlayerInput playerInput;

//    public T weaponData;

//    [HideInInspector]
//    public int bulletsShot; // how much ammo has the player shot since the last reload or refill?
//    [HideInInspector]
//    public int bulletsLoaded; // how much ammo is currently in the magazine?

//    //[HideInInspector]
//    //public int ammoInReserve; // how much ammo is currently in capacity?

//    [Header("Bullet Logic")]
//    public Transform bulletSpawnPoint; // where does this bullet get shot from? (i.e the barrel)


//    private float shootInput; // input for shooting

//    [HideInInspector]
//    public bool actuallyShooting; // is the player shooting (i.e, not idle or reloading or just moving)

//    [HideInInspector]
//    public bool isReloading;


//    private void Start()
//    {
//        bulletsShot = 0;
//        bulletsLoaded = weaponData.magazineSize;
//        //ammoInReserve = weaponData.maxAmmoInReserve;
//        isReloading = false;


//        playerInput = PlayerManager.instance.GetPlayerInput();

//        playerInput.actions["Fire"].performed += OnFire;
//        playerInput.actions["Reload"].performed += OnReload;

//    }

//    private void OnEnable()
//    {
//        //UpdateWeaponData(weaponData);
//        UpdateScriptableObject(weaponData);
//    }

//    private void OnDisable()
//    {
//        playerInput.actions["Fire"].performed -= OnFire;
//        playerInput.actions["Reload"].performed -= OnReload;
//    }

//    private void Update()
//    {
//        // if player is holding down "fire" button, then attempt to shoot
//        if (shootInput > 0 && CheckIfCanShoot())
//            Fire();
//        else
//            actuallyShooting = false;
//    }

//    public abstract void Fire();

//    // When upgrading a weapon, pass in a new GunData scriptable Object
//    // Ex. if a pistol goes from Level 1, to Level 2, you can call this function and pass in "Pistol Level 2" gun data
//    // and the gun will start to use Level 2 values.
//    //protected virtual void UpdateWeaponData(GunData scriptableObject)
//    //{
//    //    weaponData = (T) scriptableObject;

//    //    bulletsLoaded = weaponData.magazineSize;
//    //    //ammoInReserve = weaponData.maxAmmoInReserve;

//    //}

//    public virtual void SpawnBullet()
//    {
//        GameObject bullet;

//        // fetch a bullet from object pooler
//        bullet = ObjectPooler.instance.GetPooledObject(weaponData.bullet.PoolKey);

//        if(bullet != null)
//        {
//            // set the position to be at the barrel of the gun
//            bullet.transform.position = bulletSpawnPoint.position;
//            bullet.transform.rotation = this.transform.rotation;

//            // give the instaniated bullet the current scriptable object of this weapon
//            bullet.GetComponent<Bullet>().UpdateWeaponData(weaponData);

//            bullet.SetActive(true);

//            bulletsShot++;
//            bulletsLoaded--;
//        }
//        else
//        {
//            Debug.Log("Could not retrieve a bullet from object pool!");
//        }
        
//    }

//    public void OnFire(CallbackContext context)
//    {
//        shootInput = context.ReadValue<float>();

//    }
//    private void OnDrawGizmos()
//    {
//        // draws a green ray so its easier to aim (debugging purposes)
//        Gizmos.color = Color.green;
//        Vector3 direction = transform.TransformDirection(Vector2.right) * 6;
//        Gizmos.DrawRay(bulletSpawnPoint.position, direction);
//    }
//    #region Reloading

//    public virtual bool CheckIfCanShoot()
//    {
//        // if player has no ammo in reserve... force them to reload, then return false
//        if (bulletsLoaded <= 0f)
//        {
//            Debug.Log("RELOADING!");
//            Reload();
//            return false;
//        }
//        // otherwise if they are not reloading, allow them to shoot
//        else if (!isReloading)
//        {
//            actuallyShooting = true;
//            return true;
//        }

//        return false;
//    }

//    public void OnReload(CallbackContext context) {
//        Reload();
//    }
//    public void Reload()
//    {
//        // dont' reload if player doesn't have ammo in reserve
//        // or, if the player has a full magazine clip
//        //if (ammoInReserve == 0 || bulletsLoaded == weaponData.magazineSize)
//        //    return;

//        if (!isReloading)
//            StartCoroutine(WaitReload());
//    }

//    public virtual IEnumerator WaitReload()
//    {
//        actuallyShooting = false;
//        isReloading = true;

//        yield return new WaitForSeconds(weaponData.totalReloadTime);

//        RefillAmmo();

//        isReloading = false;
//    }

//    // Take away ammo from player reserves after they finished reloading
//    // This way of refilling ammo should work for all weapons except for pump shotguns
//    public virtual void RefillAmmo()
//    {
//        bulletsLoaded += bulletsShot;
//        bulletsShot = 0;

//        // has ammo in reserve after reloading (ex. 7/56 ammo to 8/55 ammo)
//        //if((ammoInReserve - bulletsShot) > 0)
//        //{
//        //    ammoInReserve -= bulletsShot;
//        //    bulletsLoaded += bulletsShot;
//        //    bulletsShot = 0;
//        //}
//        //// does not have ammo in reserve after reloading (ex. 6/2 to 8/0)
//        //else if((ammoInReserve - bulletsShot) <= 0)
//        //{
//        //    bulletsLoaded += ammoInReserve;
//        //    ammoInReserve = 0;
//        //    bulletsShot = 0;
//        //}
//    }

//    public void UpdateScriptableObject(GunData scriptableObject)
//    {
//        weaponData = scriptableObject as T;
//    }

//    // Gives player full ammo
//    //public void FullAmmo()
//    //{
//    //    ammoInReserve = weaponData.maxAmmoInReserve;
//    //    bulletsLoaded = weaponData.magazineSize;
//    //    bulletsShot = 0;
//    //}

//    #endregion
//}

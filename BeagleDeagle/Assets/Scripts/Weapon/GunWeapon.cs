using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public abstract class GunWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private PlayerInput playerInput;

    [Range(0, 1000)]
    public float damage;

    [HideInInspector]
    public int bulletsLoaded;

    public int magazineSize;

    public int maxAmmo;

    public float bulletTravelDistance; // how far until this bullet despawns

    public int penetrationCount; // how many enemies can this gun's bullet pass through?

    public float totalReloadTime; // how long will this gun take to reload to full?

    public Transform bulletSpawnPoint;

    public GameObject bulletPrefab;

    [HideInInspector]
    public bool isReloading;

    private void Start()
    {
        bulletsLoaded = magazineSize;
        isReloading = false;

        playerInput = player.GetComponent<PlayerInput>();
    }

    public virtual void Shoot()
    {
        if (bulletsLoaded <= 0f)
        {
            Debug.Log("RELOADING!");
            Reload();
        }   
        else
        {
            Debug.Log("SHOOT!");
            bulletsLoaded--;
        }
    }

    

    public abstract void Reload();
}

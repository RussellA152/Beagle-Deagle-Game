using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    //private GunWeapon gun; // what gun did this bullet come from?

    private GunData weaponData;

    [SerializeField]
    private int poolKey;

    [SerializeField]
    private float bulletSpeed = 15f;

    [SerializeField]
    private float destroyTime = 3f;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private LayerMask whatDestroysBullet;

    private int amountPenetrated; // how many enemies has this bullet penetrated through?

    // return the pool key (anything that is IPoolable, must have a pool key)
    public int PoolKey => poolKey;

    private void OnEnable()
    {
        amountPenetrated = 0;

        StartCoroutine(DisableAfterTime());

        SetStraightVelocity();
    }

    private void OnDisable()
    {
        // stop all coroutines when this bullet has been disabled
        StopAllCoroutines();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((whatDestroysBullet.value &  (1 << collision.gameObject.layer)) > 0){
            Debug.Log("HIT " + collision.gameObject.name);

            // Make target take damage
            collision.gameObject.GetComponent<Health>().ModifyHealth(-1 * weaponData.damagePerHit);

            Penetrate();
        }
    }
    private void Penetrate()
    {
        amountPenetrated++;

        // if this bullet has penetrated through too many enemies, then destroy it
        if (amountPenetrated >= weaponData.penetrationCount)
        {
            gameObject.SetActive(false);
        }

    }
    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * bulletSpeed;
    }

    // When a weapon has been upgraded, the gun will tell this bullet to change its scriptable Object to the same one that the gun has.
    // Ex. When a pistol goes from level 1 to level 2, this bullet will use "Pistol Level 2" data
    public void UpdateWeaponData(GunData scriptableObject)
    {
        weaponData = scriptableObject;

    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        gameObject.SetActive(false);
    }

}

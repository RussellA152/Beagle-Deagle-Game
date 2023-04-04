using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GunWeapon gun; // what gun did this bullet come from?

    [SerializeField]
    private float bulletSpeed = 15f;

    [SerializeField]
    private float destroyTime = 3f;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private LayerMask whatDestroysBullet;

    private int amountPenetrated; // how many enemies has this bullet penetrated through?


    private void Start()
    {
        amountPenetrated = 0;
        SetDestroyTime();
        SetStraightVelocity();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((whatDestroysBullet.value &  (1 << collision.gameObject.layer)) > 0){
            Debug.Log("HIT " + collision.gameObject.name);

            // Make target take damage
            collision.gameObject.GetComponent<Health>().ModifyHealth(-1 * gun.damagePerHit);

            Penetrate();
        }
    }
    private void Penetrate()
    {
        amountPenetrated++;

        // if this bullet has penetrated through too many enemies, then destroy it
        if (amountPenetrated >= gun.penetrationCount)
            Destroy(this.gameObject);

    }
    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * bulletSpeed;
    }

    // TEMPORARY
    // WILL PROBABLY OBJECT POOL THIS AT SOME POINT
    private void SetDestroyTime()
    {
        Destroy(this.gameObject, destroyTime);
    }

    public void SetGun(GunWeapon newGun)
    {
        gun = newGun;
    }
}

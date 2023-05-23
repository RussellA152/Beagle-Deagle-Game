using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    //private GunWeapon gun; // what gun did this bullet come from?

    [SerializeField]
    private ProjectileData projectileData;

    private GunData weaponData;


    [SerializeField]
    private int poolKey;

    [SerializeField]
    private Rigidbody2D rb;

    private int penetration = 1; // local version of GunData's "penetrationCount"

    private float damage = 0f; // local version of GunData's "damagePerHit" variable (this gets passed into the Bullet so that it can apply to the enemies they hit)

    private int amountPenetrated; // how many enemies has this bullet penetrated through?

    // return the pool key (anything that is IPoolable, must have a pool key)
    public int PoolKey => poolKey;

    private Vector3 defaultRotation = new Vector3(0f, 0f, -90f);

    private void OnEnable()
    {
        amountPenetrated = 0;

        StartCoroutine(DisableAfterTime());

        projectileData.ApplyTrajectory(rb, transform);
    }

    private void OnDisable()
    {
        // Resetting rotation before applying spread
        transform.rotation = Quaternion.Euler(defaultRotation);

        // stop all coroutines when this bullet has been disabled
        StopAllCoroutines();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((projectileData.whatDestroysBullet.value & (1 << collision.gameObject.layer)) > 0)
        {
            Debug.Log("BULLET HIT " + collision.gameObject.name);

            // Make target take damage
            collision.gameObject.GetComponent<IHealth>().ModifyHealth(-1 * damage);


            Penetrate();
        }
    }
    private void Penetrate()
    {
        amountPenetrated++;

        // if this bullet has penetrated through too many enemies, then destroy it
        if (amountPenetrated >= penetration)
        {
            gameObject.SetActive(false);
        }

    }

    // When a weapon has been upgraded, the gun will tell this bullet to change its scriptable Object to the same one that the gun has.
    // Ex. When a pistol goes from level 1 to level 2, this bullet will use "Pistol Level 2" data
    public void UpdateWeaponData(GunData scriptableObject)
    {
        if (weaponData == null)
        {
            weaponData = scriptableObject;
            penetration = scriptableObject.penetrationCount;
            damage = scriptableObject.damagePerHit;
            return;
        }

        // initialize difference variables
        int penetrationDifference = 0;
        float damageDifference = 0f;

        // If any item (such an item purchased from a trader) affects the local damage or penetration values, we make sure we account for it BEFORE we swap out for some new gun data
        // We account for item boosts by checking if the current penetration minus the gun data's penetration
        penetrationDifference = penetration - weaponData.penetrationCount;

        damageDifference = damage - weaponData.damagePerHit;

        // If penetration is greater than 0 or less than , we know the player received an upgrade or downgrade from a separate item (Ex. a purchased item boost from a trader)
        // This also works if the penetration did not receive an upgrade when receiving new weapon data
        if (penetrationDifference > 0 || penetrationDifference < 0)
        {
            penetration = scriptableObject.penetrationCount + penetrationDifference;
        }
        // otherwise, the player didn't receive an item boost upgrade, so just set penetration to new data's penetration
        else
        {
            penetration = scriptableObject.penetrationCount;
        }

        // If penetration is greater than 0, we know the player received an upgrade from a separate item (Ex. a purchased item boost from a trader)
        if (penetrationDifference > 0)
        {
            penetration = scriptableObject.penetrationCount + penetrationDifference;
        }
        // otherwise, the player didn't receive an item boost upgrade, so just set penetration to new data's penetration
        else
        {
            penetration = scriptableObject.penetrationCount;
        }


        // update to new GunData
        weaponData = scriptableObject;
        
    }

    public void UpdateProjectileData(ProjectileData scriptableObject)
    {

        projectileData = scriptableObject;

    }

    public void CalculateUpgrade()
    {
        int penetrationDifference = 0;
        float damageDifference = 0f;

    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(projectileData.destroyTime);
        gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    //private GunWeapon gun; // what gun did this bullet come from?
    private IPlayerStatModifier playerStatModifierScript;

    [SerializeField]
    private ProjectileData projectileData;

    private GunData weaponData;

    [SerializeField]
    private int poolKey;

    [SerializeField]
    private Rigidbody2D rb;

    private Vector3 defaultRotation = new Vector3(0f, 0f, -90f);

    private int amountPenetrated; // how many enemies has this bullet penetrated through?

    // return the pool key (anything that is IPoolable, must have a pool key)
    public int PoolKey => poolKey;

    private void OnEnable()
    {
        
        amountPenetrated = 0;

        StartCoroutine(DisableAfterTime());

        projectileData.ApplyTrajectory(rb, transform);

    }

    private void OnDisable()
    {
        // Resetting rotation before applying spread
        transform.position = Vector2.zero;
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
            collision.gameObject.GetComponent<IHealth>().ModifyHealth(-1 * weaponData.damagePerHit * playerStatModifierScript.GetDamageModifier());


            Penetrate();
        }
    }
    private void Penetrate()
    {
        amountPenetrated++;

        // if this bullet has penetrated through too many enemies, then destroy it
        if (amountPenetrated >= weaponData.penetrationCount * playerStatModifierScript.GetPenetrationCountModifier())
        {
            gameObject.SetActive(false);
        }

    }

    // When a weapon has been upgraded, the gun will tell this bullet to change its scriptable Object to the same one that the gun has.
    // Ex. When a pistol goes from level 1 to level 2, this bullet will use "Pistol Level 2" data
    public void UpdateWeaponData(GunData scriptableObject)
    {
        // update to new GunData
        weaponData = scriptableObject;     
    }

    public void UpdateProjectileData(ProjectileData scriptableObject)
    {
        projectileData = scriptableObject;
    }

    public void UpdatePlayerStatsModifierScript(IPlayerStatModifier modifierScript)
    {
        playerStatModifierScript = modifierScript;
    }


    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(projectileData.destroyTime);
        gameObject.SetActive(false);
    }

}

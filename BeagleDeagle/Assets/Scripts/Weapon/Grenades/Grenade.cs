using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Explosive<SmokeGrenadeData>, IPoolable
{
    [SerializeField]
    private int poolKey;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Collider2D grenadeCollider;
    

    [Range(0f, 100f)]
    [SerializeField]
    private float throwSpeed = 15f; // How fast this grenade throws towards enemies (inside of monobehaviour for now)

    public int PoolKey => poolKey; // Return the pool key (anything that is IPoolable, must have a pool key)

    private void OnDisable()
    {
        areaOfEffectGameObject.gameObject.SetActive(false);
        
        //particleAOE.SetActive(false);
        sprite.SetActive(true);

        grenadeCollider.enabled = true;

        UnfreezePosition();

        StopAllCoroutines();
    }

    public void ActivateGrenade(Vector2 aimDirection)
    {
        StartCoroutine(Detonate());

        // Rotate the grenade based on the aim direction
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        // Apply force to push the grenade forward in the aim direction
        rb.AddForce(aimDirection * throwSpeed, ForceMode2D.Impulse);
    }

    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public override IEnumerator Detonate()
    {
        yield return new WaitForSeconds(explosiveData.detonationTime);

        sprite.SetActive(false);

        //particleAOE.SetActive(true);
        areaOfEffectGameObject.gameObject.SetActive(true);

        FreezePosition();

        Explode();

        grenadeCollider.enabled = false;

        yield return new WaitForSeconds(explosiveData.GetDuration());

        gameObject.SetActive(false);
        
    }

    private void FreezePosition()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

    }

    private void UnfreezePosition()
    {
        rb.constraints = RigidbodyConstraints2D.None;
    }

}

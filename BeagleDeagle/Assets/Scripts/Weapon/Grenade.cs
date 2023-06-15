using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Explosive<GrenadeData>, IPoolable
{
    [SerializeField]
    private int poolKey;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Collider2D grenadeCollider;

    public int PoolKey => poolKey; // Return the pool key (anything that is IPoolable, must have a pool key)
    private void OnEnable()
    {
        areaOfEffect.UpdateAOEData(explosiveData.aoeData);
    }

    private void OnDisable()
    {
        areaOfEffect.gameObject.SetActive(false);
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
        rb.AddForce(aimDirection * explosiveData.throwSpeed, ForceMode2D.Impulse);
    }

    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public override IEnumerator Detonate()
    {
        yield return new WaitForSeconds(explosiveData.detonationTime);

        sprite.SetActive(false);
        areaOfEffect.gameObject.SetActive(true);

        FreezePosition();

        explosiveData.Explode(transform.position);

        grenadeCollider.enabled = false;

        yield return new WaitForSeconds(explosiveData.GetDuration());

        gameObject.SetActive(false);
        
    }

    public void FreezePosition()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

    }

    public void UnfreezePosition()
    {
        rb.constraints = RigidbodyConstraints2D.None;
    }
    public override void UpdateExplosiveData(GrenadeData scriptableObject)
    {
        base.UpdateExplosiveData(scriptableObject);
    }
}

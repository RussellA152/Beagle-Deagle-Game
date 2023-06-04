using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IPoolable
{
    private GameObject player;

    [SerializeField]
    private int poolKey;

    [SerializeField]
    private GrenadeData grenadeData;

    [SerializeField]
    private GameObject grenadeSprite;

    [SerializeField]
    private GrenadeAreaOfEffect areaOfEffect;

    [SerializeField]
    private Collider2D grenadeCollider;

    [SerializeField]
    private Rigidbody2D rb;

    public int PoolKey => poolKey; // Return the pool key (anything that is IPoolable, must have a pool key)
    private void OnEnable()
    {
        areaOfEffect.UpdateThrowableData(grenadeData);
    }

    private void OnDisable()
    {
        areaOfEffect.gameObject.SetActive(false);
        grenadeSprite.SetActive(true);

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
        rb.AddForce(aimDirection * grenadeData.throwSpeed, ForceMode2D.Impulse);
    }

    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public IEnumerator Detonate()
    {
        yield return new WaitForSeconds(grenadeData.detonationTime);

        grenadeSprite.SetActive(false);
        areaOfEffect.gameObject.SetActive(true);

        FreezePosition();

        grenadeData.Explode();

        grenadeCollider.enabled = false;

        yield return new WaitForSeconds(grenadeData.duration);

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
    public void UpdateThrowableData(GrenadeData scriptableObject)
    {
        grenadeData = scriptableObject;
    }
}

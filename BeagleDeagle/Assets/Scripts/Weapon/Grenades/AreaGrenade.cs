using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaGrenade : Explosive<UtilityExplosiveData>, IPoolable
{
    [SerializeField]
    private int poolKey;
    
    private Rigidbody2D _rb;
    
    private Collider2D _grenadeCollider;
    

    [Range(0f, 100f)]
    [SerializeField]
    private float throwSpeed = 15f; // How fast this grenade throws towards enemies (inside of monobehaviour for now)

    public int PoolKey => poolKey; // Return the pool key (anything that is IPoolable, must have a pool key)


    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _grenadeCollider = GetComponent<Collider2D>();
    }

    private void OnDisable()
    {
        AreaOfEffectScript.gameObject.SetActive(false);
        
        //particleAOE.SetActive(false);
        sprite.SetActive(true);

        _grenadeCollider.enabled = true;

        UnfreezePosition();

        StopAllCoroutines();
    }

    public override void Activate(Vector2 aimDirection)
    {
        base.Activate(aimDirection);
        
        StartCoroutine(Detonate());

        // Rotate the grenade based on the aim direction
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        // Apply force to push the grenade forward in the aim direction
        _rb.AddForce(aimDirection * throwSpeed, ForceMode2D.Impulse);
    }

    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public override IEnumerator Detonate()
    {
        yield return new WaitForSeconds(ExplosiveData.detonationTime);

        sprite.SetActive(false);

        //particleAOE.SetActive(true);
        AreaOfEffectScript.gameObject.SetActive(true);

        FreezePosition();

        Explode();

        _grenadeCollider.enabled = false;

        yield return new WaitForSeconds(Duration);

        gameObject.SetActive(false);
        
    }

    private void FreezePosition()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

    }

    private void UnfreezePosition()
    {
        _rb.constraints = RigidbodyConstraints2D.None;
    }

}

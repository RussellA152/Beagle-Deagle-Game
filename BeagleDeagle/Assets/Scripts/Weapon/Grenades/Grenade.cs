using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Explosive, IPoolable
{
    private Rigidbody2D _rb;
    
    private Collider2D _grenadeCollider;
    
    private GrenadeAnimation _grenadeAnimation;
    

    [Range(0f, 100f)]
    [SerializeField]
    private float throwSpeed = 15f; // How fast this grenade throws towards enemies (inside of monobehaviour for now)

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _grenadeCollider = GetComponent<Collider2D>();

        _grenadeAnimation = GetComponentInChildren<GrenadeAnimation>();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        _grenadeCollider.enabled = true;

        UnfreezePosition();
    }

    public override void Activate(Vector2 aimDirection)
    {
        base.Activate(aimDirection);
        
        StartCoroutine(Detonate());
        
        _grenadeAnimation.ThrowAnimation(aimDirection);

        // Apply force to push the grenade forward in the aim direction
        _rb.AddForce(aimDirection * throwSpeed, ForceMode2D.Impulse);
        
    }

    protected override void AfterDetonation()
    {
        base.AfterDetonation();
        
        _grenadeAnimation.BounceOnLandAnimation();
        
        FreezePosition();
        
        _grenadeCollider.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _grenadeAnimation.StopThrowAnimation();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 15f;

    [SerializeField]
    private float destroyTime = 3f;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private LayerMask whatDestroysBullet;

    private void Start()
    {  
        SetDestroyTime();
        SetStraightVelocity();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((whatDestroysBullet.value &  (1 << collision.gameObject.layer)) > 0){
            Debug.Log("HIT " + collision.gameObject.name);
        }
    }
    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * bulletSpeed;
    }
    private void SetDestroyTime()
    {
        Destroy(this.gameObject, destroyTime);
    }
}

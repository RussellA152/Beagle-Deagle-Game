using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGrenade", menuName = "ScriptableObjects/Grenade")]
public abstract class GrenadeData : ScriptableObject
{
    [SerializeField]
    private PhysicsMaterial2D physicsMaterial; // What physics material should this grenade use? (Ex. Bouncy material)

    public AreaOfEffectData aoeData;

    public bool hitThroughWalls;

    //[Header("Speed of Grenade")]
    //[Range(0f, 100f)]
    //public float throwSpeed = 15f;

    [Header("Grenade Timers")]
    [Range(0f, 30f)]
    public float detonationTime; // how long until this grenade detonates?

    public virtual void Explode(Vector2 explosionSource)
    {
        // Play explosion sound

        // Screen shake
        Debug.Log("EXPLODE! " + name);
    }

    // Check if there is a wall between the explosion source and the potential target of that explosion
    public bool CheckObstruction(Vector2 explosionSource, Collider2D targetCollider)
    {
        if (hitThroughWalls)
        {
            return false;
        }
        
        Vector2 targetPosition = targetCollider.transform.position;
        Vector2 direction = targetPosition - explosionSource;
        float distance = direction.magnitude;

        // Exclude the target if there is an obstruction between the explosion source and the target
        RaycastHit2D hit = Physics2D.Raycast(explosionSource, direction.normalized, distance, 1 << LayerMask.NameToLayer("Wall"));

        if (hit.collider != null)
        {
            Debug.Log("OBSTRUCTION FOUND!");
            // There is an obstruction, so skip this target
            return true;
        }
        else
        {
            // If no obstruction found, allow this target to be affected by the explosion
            return false;
        }
    }

    public abstract float GetDuration();

}

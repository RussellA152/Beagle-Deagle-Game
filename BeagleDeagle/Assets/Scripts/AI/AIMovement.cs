using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour, IMovable, IStunnable, IKnockBackable
{
    
    [Header("Data to Use")]
    [SerializeField]
    private EnemyData enemyScriptableObject; // Data for enemy's movement (contains movement speed)
    
    [Header("Required Components")]
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField] 
    private SpriteRenderer spriteRenderer;

    [Header("Required Scripts")] 
    [SerializeField]
    private ZombieAnimationHandler animationScript;
    
    [Header("Modifiers")]
    [SerializeField, NonReorderable]
    private List<MovementSpeedModifier> movementSpeedModifiers = new List<MovementSpeedModifier>(); // a list of modifiers being applied to this enemy's movement speed 

    public bool isStunned { get; private set; } // Is this enemy stunned?

    private float bonusSpeed = 1f; // Speed multiplier on the movement speed of this enemy

    private void OnEnable()
    {
        agent.speed = enemyScriptableObject.movementSpeed * bonusSpeed;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        FlipSprite();
    }

    ///-///////////////////////////////////////////////////////////
    /// Disable the enemy's ability to move.
    /// Then start a coroutine to wait some time and remove the stun effect
    /// 
    public void GetStunned(float duration)
    {       
        if(!isStunned)
            StartCoroutine(RemoveStunCoroutine(duration));
    }

    ///-///////////////////////////////////////////////////////////
    /// Apply a specified amount of force in a direction to the enemy.
    /// 
    public void ApplyKnockBack(Vector2 force, Vector2 direction)
    {
        // Prevent enemy from moving
        // Also, this enemy will not be able to flip their sprite because they are stopped
        agent.isStopped = true;

        rb.AddForce(direction * force, ForceMode2D.Impulse);

        StartCoroutine(WaitForKnockBackToFinish());
    }


    ///-///////////////////////////////////////////////////////////
    /// Wait until the enemy's velocity reaches near zero to allow them to move again
    /// 
    public IEnumerator WaitForKnockBackToFinish()
    {
        // Wait for a short duration before checking
        yield return new WaitForSeconds(0.1f);

        // Continue waiting until velocity becomes small
        while (rb.velocity.magnitude > 0.1f) 
        {
            yield return null;
        }

        // Allow enemy to move again
        agent.isStopped = false;
    }

    ///-///////////////////////////////////////////////////////////
    /// When this enemy's target is to their left, flip their sprite to the left.
    /// Otherwise, keep their sprite facing right.
    /// 
    private void FlipSprite()
    {
        spriteRenderer.flipX = agent.destination.x < transform.position.x;
    }

    ///-///////////////////////////////////////////////////////////
    /// Wait some time, then remove stun from enemy
    /// 
    public IEnumerator RemoveStunCoroutine(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    public void AddMovementSpeedModifier(MovementSpeedModifier modifierToAdd)
    {
        movementSpeedModifiers.Add(modifierToAdd);
        bonusSpeed += bonusSpeed * modifierToAdd.bonusMovementSpeed;
        agent.speed = enemyScriptableObject.movementSpeed * bonusSpeed;
        
        // Increase or decrease the animation speed of the movement animation
        animationScript.SetMovementAnimationSpeed(modifierToAdd.bonusMovementSpeed);
        
    }

    public void RemoveMovementSpeedModifier(MovementSpeedModifier modifierToRemove)
    {
        movementSpeedModifiers.Remove(modifierToRemove);
        bonusSpeed /= (1 + modifierToRemove.bonusMovementSpeed);
        agent.speed = enemyScriptableObject.movementSpeed * bonusSpeed;
        
        // Remove speed effect that was applied to the movement animation's speed
        animationScript.SetMovementAnimationSpeed(-1f * modifierToRemove.bonusMovementSpeed);
    }

    public void RevertAllModifiers()
    {
        // Reset any movement speed modifiers on the enemy
        bonusSpeed = 1f;

        // Remove speed modifiers from list when spawning
        movementSpeedModifiers.Clear();
    }
}

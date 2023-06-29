using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour, IMovable, IStunnable, IKnockBackable
{
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private EnemyData enemyScriptableObject; // Data for enemy's movement (contains movement speed)

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
    }

    public void RemoveMovementSpeedModifier(MovementSpeedModifier modifierToRemove)
    {
        movementSpeedModifiers.Remove(modifierToRemove);
        bonusSpeed /= (1 + modifierToRemove.bonusMovementSpeed);

        agent.speed = enemyScriptableObject.movementSpeed * bonusSpeed;
    }

    public void RevertAllModifiers()
    {
        // Reset any movement speed modifiers on the enemy
        bonusSpeed = 1f;

        // Remove speed modifiers from list when spawning
        movementSpeedModifiers.Clear();
    }
}

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

    // Disable the enemy's ability to move
    public void GetStunned(float duration)
    {       
        if(!isStunned)
            StartCoroutine(RemoveStunCoroutine(duration));
    }

    public void ApplyKnockback(Vector2 force, Vector2 direction)
    {
        agent.isStopped = true;

        rb.AddForce(direction * force, ForceMode2D.Impulse);

        StartCoroutine(DampKnockBackForce());
    }

    // Slow down knockback effect on this enemy
    // Without this, enemies will be knocked back forever and won't slow down
    public IEnumerator DampKnockBackForce()
    {
        yield return new WaitForSeconds(0.1f); // Wait for a short duration before damping the force

        while (rb.velocity.magnitude > 0.1f) // Continue waiting until velocity becomes small
        {
            yield return null;
        }

        agent.isStopped = false;
    }

    // Wait some time, then remove stun from enemy
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

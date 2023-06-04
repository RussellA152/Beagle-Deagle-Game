using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour, IMovable, IStunnable
{
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent agent;

    [SerializeField]
    private EnemyData enemyScriptableObject;

    [SerializeField, NonReorderable]
    private List<MovementSpeedModifier> movementSpeedModifiers = new List<MovementSpeedModifier>(); // a list of modifiers being applied to this enemy's movement speed 

    public bool isStunned { get; private set; } // Is this enemy stunned?

    private float bonusSpeed = 1f;

    private void OnEnable()
    {
        agent.speed = enemyScriptableObject.movementSpeed * bonusSpeed;
    }

    // Disable the enemy's ability to move
    public void GetStunned(float duration)
    {       
        if(!isStunned)
            StartCoroutine(RemoveStunCoroutine(duration));
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

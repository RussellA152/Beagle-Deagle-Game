using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObstruction : MonoBehaviour
{
    ///-///////////////////////////////////////////////////////////
    /// Shoot a raycast beginning from the center of the damaging object towards the target
    /// If there is wall between the object and the target, then do not apply any effects to the target (Don't hit through walls)
    ///
    public bool HasObstruction(Vector2 sourcePosition, GameObject target, LayerMask layerMask)
    {
        
        // Calculate distance and direction to shoot raycast
        Vector2 targetPosition = target.transform.position;
        
        Vector2 direction = targetPosition - sourcePosition;
        
        float distance = direction.magnitude;

        // Exclude the target if there is an obstruction between the explosion source and the target
        RaycastHit2D hit = Physics2D.Raycast(sourcePosition, direction.normalized, distance, layerMask);

        // If true, there is an obstruction between the AOE and target
        // If false, there is not an obstruction between AOE and target, thus we can apply the effect
        return hit.collider != null;
    }
}

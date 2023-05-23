using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewThrowable", menuName = "ScriptableObjects/Throwable/SmokeBomb")]
public class SmokeBomb : ThrowableData
{
    [Range(0f,-50f)]
    [SerializeField]
    private float slowAmount; // how much to slow an enemy by?

    [SerializeField]
    private float radius; // how big should this smoke bomb be?

    [SerializeField]
    private LayerMask layersToHit;

    private void OnEnable()
    {
        
    }

    public override void SpecialAbility(Vector2 position)
    {
        Collider2D collider = Physics2D.OverlapCircle(position, radius, layersToHit);

        IMovable movableObject = collider.GetComponent<IMovable>();

        if(movableObject != null)
        {
            Debug.Log("SLOW DOWN Character!");
            movableObject.ModifyMovementSpeed(-slowAmount);
        }
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius, layersToHit);

        //foreach(Collider2D collider in colliders)
        //{
        //    IMovable movableObject = collider.GetComponent<IMovable>();

        //    if(movableObject != null)
        //    {
        //        Debug.Log("SLOW DOWN Character!");
        //        movableObject.ModifyMovementSpeed(slowAmount);
        //    }
        //}
    }

    public override void OnTriggerExit(Collider2D collider)
    {
        IMovable movableObject = collider.GetComponent<IMovable>();

        if (movableObject != null)
        {
            Debug.Log("Revert Speed to character!");
            movableObject.ModifyMovementSpeed(slowAmount);
        }
    }
}

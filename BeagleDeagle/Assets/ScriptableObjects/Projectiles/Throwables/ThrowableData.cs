using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewThrowable", menuName = "ScriptableObjects/Throwable")]
public abstract class ThrowableData : ScriptableObject
{
    public LayerMask whatActivatesThrowable; // if this throwable hits something, it should activate (ex. hitting a wall or enemy)

    public float projectileSpeed = 15f;

    [Range(0f, 30f)]
    public float destroyTime = 3f;


    //public abstract void SpecialAbility(Vector2 position);

    //public abstract void OnTriggerExit(Collider2D collider);
}

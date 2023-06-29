using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockBackable
{
    // Apply a force/knockback to an entity
    // An example would be a grenade that applies knockback to enemies and other objects caught in the radius
    public void ApplyKnockBack(Vector2 force, Vector2 direction);

}

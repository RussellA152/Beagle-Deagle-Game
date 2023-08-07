using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockBackable
{
    ///-///////////////////////////////////////////////////////////
    /// Apply a force/knockBack to an entity. For example, a grenade 
    /// might apply knockBack to enemies and other objects caught in the radius
    /// 
    public void ApplyKnockBack(Vector2 force, Vector2 direction);

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStunnable
{
    // Tell entity to get stunned (use coroutine to start timer)
    public void GetStunned(float duration);

}

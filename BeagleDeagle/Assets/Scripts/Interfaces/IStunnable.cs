using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStunnable
{
    ///-///////////////////////////////////////////////////////////
    /// Tell entity to get stunned. They will use a coroutine to start a timer until they are unstunned
    /// 
    public void GetStunned(float duration);

}

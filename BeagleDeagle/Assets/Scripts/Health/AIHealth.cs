using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : Health
{
    public override void OnDeath()
    {
        base.OnDeath();
        // return to object pool?
    }
}

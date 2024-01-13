using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    ///-///////////////////////////////////////////////////////////
    /// An integer value needed to retrieve a specific item (ex. bullet) from
    /// the object pooler
    /// 
    int PoolKey { get; }
    
}

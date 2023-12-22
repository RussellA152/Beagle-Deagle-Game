using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// This script will stretch the sprite of a sphere to display how big the range of something is. For example, a Map Objective's exit range should be displayed to the player
/// so they know their boundaries before the objective will fail.
/// 
public class RangeIndicator : MonoBehaviour
{
    // Divide by 5 to account for converting a radius to transform.local scale
    private int _divider = 5;
    public void SetRadius(float radius)
    {
        transform.localScale = new Vector2(radius / _divider, radius / _divider);
    }
}


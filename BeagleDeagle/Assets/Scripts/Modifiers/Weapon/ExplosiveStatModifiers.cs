using UnityEngine;

[System.Serializable]
public class ExplosiveRadiusModifier: Modifier
{
    [Range(-1f, 1f)]
    public float bonusRadius;

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to an player's explosive radius values.
    /// 
    public ExplosiveRadiusModifier(string name, float radius)
    {
        modifierName = name;
        bonusRadius = radius;
    }

}



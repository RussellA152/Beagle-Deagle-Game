using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGrenade", menuName = "ScriptableObjects/Grenade/Smoke Grenade")]
public class SmokeGrenade : GrenadeData
{
    [SerializeField]
    private SmokeBombUtility utilityAbilityData;

    public override void Explode(Vector2 position)
    {
        Debug.Log("Explode smoke grenade!");
    }


    public override float GetDuration()
    {
        return utilityAbilityData.duration;
    }
}

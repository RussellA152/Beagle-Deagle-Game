using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/Ability/Utility/SmokeBomb")]
public class SmokeBombUtility : UtilityAbilityData
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private SmokeGrenade smokeGrenadeData;

    public override void ActivateUtility(GameObject player)
    {
        Debug.Log("Throw smoke grenade!");
        Instantiate(prefab, player.transform.position, Quaternion.identity);
        //prefab.GetComponent<Throwable>().UpdateThrowableData(smokeBombData);
    }
}

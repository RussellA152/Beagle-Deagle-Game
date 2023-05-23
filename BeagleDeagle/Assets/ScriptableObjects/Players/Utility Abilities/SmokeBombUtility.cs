using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/Ability/Utility/SmokeBomb")]
public class SmokeBombUtility : UtilityAbilityData
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private SmokeBomb smokeBombData;

    [SerializeField]
    private Vector3 offset;

    public override void ActivateUtility(GameObject player)
    {
        Instantiate(prefab, player.transform.position + offset, Quaternion.identity);
        prefab.GetComponent<Throwable>().UpdateThrowableData(smokeBombData);
    }
}

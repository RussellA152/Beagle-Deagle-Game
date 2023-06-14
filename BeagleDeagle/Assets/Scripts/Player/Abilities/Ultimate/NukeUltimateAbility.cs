using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNukeUltimate", menuName = "ScriptableObjects/Ability/Ultimate/Nuclear Bomb")]
public class NukeUltimateAbility : UltimateAbilityData
{
    [Header("Prefab to Spawn")]
    [SerializeField]
    private GameObject prefab;

    [Header("Grenade Data")]
    [SerializeField]
    private NuclearBomb nuclearBombData;

    public override void ActivateUltimate(GameObject player)
    {
        Debug.Log("Spawn nuclear bomb!");

        GameObject nuclearBomb = Instantiate(prefab);

        nuclearBomb.SetActive(false);

        nuclearBomb.transform.position = player.transform.position;

        nuclearBomb.GetComponent<Grenade>().UpdateThrowableData(nuclearBombData);

        nuclearBomb.SetActive(true);

        nuclearBomb.GetComponent<Grenade>().ActivateGrenade(player.transform.position);
        //nuclearBomb
    }

    public override IEnumerator ActivationCooldown()
    {
        yield return new WaitForSeconds(cooldown);
    }
}

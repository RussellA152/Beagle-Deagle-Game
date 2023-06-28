using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaOfEffectManager : MonoBehaviour
{
    [SerializeField]
    private List<AreaOfEffectData> activeAreaOfEffects;
    
    private Dictionary<AreaOfEffectData, Dictionary<GameObject, int>> areaOfEffectOverlappingTargets = new Dictionary<AreaOfEffectData, Dictionary<GameObject, int>>();

    
    // Key: The target inside of the AOE's trigger collider
    // Value: The number of AOE trigger colliders that the target is inside of
    //private Dictionary<GameObject, int> overLappingTargets = new Dictionary<GameObject, int>();

    // A hashset of all enemies affected by the area of effect's ability (ex. smoke slow effect or radiation damage)
    private HashSet<GameObject> affectedEnemies = new HashSet<GameObject>();
    
    private void OnDisable()
    {
        //overLappingTargets.Clear();
        areaOfEffectOverlappingTargets.Clear();
        affectedEnemies.Clear();
    }

    public void AddNewAreaOfEffect(AreaOfEffectData newAreaOfEffect)
    {
        areaOfEffectOverlappingTargets.TryAdd(newAreaOfEffect, new Dictionary<GameObject, int>());
    }

    public void RemoveAreaOfEffect(AreaOfEffectData areaOfEffect)
    {
        areaOfEffectOverlappingTargets.Remove(areaOfEffect);
    }

    public void AddNewOverlappingTarget(AreaOfEffectData areaOfEffect, GameObject target)
    {
        Dictionary<GameObject, int> nestedDictionary = areaOfEffectOverlappingTargets[areaOfEffect];

        nestedDictionary[target]++;
    }

    public void RemoveOverlappingTarget(AreaOfEffectData areaOfEffect, GameObject target)
    {
        Dictionary<GameObject, int> nestedDictionary = areaOfEffectOverlappingTargets[areaOfEffect];

        nestedDictionary[target]--;
    }
    
}

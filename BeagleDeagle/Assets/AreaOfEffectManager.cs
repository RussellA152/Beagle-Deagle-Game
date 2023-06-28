using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectManager : MonoBehaviour
{
    public static AreaOfEffectManager Instance;
    
    [SerializeField]
    private List<AreaOfEffectData> activeAreaOfEffects;
    
    // Key: The area of effect that will be applied to target (ex. Slowing Smoke)
    // Value: Another dictionary whose key is the target that is being affected by the area of effect,
    // and the value that indicates the number of same area of effects that the target is standing in
    
    private readonly Dictionary<AreaOfEffectData, Dictionary<GameObject, int>> _areaOfEffectOverlappingTargets = new Dictionary<AreaOfEffectData, Dictionary<GameObject, int>>();

    // Key: The area of effect that will be applied to target (ex. Slowing Smoke)
    // Value: A hashset containing all targets that are inside of the area of effect
    private readonly Dictionary<AreaOfEffectData, HashSet<GameObject>> _affectedTargets =
        new Dictionary<AreaOfEffectData, HashSet<GameObject>>();
    

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDisable()
    {
        activeAreaOfEffects.Clear();
        
        _areaOfEffectOverlappingTargets.Clear();
        
        _affectedTargets.Clear();
    }

    public void AddNewAreaOfEffect(AreaOfEffectData newAreaOfEffect)
    {
        if(!activeAreaOfEffects.Contains(newAreaOfEffect))
            activeAreaOfEffects.Add(newAreaOfEffect);
        
        _areaOfEffectOverlappingTargets.TryAdd(newAreaOfEffect, new Dictionary<GameObject, int>());
        
        _affectedTargets.TryAdd(newAreaOfEffect, new HashSet<GameObject>());
    }

    ///-///////////////////////////////////////////////////////////
    /// When the target enters an AOE, add them to the areaOfEffectOverlappingTargets dictionary
    /// 
    public void AddNewOverlappingTarget(AreaOfEffectData areaOfEffect, GameObject target)
    {
        Dictionary<GameObject, int> nestedOverlappingTargetsDictionary = _areaOfEffectOverlappingTargets[areaOfEffect];
        
        nestedOverlappingTargetsDictionary.TryAdd(target, 0);
        
        nestedOverlappingTargetsDictionary[target]++;
    }

    ///-///////////////////////////////////////////////////////////
    /// When the target leaves the AOE, remove them from both dictionaries
    /// 
    public bool RemoveOverlappingTarget(AreaOfEffectData areaOfEffect, GameObject target)
    {
        bool shouldRemove = false;
        Dictionary<GameObject, int> nestedOverlappingTargetsDictionary = _areaOfEffectOverlappingTargets[areaOfEffect];
        HashSet<GameObject>affectedTargetsHashSet = _affectedTargets[areaOfEffect];

        nestedOverlappingTargetsDictionary[target]--;

        if (nestedOverlappingTargetsDictionary[target] == 0)
        {
            shouldRemove = true;
            nestedOverlappingTargetsDictionary.Remove(target);
            
        }

        return shouldRemove;
    }

    public bool RemoveFromAffectedHashSet(AreaOfEffectData areaOfEffect, GameObject target)
    {
        HashSet<GameObject>affectedTargetsHashSet = _affectedTargets[areaOfEffect];

        if (affectedTargetsHashSet.Contains(target))
        {
            affectedTargetsHashSet.Remove(target);
            
            return true;
        }

        return false;
    }

    ///-///////////////////////////////////////////////////////////
    /// Check if the target has not already been affected by the AOE's effect
    /// If not, then add them to the affectedTargets dictionary
    /// 
    public bool CheckIfTargetCanBeAffected(AreaOfEffectData areaOfEffect, GameObject target)
    {
        HashSet<GameObject>affectedTargetsHashSet = _affectedTargets[areaOfEffect];

        // If this is the first AOE that the target enters,
        // and the target is not affected by the AOE, then allow the AOE to apply its effect on the target
        if (CheckIfTargetIsOverlapping(areaOfEffect, target) && !affectedTargetsHashSet.Contains(target))
        {
            affectedTargetsHashSet.Add(target);
            
            return true;
        }
        
        else
        {
            return false;
        }
        
    }

    ///-///////////////////////////////////////////////////////////
    /// Check if the target is only touching 1 AOE (of the same type)
    /// 
    public bool CheckIfTargetIsOverlapping(AreaOfEffectData areaOfEffect, GameObject target)
    {
        Dictionary<GameObject, int> nestedOverlappingTargetsDictionary = _areaOfEffectOverlappingTargets[areaOfEffect];

        return nestedOverlappingTargetsDictionary[target] == 1;
    }
    
    
}

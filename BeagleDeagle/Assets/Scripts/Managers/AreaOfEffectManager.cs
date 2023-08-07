using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectManager : MonoBehaviour
{
    public static AreaOfEffectManager Instance;

    [SerializeField] private List<AreaOfEffectData> activeAreaOfEffects;

    // Key: The area of effect that will be applied to target (ex. Slowing Smoke)
    // Value: Another dictionary whose key is the target that is being affected by the area of effect,
    // and the value that indicates the number of same area of effects that the target is standing in
    private readonly Dictionary<AreaOfEffectData, Dictionary<GameObject, int>> _areaOfEffectOverlappingTargets =
        new Dictionary<AreaOfEffectData, Dictionary<GameObject, int>>();

    // Key: The area of effect that will be applied to target (ex. Slowing Smoke)
    // Value: A hashset containing all targets that are inside of the area of effect
    private readonly Dictionary<AreaOfEffectData, HashSet<GameObject>> _affectedTargets =
        new Dictionary<AreaOfEffectData, HashSet<GameObject>>();


    private void Awake()
    {
        // Create singleton instance
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void OnDisable()
    {
        // Clear all dictionaries 
        activeAreaOfEffects.Clear();

        _areaOfEffectOverlappingTargets.Clear();

        _affectedTargets.Clear();
    }

    ///-///////////////////////////////////////////////////////////
    /// When an AOE has spawned, register it to both dictionaries
    /// This is how we can distinctly check the AOE that is being applied to a target
    /// 
    public void AddNewAreaOfEffect(AreaOfEffectData newAreaOfEffect)
    {
        if (!activeAreaOfEffects.Contains(newAreaOfEffect))
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
    /// When the target leaves the AOE, remove them from overLapping dictionary
    /// 
    public void RemoveOverlappingTarget(AreaOfEffectData areaOfEffect, GameObject target)
    {
        Dictionary<GameObject, int> nestedOverlappingTargetsDictionary = _areaOfEffectOverlappingTargets[areaOfEffect];
        
        nestedOverlappingTargetsDictionary[target]--;
        
        bool shouldRemove = nestedOverlappingTargetsDictionary[target] == 0;

        if (shouldRemove)
        {
            nestedOverlappingTargetsDictionary.Remove(target);
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// When the target leaves the AOE, or their DOT has expired
    /// Remove them from the affectedTargets hashset
    public void RemoveTargetFromAffectedHashSet(AreaOfEffectData areaOfEffect, GameObject target)
    {
        HashSet<GameObject> affectedTargetsHashSet = _affectedTargets[areaOfEffect];
        
        affectedTargetsHashSet.Remove(target);

    }

    ///-///////////////////////////////////////////////////////////
    /// Return true if the target is inside of the affectedTarget's hashset
    /// 
    public bool CheckIfTargetIsAffected(AreaOfEffectData areaOfEffect, GameObject target)
    {
        HashSet<GameObject> affectedTargetsHashSet = _affectedTargets[areaOfEffect];

        return affectedTargetsHashSet.Contains(target);
    }

    public void TryAddAffectedTarget(AreaOfEffectData areaOfEffect, GameObject target)
    {
        HashSet<GameObject> affectedTargetsHashSet = _affectedTargets[areaOfEffect];
        
        bool canBeAffected = IsTargetOverlappingSingleAreaOfEffect(areaOfEffect, target) &&
                             !affectedTargetsHashSet.Contains(target);

        if (canBeAffected)
        {
            affectedTargetsHashSet.Add(target);
        }
    }


    ///-///////////////////////////////////////////////////////////
    /// Check if the target is only touching 1 AOE (of the same type)
    /// 
    private bool IsTargetOverlappingSingleAreaOfEffect(AreaOfEffectData areaOfEffect, GameObject target)
    {
        Dictionary<GameObject, int> nestedOverlappingTargetsDictionary = _areaOfEffectOverlappingTargets[areaOfEffect];

        return nestedOverlappingTargetsDictionary[target] == 1;
    }

    ///-///////////////////////////////////////////////////////////
    /// Return true if the target is overlapping with 1 or more AOE(s) (of the same type)
    /// 
    public bool IsTargetOverlappingAreaOfEffect(AreaOfEffectData areaOfEffect, GameObject target)
    {
        Dictionary<GameObject, int> nestedOverlappingTargetsDictionary = _areaOfEffectOverlappingTargets[areaOfEffect];

        // If the target is still standing in an AOE, return true
        // Otherwise, if the value is not greater than 0, return false because the target is not standing in an AOE
        if (nestedOverlappingTargetsDictionary.TryGetValue(target, out int value))
        {
            return value > 0;
        }

        return false;
    }
}

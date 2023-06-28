using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaOfEffectManager : MonoBehaviour
{
    public static AreaOfEffectManager Instance;
    
    [SerializeField]
    private List<AreaOfEffectData> activeAreaOfEffects;
    
    // Key: The area of effect that will be applied to target (ex. Slowing Smoke)
    // Value: Another dictionary whose key is the target that is being affected by the area of effect,
    // and the value that indicates the number of same area of effects that the target is standing in
    
    private Dictionary<AreaOfEffectData, Dictionary<GameObject, int>> _areaOfEffectOverlappingTargets = new Dictionary<AreaOfEffectData, Dictionary<GameObject, int>>();

    // Key: The area of effect that will be applied to target (ex. Slowing Smoke)
    // Value: A hashset containing all targets that are inside of the area of effect
    private Dictionary<AreaOfEffectData, HashSet<GameObject>> _affectedTargets =
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

    public void AddNewOverlappingTarget(AreaOfEffectData areaOfEffect, GameObject target)
    {
        Dictionary<GameObject, int> nestedDictionary = _areaOfEffectOverlappingTargets[areaOfEffect];
        
        nestedDictionary.TryAdd(target, 0);
        
        nestedDictionary[target]++;
    }

    public bool RemoveOverlappingTarget(AreaOfEffectData areaOfEffect, GameObject target)
    {
        bool shouldRemove = false;
        Dictionary<GameObject, int> nestedDictionary = _areaOfEffectOverlappingTargets[areaOfEffect];
        HashSet<GameObject>nestedDictionary2 = _affectedTargets[areaOfEffect];

        nestedDictionary[target]--;

        if (nestedDictionary[target] == 0)
        {
            shouldRemove = true;
            nestedDictionary.Remove(target);
            
            nestedDictionary2.Remove(target);
        }
        
        return shouldRemove;
    }

    public bool CheckIfTargetCanBeAffected(AreaOfEffectData areaOfEffect, GameObject target)
    {
        HashSet<GameObject>nestedDictionary2 = _affectedTargets[areaOfEffect];

        // If this is the first AOE that the target enters,
        // and the target is not affected by the AOE, then allow the AOE to apply its effect on the target
        if (CheckIfTargetIsOverlapping(areaOfEffect, target) && !nestedDictionary2.Contains(target))
        {
            nestedDictionary2.Add(target);
            return true;
        }
        
        else
        {
            return false;
        }
        
    }

    public bool CheckIfTargetIsOverlapping(AreaOfEffectData areaOfEffect, GameObject target)
    {
        Dictionary<GameObject, int> nestedDictionary = _areaOfEffectOverlappingTargets[areaOfEffect];

        return nestedDictionary[target] == 1;
    }
    
    
}

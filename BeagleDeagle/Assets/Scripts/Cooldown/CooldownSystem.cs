using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CooldownSystem : MonoBehaviour
{
    private readonly List<CooldownData> _cooldowns = new List<CooldownData>();

    /* A hash set of id's that are ready to be assigned to any monobehaviours that need one
     This removes the need of having to make sure two monobehaviours don't accidentally have the same cooldown ID*/
    private HashSet<int> _assignedIds = new HashSet<int>();
    
    [SerializeField, Range(1, 50)] 
    private int assignableIdCount = 20; // How many ID's are needed for this cooldown system?
    
    public Action<int> OnCooldownEnded;

    private void Awake()
    {
        // Assign unique IDs to the HashSet (starts at 1...this can break for some gameObjects if Ids are set to 0 for some reason?)
        for (int i = 1; i < assignableIdCount + 1; i++)
        {
            _assignedIds.Add(i);
        }
        
    }
    
    private void Update()
    {
        ProcessCooldowns();
    }

    ///-///////////////////////////////////////////////////////////
    /// Preforms the cooldown logic
    /// Checks to see if the cooldown is 0, then remove it
    /// 
    private void ProcessCooldowns()
    {
        float deltaTime = Time.deltaTime;

        // Loop backwards not forward, removing backwards will prevent it from shifting (prevents out of range error)
        for (int i = _cooldowns.Count - 1; i >= 0; i--)
        {
            //Decrement cooldown, if its 0, remove it 
            if (_cooldowns[i].DecrementCooldown(deltaTime))
            {

                OnCooldownEnded?.Invoke(_cooldowns[i].Id);
                
                _cooldowns.RemoveAt(i);
            }
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Used to add an object on cooldown
    /// Creates a new cooldown data and adds to our list
    /// 
    public void PutOnCooldown(IHasCooldown cooldown)
    {
        _cooldowns.Add(new CooldownData(cooldown));
    }

    ///-///////////////////////////////////////////////////////////
    /// Set a cooldown to zero (will be considered completed)
    /// 
    public void EndCooldown(int id)
    {
        foreach (CooldownData cooldown in _cooldowns)
        {
            if (cooldown.Id != id)
            {
                continue;
            }

            cooldown.EndTimer();

        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Remove a cooldown from list of cooldowns (will not be considered completed)
    /// 
    public void RemoveCooldown(int id)
    {
        for (int i = _cooldowns.Count - 1; i >= 0; i--)
        {
            if (_cooldowns[i].Id != id)
            {
                continue;
            }
            
            _cooldowns.RemoveAt(i);
        }
        
    }

    public void ChangeOngoingCooldownTime(int id, float newCooldownTime)
    {
        foreach (CooldownData cooldown in _cooldowns)
        {
            if (cooldown.Id != id)
            {
                continue;
            }

            cooldown.ChangeRemainingTime(newCooldownTime);

        }
    }

    public void RefreshCooldown(int id)
    {
        foreach (CooldownData cooldown in _cooldowns)
        {
            if (cooldown.Id != id)
            {
                continue;
            }

            cooldown.RefreshCooldownTime();
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Checks if cooldown at the indicated Id is on cooldown
    /// 
    public bool IsOnCooldown(int id)
    {
        //Search for correct cooldown
        foreach (CooldownData cooldown in _cooldowns)
        {
            //Return true if has a cooldown/is on cooldown
            if (cooldown.Id == id)
            {
                return true;
            }
        }

        //If we couldn't find it, its false
        return false;
    }

    ///-///////////////////////////////////////////////////////////
    /// Get an unique int value needed to start a cooldown instance.
    /// Call this in Start.
    /// 
    public int GetAssignableId()
    {
        if (_assignedIds.Count > 0)
        {
            // Get the first available ID from the HashSet
            int id = _assignedIds.First();
            
            // Remove the assigned ID from the HashSet
            _assignedIds.Remove(id);

            return id;
        }
        
        Debug.LogError("No more unique IDs available for " + gameObject.name);
        return -1; // Indicate that no unique ID is available
    }

    public float GetStartingDuration(int id)
    {
        foreach (CooldownData cooldown in _cooldowns)
        {
            if (cooldown.Id != id)
            {
                continue;
            }

            //return cooldown time
            return cooldown.startingTime;
        }

        return 0f;
    }

    ///-///////////////////////////////////////////////////////////
    /// Returns the specific cooldowns remaining time 
    /// 
    public float GetRemainingDuration(int id)
    {
        //Search for the right cooldown
        foreach (CooldownData cooldown in _cooldowns)
        {
            if (cooldown.Id != id)
            {
                continue;
            }

            //return cooldown time
            return cooldown.RemainingTime;
        }

        return 0f;
    }
}

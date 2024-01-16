using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffBarUI : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    [SerializeField] private GameObject buffIconPrefab;

    private Dictionary<Sprite, BuffIconUI> _passiveBuffIcons = new Dictionary<Sprite, BuffIconUI>();
    
    //public List<>

    private void OnEnable()
    {
        playerEvents.displayBuffOnHud += AddBuffImage;
        playerEvents.displayBuffWithDurationOnHud += AddBuffImageWithDuration;
        playerEvents.removeBuffFromHud += RemoveBuffImage;
    }
    

    private void OnDisable()
    {
        playerEvents.displayBuffOnHud -= AddBuffImage;
        playerEvents.displayBuffWithDurationOnHud -= AddBuffImageWithDuration;
        playerEvents.removeBuffFromHud -= RemoveBuffImage;
    }
    

    ///-///////////////////////////////////////////////////////////
    /// When the player receives a new passive ability, add its icon to the buff bar
    /// 
    private void AddBuffImage(Sprite spriteToDisplay)
    {
        // Check if this passive hasn't had an icon created for it before
        if (!_passiveBuffIcons.ContainsKey(spriteToDisplay))
        {
            // Spawn a child gameObject of the icon, a grid layout will place it automatically
            GameObject newBuffIcon = Instantiate(buffIconPrefab, transform);

            _passiveBuffIcons[spriteToDisplay] = newBuffIcon.GetComponent<BuffIconUI>();

            _passiveBuffIcons[spriteToDisplay].UpdateIcon(spriteToDisplay);
  
        }
        
        _passiveBuffIcons[spriteToDisplay].gameObject.SetActive(true);
        
    }
    
    private void AddBuffImageWithDuration(Sprite spriteToDisplay, float displayDuration)
    {
        AddBuffImage(spriteToDisplay);
        
        _passiveBuffIcons[spriteToDisplay].SetBuffTimer(displayDuration);
        
    }


    ///-///////////////////////////////////////////////////////////
    /// When a passive was disabled or removed, take its icon off the buff bar
    /// 
    private void RemoveBuffImage(Sprite spriteToRemove)
    {
        // If the passive exists, disable it
        if(_passiveBuffIcons.TryGetValue(spriteToRemove, out var icon))
            icon.gameObject.SetActive(false);
    }
}

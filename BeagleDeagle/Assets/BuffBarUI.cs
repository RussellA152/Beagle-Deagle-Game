using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffBarUI : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    [SerializeField] private GameObject buffIconPrefab;

    private Dictionary<Sprite, Image> _passiveBuffIcons = new Dictionary<Sprite, Image>();

    private void OnEnable()
    {
        playerEvents.displayBuffOnHud += AddBuffImage;
        playerEvents.removeBuffFromHud += RemoveBuffImage;
    }
    

    private void OnDisable()
    {
        playerEvents.displayBuffOnHud -= AddBuffImage;
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

            _passiveBuffIcons[spriteToDisplay] = newBuffIcon.GetComponent<Image>();

            _passiveBuffIcons[spriteToDisplay].sprite = spriteToDisplay;
        }
        
        _passiveBuffIcons[spriteToDisplay].gameObject.SetActive(true);
        
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
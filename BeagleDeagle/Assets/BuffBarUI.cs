using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffBarUI : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    [SerializeField] private GameObject buffIconPrefab;

    private Dictionary<PassiveAbilityData, Image> _passiveBuffIcons = new Dictionary<PassiveAbilityData, Image>();

    private void OnEnable()
    {
        playerEvents.onPlayerPassiveActivated += AddBuffImage;
        playerEvents.onPlayerPassiveDeactivated += RemoveBuffImage;
    }
    

    private void OnDisable()
    {
        playerEvents.onPlayerPassiveActivated -= AddBuffImage;
        playerEvents.onPlayerPassiveDeactivated -= RemoveBuffImage;
    }

    ///-///////////////////////////////////////////////////////////
    /// When the player receives a new passive ability, add its icon to the buff bar
    /// 
    private void AddBuffImage(PassiveAbilityData passiveAbilityData)
    {
        // Check if this passive hasn't had an icon created for it before
        if (!_passiveBuffIcons.ContainsKey(passiveAbilityData))
        {
            // Spawn a child gameObject of the icon, a grid layout will place it automatically
            GameObject newBuffIcon = Instantiate(buffIconPrefab, transform);

            _passiveBuffIcons[passiveAbilityData] = newBuffIcon.GetComponent<Image>();

            _passiveBuffIcons[passiveAbilityData].sprite = passiveAbilityData.abilityIcon;
        }
        
        _passiveBuffIcons[passiveAbilityData].gameObject.SetActive(true);
        
    }

    ///-///////////////////////////////////////////////////////////
    /// When a passive was disabled or removed, take its icon off the buff bar
    /// 
    private void RemoveBuffImage(PassiveAbilityData passiveAbilityData)
    {
        // If the passive exists, disable it
        if(_passiveBuffIcons.TryGetValue(passiveAbilityData, out var icon))
            icon.gameObject.SetActive(false);
    }
}

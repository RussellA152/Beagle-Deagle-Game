using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardChoiceUI : MonoBehaviour
{
    [SerializeField] private Button choicePrefab;
    
    public void AddChoiceButton(Reward rewardChoice)
    {
        Button newButton = Instantiate(choicePrefab, transform, false);
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

///-///////////////////////////////////////////////////////////
/// Display choices of rewards for the player to click on. Whichever was chosen
/// will be given to the player, the rest will be ignored.
public class RewardChoiceUI : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;
    
    [SerializeField] private GameObject choicePrefab;

    private Image _imageBackground;

    private List<Button> _allButtons = new List<Button>();

    // Need access to player to give them reward
    private GameObject _playerGameObject;

    // Did the player pick a reward? If so, don't allow any other choices
    private bool _rewardWasChosen;

    private void OnEnable()
    {
        playerEvents.givePlayerGameObject += FindPlayer;
    }

    private void Start()
    {
        _imageBackground = GetComponent<Image>();
        
        _imageBackground.enabled = false;
        _rewardWasChosen = false;
    }

    private void FindPlayer(GameObject pGameObject)
    {
        _playerGameObject = pGameObject;
    }

    public void AddChoiceButton(Reward rewardChoice)
    {
        _imageBackground.enabled = true;

        GameObject newButton = Instantiate(choicePrefab, transform, false);

        RewardChoice buttonRewardChoiceScript = newButton.GetComponent<RewardChoice>();
        
        _allButtons.Add(buttonRewardChoiceScript.GetButton());
        
        // Change the name, description, and icon image of the button to whatever the reward uses
        buttonRewardChoiceScript.SetName(rewardChoice.GetRewardName());
        buttonRewardChoiceScript.SetDescription(rewardChoice.Description);
        buttonRewardChoiceScript.SetIcon(rewardChoice.Icon);
        
        buttonRewardChoiceScript.GetButton().onClick.AddListener(() => GiveRewardToPlayerOnClick(rewardChoice));

    }

    private void GiveRewardToPlayerOnClick(Reward chosenReward)
    {
        // Don't give a reward if the player already picked one
        if (_rewardWasChosen) return;
        
        Debug.Log("Player picked " + chosenReward.Description);

        _rewardWasChosen = true;
        
        chosenReward.GiveDataToPlayer(_playerGameObject);
        
        RemoveAllButtons();
    }

    ///-///////////////////////////////////////////////////////////
    /// When player picks one reward, then delete all buttons from the UI
    /// 
    private void RemoveAllButtons()
    {
        foreach (Button buttonChoice in _allButtons)
        {
            buttonChoice.onClick.RemoveAllListeners();
            
            Destroy(buttonChoice.gameObject);
        }

        _imageBackground.enabled = false;
    }
}

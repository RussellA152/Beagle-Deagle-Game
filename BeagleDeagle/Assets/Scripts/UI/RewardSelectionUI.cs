using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;

///-///////////////////////////////////////////////////////////
/// Display choices of rewards for the player to click on. Whichever was chosen
/// will be given to the player, the rest will be ignored.
public class RewardSelectionUI : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    // When the rewards menu appears, pause the game
    [SerializeField] private GamePauser gamePauser;

    [SerializeField] private Image optionalRewardPanel;
    
    [SerializeField] private GameObject choicePrefab;
    
    // Text box that will display the description of mandatory rewards (rewards that the player has no choice to receive)
    [SerializeField] private TMP_Text mandatoryRewardDescription;
    
    [SerializeField, Range(0.1f, 5f)] 
    private float rewardDescriptionDisplayTime = 3f;
    

    private List<Button> _allButtons = new List<Button>();

    // Need access to player to give them reward
    private GameObject _playerGameObject;

    // Did the player pick a reward? If so, don't allow any other choices
    private bool _rewardWasChosen;

    private void OnEnable()
    {
        playerEvents.givePlayerGameObject += FindPlayer;

        playerEvents.onPlayerReceivedOptionalRewards += AddChoiceButton;
        playerEvents.onPlayerReceivedMandatoryReward += DisplayMandatoryRewardDescription;
    }

    private void OnDisable()
    {
        playerEvents.givePlayerGameObject -= FindPlayer;

        playerEvents.onPlayerReceivedOptionalRewards -= AddChoiceButton;
        playerEvents.onPlayerReceivedMandatoryReward -= DisplayMandatoryRewardDescription;
    }

    private void Start()
    {
        // Hide description text and reward panel
        mandatoryRewardDescription.gameObject.SetActive(false);
        optionalRewardPanel.enabled = false;
        
        _rewardWasChosen = false;
    }

    private void FindPlayer(GameObject pGameObject)
    {
        _playerGameObject = pGameObject;
    }

    public void AddChoiceButton(List<Reward> rewardChoices)
    {
        gamePauser.PauseGameAutomatically();
        
        optionalRewardPanel.enabled = true;
        

        foreach (Reward potentialReward in rewardChoices)
        {
            GameObject newButton = Instantiate(choicePrefab, optionalRewardPanel.transform, false);

            RewardChoiceUIElement buttonRewardChoiceUIElementScript = newButton.GetComponent<RewardChoiceUIElement>();
        
            _allButtons.Add(buttonRewardChoiceUIElementScript.GetButton());
        
            // Change the name, description, and icon image of the button to whatever the reward uses
            buttonRewardChoiceUIElementScript.SetName(potentialReward.GetRewardName());
            buttonRewardChoiceUIElementScript.SetDescription(potentialReward.Description);
            buttonRewardChoiceUIElementScript.SetIcon(potentialReward.Icon);
        
            buttonRewardChoiceUIElementScript.GetButton().onClick.AddListener(() => GiveRewardToPlayerOnClick(potentialReward));
        }
        
        // Set UI EventSystem's "firstSelected" gameObject to the first reward choice button
        EventSystem.current.SetSelectedGameObject(_allButtons[0].gameObject);

    }

    private void GiveRewardToPlayerOnClick(Reward chosenReward)
    {
        // Don't give a reward if the player already picked one
        if (_rewardWasChosen) return;
        
        Debug.Log("Player picked " + chosenReward.Description);

        _rewardWasChosen = true;
        
        // Have reward give its data to the player
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

        optionalRewardPanel.enabled = false;
        
        gamePauser.ResumeGame();
    }

    private void DisplayMandatoryRewardDescription(Reward mandatoryReward)
    {
        StartCoroutine(RemoveDescriptionAfterTime(mandatoryReward));
    }

    ///-///////////////////////////////////////////////////////////
    /// When the player receives a "mandatory" reward (meaning the player will always receive that reward, no choices),
    /// a text box will appear above the player's head with the description of the reward. Shortly after, it will disappear again.
    private IEnumerator RemoveDescriptionAfterTime(Reward mandatoryReward)
    {
        // Change text
        mandatoryRewardDescription.gameObject.SetActive(true);
        mandatoryRewardDescription.text = mandatoryReward.Description;
        
        yield return new WaitForSeconds(rewardDescriptionDisplayTime);
        
        // Remove text,then disable text object
        mandatoryRewardDescription.text = string.Empty;
        mandatoryRewardDescription.gameObject.SetActive(false);
    }
    
}

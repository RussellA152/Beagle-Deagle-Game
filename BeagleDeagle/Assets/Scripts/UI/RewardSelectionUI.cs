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
    private Queue<List<LevelUpReward>> _optionalRewardQueue = new Queue<List<LevelUpReward>>();

    // Need access to player to give them reward
    private GameObject _playerGameObject;

    [Header("Sounds")] 
    [SerializeField] private AudioClip optionalRewardSound;
    [SerializeField] private AudioClip mandatoryRewardSound;
    [SerializeField, Range(0.1f, 1f)] 
    private float volume;
    private AudioClipPlayer _audioClipPlayer;

    // Did the player pick a reward? If so, don't allow any other choices
    private bool _rewardWasChosen;
    
    // Is there currently a set of optional reward being displayed on screen? If so, wait until that one goes away for another to appear
    private bool _optionalRewardIsBeingDisplayed;
    // Is there currently a mandatory reward description being displayed on screen? If so, other mandatory rewards must wait for that one to go away
    private bool _mandatoryRewardIsBeingDisplayed;

    private void Awake()
    {
        _audioClipPlayer = GetComponent<AudioClipPlayer>();
    }

    private void OnEnable()
    {
        _optionalRewardQueue.Clear();
        
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
        _optionalRewardIsBeingDisplayed = false;
        _mandatoryRewardIsBeingDisplayed = false;
    }

    private void FindPlayer(GameObject pGameObject)
    {
        _playerGameObject = pGameObject;
    }

    public void AddChoiceButton(List<LevelUpReward> rewardChoices)
    {
        // Add any rewards from a level to a queue (prevents multiple rewards from differing levels from appearing at once)
        _optionalRewardQueue.Enqueue(rewardChoices);
        
        // Don't display more than one set of rewards at a time
        if(!_optionalRewardIsBeingDisplayed)
            AddChoices(rewardChoices);
    }

    private void AddChoices(List<LevelUpReward> rewardChoices)
    {
        _optionalRewardIsBeingDisplayed = true;
        
        gamePauser.PauseGameAutomatically();
        
        optionalRewardPanel.enabled = true;
        
        if(_audioClipPlayer != null)
            _audioClipPlayer.PlayGeneralAudioClip(optionalRewardSound, volume);

        foreach (LevelUpReward potentialReward in rewardChoices)
        {
            GameObject newButton = Instantiate(choicePrefab, optionalRewardPanel.transform, false);

            RewardChoiceUIElement buttonRewardChoiceUIElementScript = newButton.GetComponent<RewardChoiceUIElement>();
        
            _allButtons.Add(buttonRewardChoiceUIElementScript.GetButton());
        
            // Change the name, description, and icon image of the button to whatever the reward uses
            buttonRewardChoiceUIElementScript.SetName(potentialReward.GetRewardName());
            buttonRewardChoiceUIElementScript.SetDescription(potentialReward.GetDescription());
            buttonRewardChoiceUIElementScript.SetIcon(potentialReward.SpriteIcon);
        
            buttonRewardChoiceUIElementScript.GetButton().onClick.AddListener(() => GiveRewardToPlayerOnClick(potentialReward));
        }
        
        // Set UI EventSystem's "firstSelected" gameObject to the first reward choice button
        EventSystem.current.SetSelectedGameObject(_allButtons[0].gameObject);
    }

    private void GiveRewardToPlayerOnClick(LevelUpReward chosenLevelUpReward)
    {
        // Don't give a reward if the player already picked one
        if (_rewardWasChosen) return;
        
        Debug.Log("Player picked " + chosenLevelUpReward.GetDescription());

        _rewardWasChosen = true;
        
        _optionalRewardIsBeingDisplayed = false;
        
        // Remove the current set of rewards from the queue, because the player has made their choice
        _optionalRewardQueue.Dequeue();
        
        // Have reward give its data to the player
        chosenLevelUpReward.GiveDataToPlayer(_playerGameObject);
        
        RemoveAllButtons();
        
        // If there are still other rewards to be chosen, then display them to the player
        if(_optionalRewardQueue.Count > 0)
            AddChoices(_optionalRewardQueue.Peek());
        
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
        
        _allButtons.Clear();
    
        // Hide button panel
        optionalRewardPanel.enabled = false;
        
        // Reset bool back to false so player can choose another reward in future level ups
        _rewardWasChosen = false;
        
        gamePauser.ResumeGame();
    }

    private void DisplayMandatoryRewardDescription(LevelUpReward mandatoryLevelUpReward)
    {
        if(_audioClipPlayer != null)
            _audioClipPlayer.PlayGeneralAudioClip(mandatoryRewardSound, volume);
        
        StartCoroutine(RemoveDescriptionAfterTime(mandatoryLevelUpReward));
    }

    ///-///////////////////////////////////////////////////////////
    /// When the player receives a "mandatory" reward (meaning the player will always receive that reward, no choices),
    /// a text box will appear above the player's head with the description of the reward. Shortly after, it will disappear again.
    private IEnumerator RemoveDescriptionAfterTime(LevelUpReward mandatoryLevelUpReward)
    {
        while (_mandatoryRewardIsBeingDisplayed)
            yield return null;
        
        // Change text
        mandatoryRewardDescription.gameObject.SetActive(true);
        mandatoryRewardDescription.text = mandatoryLevelUpReward.GetDescription();

        _mandatoryRewardIsBeingDisplayed = true;
        
        yield return new WaitForSeconds(rewardDescriptionDisplayTime);
        
        // Remove text,then disable text object
        mandatoryRewardDescription.text = string.Empty;
        mandatoryRewardDescription.gameObject.SetActive(false);

        _mandatoryRewardIsBeingDisplayed = false;
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardChoice : MonoBehaviour
{
    private Button _button;
    private Image _image;
    
    [SerializeField] private TMP_Text nameText;
    
    [SerializeField] private TMP_Text descriptionText;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
    }

    public Button GetButton()
    {
        return _button;
    }

    public void SetIcon(Image rewardImage)
    {
        _image = rewardImage;
    }
    
    public void SetDescription(string rewardDescription)
    {
        descriptionText.text = rewardDescription;
    }

    public void SetName(string rewardName)
    {
        nameText.text = rewardName;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WheelDisplayer : MonoBehaviour{
    public Controller controller;
    public TextMeshProUGUI wheelHeaderText;
    public TextMeshProUGUI wheelBonusText;
    public Image wheelImage;
    public Image indicatorImage;
    public Color[] spinTypeColors;
    public string[] spinTypeHeaders;
    public string[] spinTypeBonuses;
    public Sprite[] spinTypeWheelSprites;
    public Sprite[] spinTypeIndicatorSprites;
    public Transform wheelObject;
    public Transform indicatorObject;

    public void updateUI(int type){
        wheelHeaderText.text = spinTypeHeaders[type];
        wheelBonusText.text = spinTypeBonuses[type];
        wheelHeaderText.color = spinTypeColors[type];
        wheelBonusText.color = spinTypeColors[type];
        wheelImage.sprite = spinTypeWheelSprites[type];
        indicatorImage.sprite = spinTypeIndicatorSprites[type];
    }
}

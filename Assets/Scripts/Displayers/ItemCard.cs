using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCard : MonoBehaviour{
    [Header("UI Related")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemAmountText;
    public ItemStore itemStore;
    private float displayedAmount;
    [Header("Animation Related")]
    public float curSize;
    public float expandAnimationDuration;
    public float movementDuration;
    public float movementPoint;
    [SerializeField] private Transform startingPosition;
    private Transform moveTarget;
    private RectTransform moveTargetRectTransform;
    
    private bool moveAnimationEnd;

    void Update(){
        if(panel.activeSelf && itemStore.item != null){
            if(curSize < 1){
                expand();
            }else if(movementPoint < movementDuration){
                moveAndShrink();
            }else if(displayedAmount != itemStore.amount){
                updateUI();
            }
        }

    }

    public void updateUI(){
            displayedAmount = moveAnimationEnd?Mathf.MoveTowards(displayedAmount,itemStore.amount, (float)(itemStore.amount*1.7f-displayedAmount)*Time.deltaTime):displayedAmount;
            if (displayedAmount > itemStore.amount) { 
                displayedAmount = itemStore.amount;
            }
            itemIcon.sprite = itemStore.item.itemIcon;
            if (itemNameText != null){
                itemNameText.text = itemStore.item.itemName;
            }

            if (displayedAmount ==0) {
                itemAmountText.text = string.Empty;
            }else if (itemStore.amount == 1) {
                itemAmountText.text = "Unlocked";
            }else {
                itemAmountText.text = "x" + displayedAmount.ToString("f0");
            }
    }

    public void updateValues(Item item, int amount, bool displayDirectly){
        itemStore.item = item;
        itemStore.amount = amount;
        moveAnimationEnd = displayDirectly ? true : moveAnimationEnd;
        if (displayDirectly){
            displayedAmount = itemStore.amount;
            updateUI();
        }
    }

    public void changeAmount(int amount, bool displayDirectly){
        itemStore.amount += amount;
        displayedAmount = displayDirectly ? amount : displayedAmount;
        updateUI();
    }

    public void startExpand(){
        panel.SetActive(true);
        curSize = 0;
        panel.transform.localScale = new Vector3(curSize, curSize, curSize);
        transform.position = startingPosition.position;
        movementPoint = movementDuration + 1;
    }

    public void startMoveAndShrink(Transform moveTarget){
        movementPoint = 0;
        this.moveTarget = moveTarget;
        moveTargetRectTransform = moveTarget.GetComponent<RectTransform>();
    }

    public void expand(){
        curSize = Mathf.MoveTowards(curSize, 1, (1 / expandAnimationDuration) * Time.deltaTime);
        panel.transform.localScale = new Vector3(curSize, curSize, curSize);
    }

    

    public void moveAndShrink(){
        movementPoint = Mathf.MoveTowards(movementPoint, movementDuration, 1 * Time.deltaTime); 
        float moveProgress = movementPoint / movementDuration;
        transform.localScale = (1 - moveProgress) * new Vector3(1, 1, 1);
        transform.position = moveProgress * (moveTarget.position - new Vector3(moveTargetRectTransform.sizeDelta.x/5f,0,0)) + (1 - moveProgress) * startingPosition.position;
        if(movementPoint == movementDuration){
            gameObject.SetActive(false);
        }
    }

    public void setMoveAnimationEnd(bool moveAnimationEnd){
        this.moveAnimationEnd = moveAnimationEnd;
    }

    public override bool Equals(object obj){
        if (obj is ItemCard other){
            return itemStore.item.itemID == other.itemStore.item.itemID;
        }
        return false;
    }

    public override int GetHashCode(){
        return itemStore.item.itemID.GetHashCode();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour{
    public Controller controller;
    public Button[] buttonList;


    void Start() {
        buttonList[ButtonConstants.Button_Exit].onClick.AddListener(() => controller.restartLevel());
        buttonList[ButtonConstants.Button_Spin].onClick.AddListener(() => controller.spinManager.spin());
        buttonList[ButtonConstants.Button_Revive].onClick.AddListener(() => controller.revive());
        buttonList[ButtonConstants.Button_GiveUp].onClick.AddListener(() => controller.restartLevel());
    }


    void Update(){
        buttonList[ButtonConstants.Button_Exit].interactable = buttonExitInteractable();
        buttonList[ButtonConstants.Button_Spin].interactable = buttonSpinInteractable();
        buttonList[ButtonConstants.Button_Revive].interactable = buttonReviveInteractable();
        buttonList[ButtonConstants.Button_GiveUp].interactable = buttonGiveUpInteractable();
    }

    public bool buttonExitInteractable(){
        if(controller.wheelManager.rotating){
            return false;
        }

        if(controller.spinManager.spinID%GameConstants.Silver_Spin_Interval != 0 && controller.spinManager.spinID%GameConstants.Gold_Spin_Interval!=0){
            return false;
        }

        if (controller.spinManager.wonItem.gameObject.activeSelf){
            return false;
        }

        return true;
    }

    public bool buttonSpinInteractable(){
        return !controller.wheelManager.rotating;
    }

    public bool buttonReviveInteractable(){
        if (!controller.spinManager.wonItem.gameObject.activeSelf){
            return false;
        }

        return controller.acquiredItemsDisplayer.acquiredItemCards.ContainsKey(GameConstants.Item_Gold_ID) && controller.acquiredItemsDisplayer.acquiredItemCards[GameConstants.Item_Gold_ID].itemStore.amount >= controller.getReviveCost();

    }

    public bool buttonGiveUpInteractable() {
        return true;
    }
}

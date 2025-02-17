using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WheelManager : MonoBehaviour{
    public Controller controller;
    public WheelSlot[] wheelSlots = new WheelSlot[8];
    public Item[] itemList = new Item[0];
    public Item deathItem;
    public bool rotating;
    public float targetZAngle;

    public void initializeSlots(int spinID){
        float seedValue = 0.05f + spinID * 0.05f;
        int type = GameConstants.Normal_Spin_Type;
        seedValue = Mathf.Min(0.334f, seedValue);
        if (spinID % GameConstants.Gold_Spin_Interval == 0){
            seedValue *= 3f;
            type = GameConstants.Gold_Spin_Type;
        }else if (spinID % GameConstants.Silver_Spin_Interval == 0) {
            type = GameConstants.Silver_Spin_Type;
        } 
        
        selectItems(wheelSlots.Length, seedValue, type);
        controller.wheelDisplayer.updateUI(type);
    }

    public void selectItems(int itemCount, float seed, int type){
        List<(Item item, float probability)> itemProbabilities = new List<(Item, float)>();

        foreach (var item in itemList){
            float probability = item.getWheelPossibility(seed);
            if (probability > 0){
                itemProbabilities.Add((item, probability));
            }
        }

        List<Item> selectedItems = new List<Item>();
        for (int i = 0; i < itemCount; i++){
            Item selectedItem = selectItem(ref itemProbabilities);
            if (selectedItem != null){
                selectedItems.Add(selectedItem);
            }
        }

        for (int i = 0; i < selectedItems.Count; i++){
            wheelSlots[i].updateValue(selectedItems[i], (int)selectedItems[i].getAmount(seed));
        }

        if(type == GameConstants.Normal_Spin_Type){ //if it is normal spin (not safe), then place bomb item at the wheel
            wheelSlots[itemCount - 1].updateValue(deathItem,0);
        }
    }

    private Item selectItem(ref List<(Item item, float probability)> itemProbabilities){
        if (itemProbabilities.Count == 0) return null;

        float totalProbability = itemProbabilities.Sum(x => x.probability);
        if (totalProbability == 0) return null;

        float cumulativeSum = 0;
        List<(Item item, float cumulativeProbability)> cumulativeList = new List<(Item, float)>();

        foreach (var entry in itemProbabilities){
            cumulativeSum += entry.probability / totalProbability;
            cumulativeList.Add((entry.item, cumulativeSum));
        }

        float randomValue = Random.value; 

        for (int i = 0; i < cumulativeList.Count; i++){
            if (randomValue <= cumulativeList[i].cumulativeProbability){
                Item selectedItem = cumulativeList[i].item;
                for (int j = 0; j < itemProbabilities.Count; j++){
                    if (itemProbabilities[j].item == selectedItem){
                        itemProbabilities[j] = (selectedItem, 0);
                        break;
                    }
                }

                if (!selectedItem.accumulatable && controller.acquiredItemsDisplayer.acquiredItemCards.ContainsKey(selectedItem.itemID)){
                    //if selected item is not accumulatable, and already in the inventory, then place gold item in the place of that item at the wheel.
                    return itemProbabilities[GameConstants.Item_Gold_ID].item; 
                }
                return selectedItem;
            }
        }

        return null;
    }
}

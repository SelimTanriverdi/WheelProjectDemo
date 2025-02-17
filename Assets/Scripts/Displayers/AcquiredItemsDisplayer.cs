using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquiredItemsDisplayer : MonoBehaviour{
    public Controller controller;
    public Dictionary<int, ItemCard> acquiredItemCards = new Dictionary<int, ItemCard>();
    public ItemCard acquiredItem;
    public Transform acquiredItemsContainer;
    public Vector3 initialPosition;
    public float yDifference;

    public ItemCard generateItem(ItemStore item){
        ItemCard initialized = Instantiate(acquiredItem, acquiredItemsContainer);
        initialized.gameObject.SetActive(true);
        initialized.updateValues(item.item,item.amount,false);
        acquiredItemCards.Add(item.item.itemID,initialized);
        insertItemCardByOrder(acquiredItemCards[item.item.itemID]);
        return acquiredItemCards[item.item.itemID];
    }

    private void insertItemCardByOrder(ItemCard newItem){
        int newItemID = newItem.itemStore.item.itemID;
        for (int i = 0; i < acquiredItemsContainer.childCount; i++){
            ItemCard existingItem = acquiredItemsContainer.GetChild(i).GetComponent<ItemCard>();
            if (newItemID < existingItem.itemStore.item.itemID){
                newItem.transform.SetSiblingIndex(i);
                return;
            }
        }
        newItem.transform.SetSiblingIndex(acquiredItemsContainer.childCount - 1);
    }

    public void clear(){
        acquiredItemCards.Clear(); 
        foreach (Transform child in acquiredItemsContainer){
            Destroy(child.gameObject);
        }
    }
}

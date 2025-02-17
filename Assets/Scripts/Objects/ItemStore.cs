using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStore : MonoBehaviour{
    public Item item;
    public int amount;


    public void loadItem(Item item, int amount){
        this.item = item;
        this.amount = amount;
    }

}

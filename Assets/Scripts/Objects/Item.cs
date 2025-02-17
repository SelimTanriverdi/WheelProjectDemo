using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject{
    public int itemID;
    public string itemName;
    public int itemMinAmount;
    public int itemMaxAmount;
    public float wheelPossibility;
    public Sprite itemIcon;
    public bool accumulatable;
    public float wheelIconScale;

    public int getAmount(float seed){
        if(itemMinAmount == itemMaxAmount){
            return itemMinAmount;
        }
        float exponent = Mathf.Log(1f / 6f) / Mathf.Log(0.5f);
        float t = Mathf.Pow(seed, exponent);
        int amount = (int)(Mathf.Lerp(itemMinAmount, itemMaxAmount, t)*Random.Range(0.8f, 2f));
        if(amount > 10){
            amount = (amount / 5) * 5;
        }
        return amount;
    }

    public float getWheelPossibility(float seed){
        float limitingValue = 0.5f;
        float maxIncrease = 0.3f;
        float maxDecrease = 0.5f;
        if (wheelPossibility == 0) return 0;
        else if (seed <= 0) return wheelPossibility; 
        else if (seed >= 1){
            return wheelPossibility < limitingValue ?
                Mathf.Min(wheelPossibility + maxIncrease, 2f): Mathf.Max(wheelPossibility - maxDecrease, 0f);
        }

        if (wheelPossibility < limitingValue){
            return Mathf.Lerp(wheelPossibility, wheelPossibility + maxIncrease, seed);
        }else{
            return Mathf.Lerp(wheelPossibility, wheelPossibility - maxDecrease, seed);
        }
    }

}

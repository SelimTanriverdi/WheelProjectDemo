using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelSlot : MonoBehaviour{
    public ItemStore itemStore;
    public Image itemIconImage;
    public TextMeshProUGUI itemAmountText;
    public float imageMaxWidth = 65;
    public float imageMaxHeight = 35;

    public void updateValue(Item item, int amount){
        itemStore.loadItem(item, amount);
        itemIconImage.sprite = itemStore.item.itemIcon;
        if(amount > 0){
            itemAmountText.text = "x" + getItemValue(itemStore.amount);
        }else{
            itemAmountText.text = string.Empty;
        }


        //this part of the code do resizing of the image according to sprite's dimensions
        float textureWidth = itemIconImage.sprite.texture.width;
        float textureHeight = itemIconImage.sprite.texture.height;
        float aspectRatio = textureWidth / textureHeight;

        float finalWidth, finalHeight;

        if (aspectRatio > 1){
            finalWidth = imageMaxWidth;
            finalHeight = imageMaxWidth / aspectRatio;

            if (finalHeight > imageMaxHeight){
                finalHeight = imageMaxHeight;
                finalWidth = imageMaxHeight * aspectRatio;
            }
        }else{
            finalHeight = imageMaxHeight;
            finalWidth = imageMaxHeight * aspectRatio;

            if (finalWidth > imageMaxWidth){
                finalWidth = imageMaxWidth;
                finalHeight = imageMaxWidth / aspectRatio;
            }
        }

        itemIconImage.rectTransform.sizeDelta = new Vector2(finalWidth, finalHeight);
    }


    public string getItemValue(int amount){
        if(amount > 1000000){
            return ((float)amount / 1000000).ToString("0.#",CultureInfo.InvariantCulture) + "M";
        }
        if(amount > 1000){
            return ((float)amount / 1000).ToString("0.#",CultureInfo.InvariantCulture) + "K";
        }

        return amount.ToString();
    }
}

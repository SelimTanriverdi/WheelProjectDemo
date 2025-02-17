using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinNumbersDisplayer : MonoBehaviour{
    public Controller controller;
    public RectTransform numbersContainer;
    public float numbersContainerShiftingSpeed;
    Vector3 numberContainerTargetPos;
    bool moving;
    public RectTransform lastSpawnedNumber;
    public TextMeshProUGUI spawnPrefab;
    public Image centralNumberBorderImage;
    public int numberAtSide;
    public float xDifference;

    private void Update(){
        if (moving){
            if(Vector3.Distance(numbersContainer.transform.localPosition,numberContainerTargetPos) > xDifference/10){
                numbersContainer.transform.localPosition = Vector3.MoveTowards(numbersContainer.transform.localPosition,numberContainerTargetPos,numbersContainerShiftingSpeed*Time.deltaTime);
            }else{
                numbersContainer.transform.localPosition = numberContainerTargetPos;
                moving = false;
            }
        }
    }
    public void initialize(int startingSpin){
        instantiateText(startingSpin, numbersContainer.localPosition.x * -1);
        for (int i = 1; i < numberAtSide; i++) {
            instantiateText(startingSpin+i, lastSpawnedNumber.localPosition.x + xDifference);
        }
        centralNumberBorderImage.color = getCenterNumber().color;
    }

    public void clear(){
        foreach (Transform child in numbersContainer){
            Destroy(child.gameObject);
        }
        numbersContainer.localPosition = new Vector3(182.75f, 0, 0);
        lastSpawnedNumber = null;
        numberContainerTargetPos = Vector3.zero;
    }

    public void updateUI(int spinID){
        TextMeshProUGUI centerNumber = getCenterNumber();
        centerNumber.color = new Color(centerNumber.color.r,centerNumber.color.g,centerNumber.color.b,0.5f);
        instantiateText(spinID+numberAtSide,lastSpawnedNumber.localPosition.x+xDifference);
        centralNumberBorderImage.color= getCenterNumber().color;
        numberContainerTargetPos = new Vector3(numbersContainer.transform.localPosition.x - xDifference, 0, 0);
        moving = true;
    }

    public void instantiateText(int spinID, float xPos){
        TextMeshProUGUI startingNumber = Instantiate(spawnPrefab, numbersContainer);
        startingNumber.rectTransform.localPosition = new Vector3(xPos, 0, 0);
        updateTextProperties(startingNumber, spinID);
        lastSpawnedNumber = startingNumber.rectTransform;
    }

    public void updateTextProperties(TextMeshProUGUI number, int spin){
        number.gameObject.SetActive(true);
        number.text = spin.ToString();
        if (spin % GameConstants.Gold_Spin_Interval == 0){
            number.color = controller.wheelDisplayer.spinTypeColors[2];
        }else if (spin % GameConstants.Silver_Spin_Interval == 0){
            number.color = controller.wheelDisplayer.spinTypeColors[1];
        }
    }

    public TextMeshProUGUI getCenterNumber(){
        return numbersContainer.transform.GetChild(numbersContainer.transform.childCount-(numberAtSide)).GetComponent<TextMeshProUGUI>();
    }
}

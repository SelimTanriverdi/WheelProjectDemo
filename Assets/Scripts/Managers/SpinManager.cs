using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinManager : MonoBehaviour {
    public Controller controller;

    public float initialSpeed = 1000f;
    public float minSpeed = 50f;
    Vector3 originalIndicatorPosition = Vector3.zero;
    public int spinID;
    public ItemCard wonItem;
    float deceleration;

    public void initialize() {
        spinID = 0;
        prepareWheel();
        controller.spinNumbersDisplayer.initialize(spinID);
    }

    public void clear(){
        wonItem.curSize = 0;
        wonItem.gameObject.SetActive(false);
    }


    public void spin() {
        StartCoroutine(SpinRoutine());
    }


    public void afterSpin() {
        controller.spinNumbersDisplayer.updateUI(spinID);
        StartCoroutine(displayItemCoroutine());
    }

    private IEnumerator displayItemCoroutine(){
        ItemStore curItem = controller.wheelManager.wheelSlots[Mathf.RoundToInt(controller.wheelDisplayer.wheelObject.transform.eulerAngles.z) / 45].itemStore;
        wonItem.startExpand();
        wonItem.updateValues(curItem.item, curItem.amount, true);
        yield return new WaitForSeconds(wonItem.expandAnimationDuration);
        if(curItem.item.itemID != GameConstants.Item_Death_ID){ //if the item is not bomb
            ItemCard targetPos = increaseStorage(curItem);
            targetPos.setMoveAnimationEnd(false);
            yield return new WaitForSeconds(wonItem.expandAnimationDuration);
            wonItem.startMoveAndShrink(targetPos.transform);
            yield return new WaitForSeconds(wonItem.movementDuration);
            targetPos.setMoveAnimationEnd(true);
            prepareWheel();
        }else{
            controller.bombExploded();
        }

    }

    public ItemCard increaseStorage(ItemStore curItem){
        if (controller.acquiredItemsDisplayer.acquiredItemCards.TryGetValue(curItem.item.itemID, out ItemCard existingItem)){
            existingItem.changeAmount(curItem.amount, false); //if item already exists, increase its amount
            return existingItem;
        }

        return controller.acquiredItemsDisplayer.generateItem(curItem); //otherwise generate new item

    }
    public void prepareWheel() {
        spinID += 1;
        controller.wheelManager.initializeSlots(spinID);
    }

    private IEnumerator SpinRoutine(){
        int segments = Random.Range(60, 68); // 1 segment = 45 degree rotation (one item on the wheel)
        controller.wheelManager.rotating = true;

        float totalAngle = segments * 45f;

        float startAngle = controller.wheelDisplayer.wheelObject.localEulerAngles.z;
        float endAngle = startAngle + totalAngle;

        float currentSpeed = initialSpeed;
        float angleSoFar = 0f;
        float speedDeductionPerSegment = (initialSpeed - minSpeed) / segments;
        int boundariesCrossed = 0;
        float nextBoundary = 45f; 
        float tingleThreshold = 5f; 
        while (angleSoFar < totalAngle){
            float rotateThisFrame = currentSpeed * Time.deltaTime;
            angleSoFar += rotateThisFrame;
            if (angleSoFar > totalAngle){
                rotateThisFrame -= (angleSoFar - totalAngle);
                angleSoFar = totalAngle;
            }
            controller.wheelDisplayer.wheelObject.Rotate(0f, 0f, -rotateThisFrame);
            if (angleSoFar >= nextBoundary - tingleThreshold){
                StartCoroutine(TingleEffect());
                nextBoundary += 45f; 
            }
            int newBoundariesCrossed = Mathf.FloorToInt(angleSoFar / 45f);
            if (newBoundariesCrossed > boundariesCrossed){
                int boundariesPassed = newBoundariesCrossed - boundariesCrossed;
                boundariesCrossed = newBoundariesCrossed;

                currentSpeed -= speedDeductionPerSegment * boundariesPassed;
                if (currentSpeed < minSpeed)
                {
                    currentSpeed = minSpeed;
                }
            }

            yield return null;
        }

        Vector3 finalEuler = controller.wheelDisplayer.wheelObject.localEulerAngles;
        float snappedZ = Mathf.Round(finalEuler.z / 45f) * 45f;
        finalEuler.z = snappedZ;
        controller.wheelDisplayer.wheelObject.localEulerAngles = finalEuler;

        controller.wheelManager.rotating = false;
        afterSpin();
    }



    private IEnumerator TingleEffect(){ //tingling effect of the indicator when wheel passes each segment
        Transform indicator = controller.wheelDisplayer.indicatorObject;
        if(originalIndicatorPosition == Vector3.zero){
            originalIndicatorPosition = indicator.localPosition;
        }

        float tingleAmount = 6f;
        float tingleDuration = 0.025f;
        float frequency = 30f;
        float elapsedTime = 0f;

        while (elapsedTime < tingleDuration){
            elapsedTime += Time.deltaTime;
            float t = Mathf.Sin(elapsedTime * frequency) * tingleAmount;
            indicator.localPosition = originalIndicatorPosition + new Vector3(0, t, 0);
            yield return null;
        }

        indicator.localPosition = originalIndicatorPosition;
    }


}

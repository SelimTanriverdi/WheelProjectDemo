using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Controller : MonoBehaviour{
    public WheelManager wheelManager;
    public SpinManager spinManager;
    public ButtonManager buttonManager;
    public WheelDisplayer wheelDisplayer;
    public SpinNumbersDisplayer spinNumbersDisplayer;
    public AcquiredItemsDisplayer acquiredItemsDisplayer;
    public Image gameEndScreen;
    public Transform[] gameEndButtons;
    public TextMeshProUGUI reviveCostText;
    private void Start(){
        spinManager.initialize();
    }

    public void restartLevel(){
        spinNumbersDisplayer.clear();
        acquiredItemsDisplayer.clear();
        spinManager.clear();
        spinManager.initialize();
        StartCoroutine(fadeAndScale(0, 0.5f));
    }

    public void revive(){
        spinManager.clear();
        acquiredItemsDisplayer.acquiredItemCards[GameConstants.Item_Gold_ID].changeAmount(getReviveCost(-1)*-1,true);
        StartCoroutine(fadeAndScale(0,0.5f));
    }

    public void bombExploded(){
        reviveCostText.text = getReviveCost().ToString();
        gameEndScreen.gameObject.SetActive(true);
        StartCoroutine(fadeAndScale(0.88f, 0.88f));

    }

    private IEnumerator fadeAndScale(float targetValue, float duration){ //fading and scaling animation of the game end screen when bomb is encountered at the wheel
        float time = 0;
        Color startColor = gameEndScreen.color;
        Vector3 startScale = gameEndButtons[0].localScale; 

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            gameEndScreen.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startColor.a, targetValue, t));

            Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * targetValue, t);
            foreach (var button in gameEndButtons) {
                button.localScale = newScale;
            }
                
            yield return null;
        }

        gameEndScreen.color = new Color(startColor.r, startColor.g, startColor.b, targetValue);

        foreach (var button in gameEndButtons){
            button.localScale = Vector3.one * targetValue;
        }

        if(targetValue == 0){
            gameEndScreen.gameObject.SetActive(false);
        }
    }

    public int getReviveCost(int substracter = 0){
        return (spinManager.spinID-substracter) * GameConstants.Revive_Cost_By_Spin;
    }
}

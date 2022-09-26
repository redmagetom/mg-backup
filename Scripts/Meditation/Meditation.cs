using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meditation : MonoBehaviour
{
    public DataManager dataManager;
    public MainManager mainManager;
    public bool held;
    public GameObject darknessOverlay;
    public GameObject finishedPopUp;
    public GameObject backdrop;
    public Text meditationCount;
    public Text sessionLongest;
    public int prevMeditation;
    public int meditationTime;
    public int fingers;
    private bool meditating;
    private bool doneCountdown;
    public float doneCount;
    private LTDescr fadetween;
    private UnityEngine.Coroutine med;
    void Start(){
        finishedPopUp.SetActive(false);
        // backdrop.SetActive(false);
    }
    void Update(){
        if(!held){
            meditating = false;
            ResetOverlayColor();
        }
        if(held && !meditating){
            meditating = true;
            med = StartCoroutine(CountMeditation());
        }     
        if(doneCountdown && doneCount < 3){
            doneCount += 1 * Time.deltaTime;
        }
        if(doneCount >= 3){
            doneCount = 0;
            doneCountdown = false;
            // backdrop.SetActive(true);
            finishedPopUp.SetActive(true);
        }
    }

    public void ButtonHeld(){
        held = true;
        fingers += 1;
    }
    public void ButtonReleased(){
        fingers -= 1;
        if(fingers == 0){
            held = false;
            StopCoroutine(med);
            doneCountdown = true;
        }
    }


    IEnumerator CountMeditation(){
        Color lastColor = darknessOverlay.GetComponent<Image>().color;
        lastColor.a = 0;
        LeanTween.alpha(darknessOverlay.GetComponent<Image>().rectTransform, 1, 5);
        if(!held){
            meditating = false;
            yield break;
        }
        while(meditating){
            yield return new WaitForSeconds(1);
            if(held){
                meditationTime += 1;
                meditationCount.text = ConvertIntToTime(meditationTime);    
            }
        }
    }

    public void ContinueMeditation(){
        if(meditationTime > prevMeditation){
            sessionLongest.text = ConvertIntToTime(meditationTime);
        }
        prevMeditation = meditationTime;
        meditationTime = 0;
        finishedPopUp.SetActive(false);
    }

    public void SaveAndExit(){
        dataManager.player.lastDateMeditated = (int)System.DateTimeOffset.Now.ToUnixTimeSeconds();
        if(meditationTime > dataManager.player.longestMeditation){
            dataManager.player.longestMeditation = meditationTime;
        }
        dataManager.SaveAll();
        finishedPopUp.SetActive(false);
        mainManager.GetComponent<MainManager>().LoadMainScreen();
        meditationTime = 0;
        prevMeditation =  0;
    }
    private void ResetOverlayColor(){
        LeanTween.cancel(darknessOverlay.GetComponent<Image>().rectTransform);
        LeanTween.alpha(darknessOverlay.GetComponent<Image>().rectTransform, 0, 1);
    }
    public string ConvertIntToTime(int timeToConvert){
        var time = System.TimeSpan.FromSeconds(timeToConvert);
        return time.ToString(@"hh\:mm\:ss");
    }

    public string ConvertEpochToDate(int timeToConvert){
        var datetime =  System.DateTimeOffset.FromUnixTimeSeconds(timeToConvert);
        Debug.Log(datetime.ToString("MM/dd/yyyy"));
        return datetime.ToString("MM/dd/yyyy");
    }
}

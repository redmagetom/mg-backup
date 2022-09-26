using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public DataManager dataManager;
    public CameraManager cameraManager;

    [Header("Screens")]
    public GameObject memoryGamePanel;
    public GameObject mainPagePanel;
    public GameObject newGardenPanel;
    public GameObject newPlayerName;
    public GameObject newGardenName;
    public GameObject windGamePanel;
    public GameObject clickerGamePanel;
    public GameObject reactionGamePanel;
    public GameObject meditationPanel;
    public Text coinsText;
    public Text gardenNameText;
    [Header("Start Screen")]
    public GameObject startScreen;
    public GameObject tapToStart;
    [Header("Overlay")]
    public GameObject overlayScreen;
    public Text overlayQuote;
    public Text overlayAttribution;
    [Header("Audio")]
    public GameObject soundPlayer;
    public AudioClip tapChime1;
    public AudioClip confirmChime1;
    public AudioClip confirmChime2;
    public AudioClip levelSelectChime1;
    [Header("Main Screen")]
    public GameObject gameSelectionPanel;

    void Start(){
        startScreen.SetActive(true);
        tapToStart.GetComponent<Animation>().Play("startPulse");
        cameraManager.mainCam.gameObject.SetActive(true);
        Application.targetFrameRate = 30;
        // Startup();
        cameraManager.gardenCam.gameObject.SetActive(false);
        cameraManager.gardenHolder.SetActive(false);
    }

    public void Startup(){
        memoryGamePanel.SetActive(false);
        try{
            dataManager.LoadGame();
            Debug.Log(dataManager.player.playerName);
            newGardenPanel.SetActive(false);
            soundPlayer.GetComponent<AudioSource>().PlayOneShot(tapChime1);
            startScreen.GetComponent<Image>().raycastTarget = false;
            LoadMainScreen();
            
        } catch (Exception e){
            Debug.Log("Error: " + e);
            newGardenPanel.SetActive(true);
            mainPagePanel.SetActive(false);
            startScreen.SetActive(false);
        }
    }

    public void ShowOverlay(){
        StartCoroutine(I_ShowOverlay());
    }
    IEnumerator I_ShowOverlay(){
        Animation anims = overlayScreen.GetComponent<Animation>();
        anims.PlayQueued("overlayFadeIn");
        ChangeOverlayQuote();
        yield return StartCoroutine(WaitForAnimation(anims));
        // anims.Play("overlayFadeOut");
    }

    public void ChangeOverlayQuote(){
        Debug.Log("Change Quote");
    }
    public void CreateNewGarden(){
        dataManager.CreateNew(newPlayerName.GetComponent<InputField>().text, newGardenName.GetComponent<InputField>().text);
        dataManager.LoadGame();
        LoadMainScreen();
    }

  
    // LOADING SCREEN STUFF
    public void LoadMainScreen(){
        StartCoroutine(I_LoadMainScreen());
    }

    IEnumerator I_LoadMainScreen(){
        yield return StartCoroutine(I_ShowOverlay());
        mainPagePanel.SetActive(true);
        startScreen.SetActive(false);
        newGardenPanel.SetActive(false);
        // cameraManager.mainCam.transform.localPosition = new Vector3(0,0,-835);
        gardenNameText.text = dataManager.player.gardenName;
        memoryGamePanel.SetActive(false);
        windGamePanel.SetActive(false);
        clickerGamePanel.SetActive(false);
        reactionGamePanel.SetActive(false);
        meditationPanel.SetActive(false);
        coinsText.text = dataManager.player.coins.ToString();
        overlayScreen.GetComponent<Animation>().Play("overlayFadeOut");
    }

    public void GoToGarden(){
        StartCoroutine(I_GoToGarden());
    }

    public void ShowGames(){
        gameSelectionPanel.GetComponent<Animation>().Play("showGames");
        soundPlayer.GetComponent<AudioSource>().PlayOneShot(confirmChime2);
    }

    public void HideGames(){
        gameSelectionPanel.GetComponent<Animation>().Play("hideGames");
        soundPlayer.GetComponent<AudioSource>().PlayOneShot(confirmChime2);
    }
    IEnumerator I_GoToGarden(){
        soundPlayer.GetComponent<AudioSource>().PlayOneShot(confirmChime1);
        yield return StartCoroutine(I_ShowOverlay());
        cameraManager.SwitchCamera();
    }
    public void InitiateMeditation(){
        StartCoroutine(StartMeditation());
    }
    public void InitiateMemoryGame(){
        StartCoroutine(StartMemoryGame());
    }

    public void InitiateWindGame(){
        windGamePanel.SetActive(true);
        StartCoroutine(StartWindGame());
    }

    public void IntitiateClickerGame(){
        mainPagePanel.SetActive(false);
        clickerGamePanel.SetActive(true);
        StartCoroutine(StartClickerGame());
    }

    public void InititateReactionGame(){
        mainPagePanel.SetActive(false);
        reactionGamePanel.SetActive(true);
        StartCoroutine(StartReactionGame());
    }

    IEnumerator StartReactionGame(){
        // cameraManager.mainCam.transform.localPosition = new Vector3(0,0,-1340);
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<ReactionChallenge>().Setup();
    }
    IEnumerator StartClickerGame(){
        // cameraManager.mainCam.transform.localPosition = new Vector3(0,0,-1340);
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<ClickerGame>().Setup();
    }
    IEnumerator StartMeditation(){
        soundPlayer.GetComponent<AudioSource>().PlayOneShot(levelSelectChime1);
        yield return StartCoroutine(I_ShowOverlay());
        mainPagePanel.SetActive(false);
        meditationPanel.SetActive(true);
        gameSelectionPanel.GetComponent<Animation>().Play("hideGames");
        // cameraManager.mainCam.transform.localPosition = new Vector3(0,0,-1340);
        overlayScreen.GetComponent<Animation>().Play("overlayFadeOut");
    }
    IEnumerator StartWindGame(){
        soundPlayer.GetComponent<AudioSource>().PlayOneShot(levelSelectChime1);
        yield return StartCoroutine(I_ShowOverlay());
        mainPagePanel.SetActive(false);
        windGamePanel.SetActive(true);
        gameObject.GetComponent<WindHoopChallenge>().tapToStart.GetComponent<Animation>().Play("startPulse");
        gameSelectionPanel.GetComponent<Animation>().Play("hideGames");
        // cameraManager.mainCam.transform.localPosition = new Vector3(0,0,-1340);
        gameObject.GetComponent<WindHoopChallenge>().Setup();
        overlayScreen.GetComponent<Animation>().Play("overlayFadeOut");
    }
    IEnumerator StartMemoryGame(){
        soundPlayer.GetComponent<AudioSource>().PlayOneShot(levelSelectChime1);
        yield return StartCoroutine(I_ShowOverlay());
        mainPagePanel.SetActive(false);
        memoryGamePanel.SetActive(true);
        gameSelectionPanel.GetComponent<Animation>().Play("hideGames");
        // cameraManager.mainCam.transform.localPosition = new Vector3(0,0,-1340);
        gameObject.GetComponent<ButtonChallenge>().SetUp();
        overlayScreen.GetComponent<Animation>().Play("overlayFadeOut");
    }
    private IEnumerator WaitForAnimation ( Animation animation ){
        do { yield return null; } while ( animation.isPlaying );
    }

    // PLAYER/INVENTORY MANAGEMENT



}

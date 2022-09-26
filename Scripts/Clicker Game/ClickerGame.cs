using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickerGame : MonoBehaviour
{
    public GameObject clickerButton;
    public Text coinsText;
    public int perClickIncrease;
    private DataManager dataManager;

    public void Setup(){
        dataManager = gameObject.GetComponent<MainManager>().dataManager;
        coinsText.text = ("Coins: " + dataManager.player.coins);
    }
    public void ClickedButton(){
        dataManager.player.coins += perClickIncrease;
        coinsText.text = ("Coins: " + dataManager.player.coins);
    }
}

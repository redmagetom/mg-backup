using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

public class DataManager : MonoBehaviour
{
    public PlayerData player;
    public ItemVault itemVault;
    public bool playerLoaded;
    public Island initialIsland;
    public void CreateNew(string player, string garden){
        PlayerData newPlayer = new PlayerData();
        newPlayer.playerName = player;
        newPlayer.gardenName = garden;
        newPlayer.coins = 10000;
        newPlayer.inventorySize = 40;
        newPlayer.lastShopCheck = 0;
        newPlayer.storedShopItems = new List<int>();
        newPlayer.inventory = new List<int>();
        for(var i = 0; i < newPlayer.inventorySize; i ++){
            newPlayer.inventory.Add(0);
        }
        newPlayer.currentIsland = initialIsland;
        newPlayer.islands = new List<Island>();

        newPlayer.lastDateMeditated = 0;
        newPlayer.longestMeditation = 0;

        newPlayer.moodRecord = new Dictionary<string, int[]>();
        newPlayer.lastMoodInput = 0;
        newPlayer.moodRecordStreak = 0;
        newPlayer.dailyMorningDone = false;
        newPlayer.dailyEveningDone = false;
        
        SaveGame.Save<PlayerData>("playerData", newPlayer);
    }

    public void SaveAll(){
        SaveGame.Save<PlayerData>("playerData", player);
    }

    public void LoadGame(){
        player = SaveGame.Load<PlayerData>("playerData");
        CleanInventory();
        playerLoaded = true;
    }


    public void CleanInventory(){
        for(var i = 0; i < player.inventorySize; i++){
            if(player.inventory[i] <= 10){
                player.inventory[i] = 0;
            }
        }
        SaveAll();
    }

    public int GetEpochTime(){
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Local);
        return (int)(System.DateTime.Now - epochStart).TotalSeconds;
    }
}

public class PlayerData : MonoBehaviour
{
    public string playerName;
    public string gardenName;
    public int coins;
    public int inventorySize;
    public int lastShopCheck;
    public List<int> inventory;
    public List<int> storedShopItems;
    public Island currentIsland;
    public List<Island> islands;

    // Game and Other STuff
    public int lastDateMeditated;
    public int longestMeditation;

    // mood stuff
    // REDO RECORD TO BE STRING FOR DATE AND ARRAY OF 2 FOR MORNING EVENING
    public Dictionary<string, int[]> moodRecord;
    public int lastMoodInput;
    public int moodRecordStreak;
    public bool dailyMorningDone;
    public bool dailyEveningDone;
}



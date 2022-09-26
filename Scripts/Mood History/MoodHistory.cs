using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodHistory : MonoBehaviour
{
    [Header("Recording Stuff")]
    public DataManager dataManager;
    public List<Image> moodImages;
    public Text streakReadout;
    public GameObject moodselector;
    public GameObject recordMoodButton;
    public Text recordMoodText;
    public Text startOfDayMood;
    public Text endOfDayMood;
    public GameObject confirmMoodPopUp;
    public Text confirmedMoodAmount;
    private int moodToRecord;
    [Header("History Stuff")]
    public GameObject blankBuffer;
    public Text header;
    public GameObject dateSection;
    public GameObject monthHistoryPanel;
    public GameObject monthHistoryContainer;
    public System.DateTime viewingDate;
    private GameObject lastSection;

    public void _EventualStart_(){
        SetupMoodRecording();
    }

    public void SetupMoodRecording(){
        recordMoodButton.SetActive(true);
        moodselector.SetActive(false);

        int[] currentRecord = new int[2];

        if(!dataManager.player.moodRecord.ContainsKey(System.DateTime.Now.ToString("MM/dd/yyyy"))){
            dataManager.player.moodRecord.Add(System.DateTime.Now.ToString("MM/dd/yyyy").ToString(), new int[2]);
            dataManager.player.moodRecord[System.DateTime.Now.ToString("MM/dd/yyyy")].SetValue(0, 0);
            dataManager.player.moodRecord[System.DateTime.Now.ToString("MM/dd/yyyy")].SetValue(0, 1);
            currentRecord = dataManager.player.moodRecord[System.DateTime.Now.ToString("MM/dd/yyyy")];
        } 

        currentRecord = dataManager.player.moodRecord[System.DateTime.Now.ToString("MM/dd/yyyy")];

        startOfDayMood.text = currentRecord[0].ToString();
        endOfDayMood.text = currentRecord[1].ToString();
        if(currentRecord[0] != 0 && currentRecord[1] != 0){
            recordMoodText.text = "mood recorded for day. \n Good Job!";
            recordMoodButton.GetComponent<Button>().interactable = false;
            return;
        }

        if(GetEpochTime() - dataManager.player.lastMoodInput < 10){
            recordMoodButton.GetComponent<Button>().interactable = false;
            recordMoodText.text = "Too soon to record other record";
            return;
        } else {
            recordMoodButton.GetComponent<Button>().interactable = true;
        }

        if(currentRecord[0] == 0){
            recordMoodText.text = "input first record";
        }  else {
            recordMoodText.text = "input second record";
        }
    }

    #region  Mood Levels
    public void Record1(){
        moodToRecord = 1;
        ShowMoodConfirmation();
    }

    public void Record2(){
        moodToRecord = 2;
        ShowMoodConfirmation();
    }
    
    public void Record3(){
        moodToRecord = 3;
        ShowMoodConfirmation();
    }
    
    public void Record4(){
        moodToRecord = 4;
        ShowMoodConfirmation();
    }
    
    public void Record5(){
        moodToRecord = 5;
        ShowMoodConfirmation();
    }
    
    public void Record6(){
        moodToRecord = 6;
        ShowMoodConfirmation();
    }
    
    public void Record7(){
        moodToRecord = 7;
        ShowMoodConfirmation();
    }
    
    public void Record8(){
        moodToRecord = 8;
        ShowMoodConfirmation();
    }
    
    public void Record9(){
        moodToRecord = 9;
        ShowMoodConfirmation();
    }
    
    public void Record10(){
        moodToRecord = 10;
        ShowMoodConfirmation();
    }

    #endregion

    public void ShowMoodPanel(){
        recordMoodButton.SetActive(false);
        moodselector.SetActive(true);
    }
    public void ShowMoodConfirmation(){
        confirmMoodPopUp.SetActive(true);
        confirmedMoodAmount.text = moodToRecord.ToString();
    }

    public void CloseMoodConfirmation(){
        confirmMoodPopUp.SetActive(false);
    }
    public void ConfirmedMood(){
        RecordMood(moodToRecord);
        confirmMoodPopUp.SetActive(false);
    }

    public void RecordMood(int moodLevel){
        int[] currentRecord = dataManager.player.moodRecord[System.DateTime.Now.ToString("MM/dd/yyyy")];
        if(currentRecord[0] == 0){
            currentRecord[0] = moodLevel;
        } else {
            currentRecord[1] = moodLevel;
        }
        dataManager.player.lastMoodInput = GetEpochTime();
        dataManager.SaveAll();
        SetupMoodRecording();
    }

    #region History Stuff

    public void ShowHistoryPanel(){
        monthHistoryPanel.SetActive(true);
        viewingDate = System.DateTime.Now;
        LoadHistory();
    }
    public void HideHistoryPanel(){
        monthHistoryPanel.SetActive(false);
    }

    public void NextMonth(){
        viewingDate = viewingDate.AddMonths(1);
        LoadHistory();
    }

    public void PreviousMonth(){
        viewingDate = viewingDate.AddMonths(-1);
        LoadHistory();
    }

    public void LoadHistory(){
        foreach(Transform item in monthHistoryContainer.transform){
            Destroy(item.gameObject);
        }
        
        var currentMonth = viewingDate.Month;
        header.text = viewingDate.ToString("MMMM yyyy");
        int daysInMonth = System.DateTime.DaysInMonth(viewingDate.Year, viewingDate.Month);

        var firstDayOfMonth = new System.DateTime(viewingDate.Year, viewingDate.Month, 1);
        GenerateBlanks(firstDayOfMonth.DayOfWeek);

        for(var i = 1; i < daysInMonth+1; i++){
            var dayString = viewingDate.Month.ToString("00")+"/"+i.ToString("00")+"/"+viewingDate.Year.ToString();
            var day = System.DateTime.Parse(dayString);
            var t = day.DayOfWeek;
            var newDateReadout = Instantiate(dateSection);
            newDateReadout.transform.SetParent(monthHistoryContainer.transform, worldPositionStays: false);
            DateSectionInfo info = newDateReadout.GetComponent<DateSectionInfo>();
            info.dayOfTheWeek.text = day.DayOfWeek.ToString();
            info.date.text = System.DateTime.Parse(day.Date.ToString()).ToString("MM/dd/yyyy");

            
            foreach(var item in dataManager.player.moodRecord){
                if(item.Key == dayString){
                    if(item.Value[0] >= 5){
                            info.firstDailyImage.GetComponent<Image>().color = Color.green;
                        } else {
                            info.firstDailyImage.GetComponent<Image>().color = Color.red;
                        }

                        if(item.Value[1] >= 5){
                            info.secondDailyImage.GetComponent<Image>().color = Color.green;
                        } else {
                            info.secondDailyImage.GetComponent<Image>().color = Color.red;
                        }
                }
            }
        }
    }

    private void GenerateBlanks(System.DayOfWeek dayOfWeek){
        int neededBlanks = 0;
        if(dayOfWeek == System.DayOfWeek.Monday){
            neededBlanks = 1;
        } else if (dayOfWeek == System.DayOfWeek.Tuesday){
            neededBlanks = 2;
        } else if (dayOfWeek == System.DayOfWeek.Wednesday){
            neededBlanks = 3;
        } else if (dayOfWeek == System.DayOfWeek.Thursday){
            neededBlanks = 4;
        } else if (dayOfWeek == System.DayOfWeek.Friday){
            neededBlanks = 5;
        } else if (dayOfWeek == System.DayOfWeek.Saturday){
            neededBlanks = 6;
        }
        for(var i = 0; i < neededBlanks; i++){
            var blank = Instantiate(blankBuffer);
            blank.transform.SetParent(monthHistoryContainer.transform, worldPositionStays: false);
        }
    }
    #endregion
   

   public void GenerationTest(){
       var start = new System.DateTime(2021, 01, 01);
       for(var i = 0; i < 5; i++){
           for(var j = 1; j < 13; j++){
                for(var k = 1; k < System.DateTime.DaysInMonth(start.Year, j); k++){
                    dataManager.player.moodRecord.Add(start.Date.ToString("MM/dd/yyyy"), new int[2]);
                    dataManager.player.moodRecord[start.Date.ToString("MM/dd/yyyy")].SetValue(Random.Range(1,11), 0);
                    dataManager.player.moodRecord[start.Date.ToString("MM/dd/yyyy")].SetValue(Random.Range(1,11), 1);
                    start = start.AddDays(1);
                }
           }

        dataManager.SaveAll();
       }
   }
    public int GetEpochTime(){
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Local);
        return (int)(System.DateTime.Now - epochStart).TotalSeconds;
    }

}

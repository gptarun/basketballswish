using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OptionMenuScript : MonoBehaviour {
    //All option menu UI
    //number of quaters in game
    [SerializeField] TextMeshProUGUI noQuater;
    //Difficulty level of Game Easy or Hard
    [SerializeField] TextMeshProUGUI difficulty;
    List<string> diffLevel = new List<string>(new string[] {"EASY","HARD"});
    //Make sound effects ON/OFF
    [SerializeField] Toggle sound;
    //Set team size - 1, 2 or 3
    [SerializeField] TextMeshProUGUI teamSize;    
    //Set quater time 30, 60, 90
    [SerializeField] TextMeshProUGUI quaterTime;
    //Switch side after Number of quaters/2
    [SerializeField] Toggle switchSide;
    //Make game music ON/OFF
    [SerializeField] Toggle music;
    //All static variables
    public static int quaterCounter = 4;
    public static int teamSizeCounter = 3;              //add this to file
    public static string difficultyLevel = "EASY";      //add this to file
    public static bool isSound = true;
    public static bool isMusic = true;
    public static int[] quaterDuration = { 60, 90, 30 };
    public static int quaterTimerCounter = 0;           //add this to file
    // Use this for initialization
    void Start () {
        noQuater.text = quaterCounter.ToString();
        difficulty.text = diffLevel[0];
        teamSize.text = teamSizeCounter.ToString();
        quaterTime.text = quaterDuration[quaterTimerCounter].ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void onToggleSound()
    {
        FindObjectOfType<AudioSource>().mute = !sound.isOn;        
    }

    public void ChangeQuaterNumber()
    {
        quaterCounter--;
        if(quaterCounter == 0)
        {
            quaterCounter = 4;
        }
        noQuater.text = quaterCounter.ToString();
    }

    public void ChangeDifficulty()
    {
        if (difficulty.text.Equals("EASY")){
            difficulty.text = diffLevel[1];
            difficultyLevel = diffLevel[1];
        }
        else
        {
            difficulty.text = diffLevel[0];
            difficultyLevel = diffLevel[0];
        }
    }

    public void ChangeTeamSize()
    {
        teamSizeCounter--;
        if (teamSizeCounter == 0)
        {
            teamSizeCounter = 3;
        }
        teamSize.text = teamSizeCounter.ToString();
    }

    public void ChangeQuaterTime()
    {
        quaterTimerCounter++;
        if (quaterTimerCounter == 3)
        {
            quaterTimerCounter = 0;
        }
        quaterTime.text = quaterDuration[quaterTimerCounter].ToString();
    }
}

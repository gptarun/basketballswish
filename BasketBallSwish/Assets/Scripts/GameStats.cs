using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;


public class GameStats : MonoBehaviour {
    [SerializeField] TextMeshProUGUI noGame;
    [SerializeField] TextMeshProUGUI maxScore;
    [SerializeField] TextMeshProUGUI maxSteal;
    public static int gameCountOld = 0;   
    public static int maxPoints = 0;
    private static int maxPointsOld = 0;
    public static int steals = 0;
    public static int stealsOld = 0;
    private const string STAT_SEP = "#STATS#";
    public static bool matchEnded;
    private string filePath;
    private string fileData;
    // Use this for initialization
    void Start () {

        filePath = Application.persistentDataPath + "/option.txt";
        Debug.Log(filePath);
        if (!File.Exists(filePath))     // Checking if file is not exists then putting some value in it.
        {
            string[] saveGameStats = new string[]{
            ""+gameCountOld,
            ""+maxPointsOld,
            ""+stealsOld
            };
            string saveStats = string.Join(STAT_SEP, saveGameStats);
            File.WriteAllText(filePath, saveStats);            
        }        
        LoadData();
    }
	
	// Update is called once per frame
	void Update () {
        if (matchEnded)
        {
            SaveData();
            matchEnded = false;
        }
	}

    public void LoadData()
    {
        string getDataFromFile = File.ReadAllText(filePath);
        string[] gameContent = getDataFromFile.Split(new[] { STAT_SEP }, System.StringSplitOptions.None);

        gameCountOld = int.Parse(gameContent[0]);
        maxPointsOld = int.Parse(gameContent[1]);
        stealsOld = int.Parse(gameContent[2]);
        noGame.text = gameCountOld.ToString();
        maxScore.text = maxPointsOld.ToString();
        maxSteal.text = stealsOld.ToString();
    }

    public void SaveData()
    {
        if(maxPointsOld > maxPoints)
        {
            maxPoints = maxPointsOld;
        }
        if(stealsOld > steals)
        {
            steals = stealsOld;
        }

        gameCountOld++;
        
        string[] saveGameStats = new string[]{
            ""+gameCountOld,
            ""+maxPoints,
            ""+steals
        };

        string saveStats = string.Join(STAT_SEP, saveGameStats);
        File.WriteAllText(filePath, saveStats);
    }
   
}

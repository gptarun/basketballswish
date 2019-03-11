using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class SelectTeam : MonoBehaviour {

    private List<string> teamList = new List<string>(new string[] { "Africa", "Argentina", "Australia", "Brazil", "China", "France", "India", "Mexico", "Philippines", "Russia", "Serbia", "Singapore", "Spain", "Thailand", "USA", "Yugoslavia"});
    private readonly List<string> teamListShort = new List<string>(new string[] { "Afr", "Arg", "Aus", "Bra", "Chi", "Fra", "Ind", "Mex", "Phi", "Rus", "Ser", "Sin", "Spa", "Tha", "Usa", "Yug" });
    private readonly List<int> teamRating = new List<int>(new int[] { 2,3,2,3,2,3,2,1,2,3,2,2,3,2,3,3 });
    private readonly List<long> teamCost = new List<long>(new long[] { 2000, 5000, 2000, 4500, 2000, 5500, 2000, 1000, 2000, 4500, 2000, 2000, 5500, 2000, 6000, 4000 });
    //private readonly List<bool> teamLocks = new List<bool>(new bool[] { false, true, false, true, false, true, false, false, false, true, false, false, true, false, true, true});
    //Choosing Teams
    [SerializeField] public TextMeshProUGUI teamAChoice;
    [SerializeField] public TextMeshProUGUI teamBChoice;
    //Choosing Human or AI mode
    [SerializeField] Button changeModeA;
    private string modeA;
    [SerializeField] Button changeModeB;
    private string modeB;
    private int modeCounterA = 0;
    private int modeCounterB = 1;
    //Chaning sprite AI to Human and vice-versa
    public Sprite[] mode;
    private int indexA = 8;
    private int indexB = 10;
    //Calculating stars
    public GameObject[] teamAStar;
    public GameObject[] teamBStar;
    //Loading flags
    public Sprite[] flags;
    [SerializeField] public Button teamAFlag;
    [SerializeField] public Button teamBFlag;
    [SerializeField] TournamentScript tournament;
    //Storing Team Data into Dictionary
    //Dictionary<int, TeamScript> teamData = new Dictionary<int, TeamScript>();
    string currButtonName;
    [SerializeField] SinglePlayerController singlePlayer;
    public GameObject loadingScreen;
    public Slider slider;
    private Dictionary<string, TeamStatus> teamDict;
    public Image lockA;
    public Image lockB;
    public Button playButton;
    public TeamDataController teamDataController;

    void Start () {
        teamAChoice.text = teamList[indexA].ToString();
        teamBChoice.text = teamList[indexB].ToString();
        changeModeA.image.sprite = mode[modeCounterA];
        changeModeB.image.sprite = mode[modeCounterB];
        flags = Resources.LoadAll<Sprite>("Flags");
        modeA = "human";
        modeB = "bot";
        teamDict = new Dictionary<string, TeamStatus>();
        teamDataController = new TeamDataController();
        loadFileData();
        //teamDataController.EditTeamData(new TeamStatus("Africa","Afr", true,3)); //For testing the data is loading from JSON file in updation at runtime
    }
	
	// Update is called once per frame
	void Update () {
        //For the updation of data from JSON file at run time if there are any changes in the data file
        if (teamDataController.uncheckedData)
        {
            teamDataController.LoadGameData(); // loading the data from file
            teamDict.Clear();
            for (int i = 0; i < teamDataController.teamData.Length; i++)
            {
                teamDict.Add(teamDataController.teamData[i].TeamName, teamDataController.teamData[i]); // setting the data in the dictionary which was fetched from file
            }
        }
	}

    public void RighButtonA()
    {
        indexA++;
        if (indexA >= teamList.Count)
        {
            indexA = 0;
        }
        teamAChoice.text = teamList[indexA].ToString();
        RankCalculationDict(teamDict[teamList[indexA]].TeamRating, teamAStar);
        SetFlagoFTeam(indexA, teamAFlag);
        LockPlayButton(teamDict[teamList[indexA]].LockedStatus, teamDict[teamList[indexB]].LockedStatus);
        if (teamDict[teamList[indexA]].LockedStatus == true)          //Locking the teams  
        {
            lockA.enabled = true;
        }
        else
        {
            lockA.enabled = false;
        }
    }

    public void LeftButtonA()
    {
        indexA--;
        if (indexA < 0)
        {
            indexA = teamList.Count - 1;
        }
        teamAChoice.text = teamList[indexA].ToString();
        RankCalculationDict(teamDict[teamList[indexA]].TeamRating, teamAStar);
        SetFlagoFTeam(indexA, teamAFlag);
        LockPlayButton(teamDict[teamList[indexA]].LockedStatus, teamDict[teamList[indexB]].LockedStatus);
        if (teamDict[teamList[indexA]].LockedStatus==true)          //Locking the teams
        {
            lockA.enabled = true;
        }
        else
        {
            lockA.enabled = false;
        }
    }

    public void RighButtonB()
    {
        indexB++;
        if (indexB >= teamList.Count)
        {
            indexB = 0;
        }
        teamBChoice.text = teamList[indexB].ToString();
        RankCalculationDict(teamDict[teamList[indexB]].TeamRating, teamBStar);
        SetFlagoFTeam(indexB, teamBFlag);
        LockPlayButton(teamDict[teamList[indexA]].LockedStatus, teamDict[teamList[indexB]].LockedStatus);
        if (teamDict[teamList[indexB]].LockedStatus == true)            //Locking the teams
        {
            lockB.enabled = true;
        }
        else
        {
            lockB.enabled = false;
        }
    }
    public void LeftButtonB()
    {
        indexB--;
        if (indexB < 0)
        {
            indexB = teamList.Count - 1;
        }
        teamBChoice.text = teamList[indexB].ToString();
        RankCalculationDict(teamDict[teamList[indexB]].TeamRating, teamBStar);
        SetFlagoFTeam(indexB, teamBFlag);
        LockPlayButton(teamDict[teamList[indexA]].LockedStatus, teamDict[teamList[indexB]].LockedStatus);
        if (teamDict[teamList[indexB]].LockedStatus == true)        //Locking the teams
        {
            lockB.enabled = true;
        }
        else
        {
            lockB.enabled = false;
        }
    }
    public void SelectModeOnA()
    {
        modeCounterA++;
        if (modeCounterA % 2 == 0)
        {
            changeModeA.image.sprite = mode[0];
            modeA = "human";
}
        else
        {
            changeModeA.image.sprite = mode[1];
            modeA = "bot";
        }
    }
    public void SelectModeOnB()
    {
        modeCounterB++;
        if (modeCounterB % 2 == 0)
        {
            changeModeB.image.sprite = mode[0];
            modeB = "human";
        }
        else
        {
            changeModeB.image.sprite = mode[1];
            modeB = "bot";
        }
    }

    public void RankCalculation(int value, GameObject[] gameObj)
    {
        ResetRank(gameObj);
        for (int i = 0; i < teamRating[value]; i++)
        {
            gameObj[i].SetActive(true);
        }        
    }

    public void RankCalculationDict(int value, GameObject[] gameObj)
    {
        ResetRank(gameObj);
        for (int i = 0; i < value; i++)
        {
            gameObj[i].SetActive(true);
        }
    }

    public void ResetRank(GameObject[] gameObj)
    {
        for (int i = 0; i < 3; i++)
        {
            gameObj[i].SetActive(false);
        }
    }
    public void SetFlagoFTeam(int flagCount,Button flagButton)
    {
        flagButton.image.sprite = flags[flagCount];
    }

    public void SetTeamForTournament()
    {
        //tournament.SetTeam(teamList[indexA].ToString(), teamList[indexB].ToString(),modeA,modeB,teamAFlag.image.sprite,teamBFlag.image.sprite, currButtonName);
        Image imageTeamA = SetFlagSpriteoFTeam(currButtonName, "TeamA", teamAFlag.image.sprite);
        Image imageTeamB = SetFlagSpriteoFTeam(currButtonName, "TeamB", teamBFlag.image.sprite);
        TeamScript teamA = new TeamScript(teamList[indexA].ToString(), teamListShort[indexA].ToString(), modeA, imageTeamA);
        TeamScript teamB = new TeamScript(teamList[indexB].ToString(), teamListShort[indexB].ToString(), modeB, imageTeamB);
        tournament.SetTeam(new MatchDay(teamA, teamB, currButtonName));
    }

    public void SetTeamForSingleMatch()
    {
        singlePlayer.SetTeams(teamList[indexA].ToString(), teamList[indexB].ToString(), modeA, modeB);
    }

    public void buttonClickedName(string buttonName)
    {
        currButtonName = buttonName;
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneAsynchronously("MainMenuScene"));
    }

    public Image SetFlagSpriteoFTeam(string buttonName, string gameObjectName, Sprite sprite)
    {
        Image imageTeam = GameObject.Find(buttonName).transform.Find(gameObjectName).GetComponent<Image>();
        imageTeam.overrideSprite = sprite;
        imageTeam.sprite = sprite;
        return imageTeam;
    }

    IEnumerator LoadSceneAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress);
            slider.value = progress * 30;
            yield return null;
        }
    }

    private void LockPlayButton(bool lockedStatusA, bool lockedStatusB)
    {
        Debug.Log("A:" + lockedStatusA + " B:" + lockedStatusB);
        if (!lockedStatusA && !lockedStatusB)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }

    public void loadFileData()
    {
        if (File.Exists(Application.persistentDataPath + "/teamData.json"))
        {
            teamDataController.LoadGameData(); // loading the data from file
            for (int i = 0; i < teamDataController.teamData.Length; i++)
            {
                teamDict.Add(teamDataController.teamData[i].TeamName, teamDataController.teamData[i]); // setting the data in the dictionary which was fetched from file
            }
        }
        else
        {
            teamDataController.teamData = new TeamStatus[16];
            for (int i = 0; i < teamList.Capacity; i++)
            {
                if (teamRating[i] == 3)
                {
                    teamDict.Add(teamList[i], new TeamStatus(teamList[i], teamListShort[i], true, teamCost[i], teamRating[i]));
                    teamDataController.teamData[i] = new TeamStatus(teamList[i], teamListShort[i], true, teamCost[i], teamRating[i]);
                }
                else
                {
                    teamDict.Add(teamList[i], new TeamStatus(teamList[i], teamListShort[i], false, teamCost[i], teamRating[i]));
                    teamDataController.teamData[i] = new TeamStatus(teamList[i], teamListShort[i], false, teamCost[i], teamRating[i]);
                }
            }
            teamDataController.SaveGameData();
        }
    }

}

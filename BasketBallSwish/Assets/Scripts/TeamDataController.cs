using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamDataController : MonoBehaviour {
    
    public TeamStatus[] teamData;

    private string gameDataProjectFilePath = "/StreamingAssets/teamData.json";

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        LoadGameData();
    }

    public TeamStatus GetTeamData(string teamName)
    {
        foreach (TeamStatus team in teamData)
        {
            if (team.TeamName.Equals(teamName))
            {
                return team;
            }
        }
        return null;
    }

    public void EditTeamData(TeamStatus team)
    {
        for (int i=0; i < teamData.Length; i++)
        {
            if (teamData[i].TeamName.Equals(team.TeamName))
            {
                teamData[i] = team;
            }
        }
        SaveGameData();
    }

    //fetch the data from JSON file
    public void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            teamData = JSonHelper.FromJson<TeamStatus>(dataAsJson);
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    //save the data from JSON file
    public void SaveGameData()
    {
        string dataAsJson = JSonHelper.ToJson(teamData, true);

        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);
    }
}

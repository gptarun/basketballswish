using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamDataController : MonoBehaviour {

    private TeamStatus[] teamData;

    private string gameDataFileName = "teamData.json";

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
    }

    private void LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            // Retrieve the allRoundData property of loadedData
            teamData = loadedData.teamData;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }
    
}

﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserDataController : MonoBehaviour {

    public UserData userData;
    public bool uncheckedData;
    private string gameDataProjectFilePath = "/StreamingAssets/userData.json";

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        LoadGameData();
        uncheckedData = false;
    }

    public UserData UserData
    {
        get
        {
            return userData;
        }

        set
        {
            userData = value;
        }
    }

    //fetch the data from JSON file
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + gameDataProjectFilePath;
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            userData = JsonUtility.FromJson<UserData>(dataAsJson);
            uncheckedData = false;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    //save the data from JSON file
    public void SaveGameData()
    {
        string dataAsJson = JsonUtility.ToJson(userData, true);

        string filePath = Application.persistentDataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);
        uncheckedData = true;
        LoadGameData();
    }
}

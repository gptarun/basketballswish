﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GameDataEditor : EditorWindow {

    public GameData gameData;

    private string gameDataProjectFilePath = "/StreamingAssets/teamData.json";

    [MenuItem("Window/Game Data Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(GameDataEditor)).Show();
    }

    void OnGUI()
    {
        if (gameData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("gameData");
            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save data"))
            {
                SaveGameData();
            }
        }

        if (GUILayout.Button("Load data"))
        {
            LoadGameData();
        }
    }

    public GameData LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;
        gameData = new GameData();
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            gameData.teamData = JSonHelper.FromJson<TeamStatus>(dataAsJson);
        }
        return gameData;
    }

    public void SaveGameData()
    {
        string dataAsJson = JSonHelper.ToJson(gameData.teamData,true);

        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

    }
}

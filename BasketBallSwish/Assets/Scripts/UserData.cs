﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData {

    public string userName;
    public int age;
    public string gender;
    public long expPoints;
    public long baskyCoins;
    public bool tutorialTaken;

    public UserData()
    {
    }

    public UserData(string userName, int age, string gender, long expPoints, long baskyCoins, bool tutorialTaken)
    {
        this.userName = userName;
        this.age = age;
        this.gender = gender;
        this.expPoints  = expPoints;
        this.baskyCoins = baskyCoins;
        this.tutorialTaken = tutorialTaken;
    }
}

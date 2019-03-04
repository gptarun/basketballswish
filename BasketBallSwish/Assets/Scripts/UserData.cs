using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData {

    public string userName;
    public int age;
    public string gender;
    public long baskyCoins;

    public UserData(string userName, int age, string gender, long baskyCoins)
    {
        this.userName = userName;
        this.age = age;
        this.gender = gender;
        this.baskyCoins = baskyCoins;
    }
}

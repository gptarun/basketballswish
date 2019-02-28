using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TeamStatus {
    public string TeamName;
    public string ShortName;
    public bool LockedStatus;
    public int TeamRating;

    public TeamStatus(string teamName, string shortName, bool lockedStatus, int teamRating)
    {
        this.TeamName = teamName;
        this.ShortName = shortName;
        this.LockedStatus = lockedStatus;
        this.TeamRating = teamRating;
    }

}

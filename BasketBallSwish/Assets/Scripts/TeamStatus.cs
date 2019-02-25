using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamStatus : MonoBehaviour {
    private string teamName;
    private string shortName;
    private bool lockedStatus;
    private int teamRating;

    public TeamStatus(string teamName, string shortName, bool lockedStatus, int teamRating)
    {
        this.teamName = teamName;
        this.shortName = shortName;
        this.lockedStatus = lockedStatus;
        this.teamRating = teamRating;
    }

    public string ShortName
    {
        get
        {
            return shortName;
        }

        set
        {
            shortName = value;
        }
    }

    public string TeamName
    {
        get
        {
            return teamName;
        }

        set
        {
            teamName = value;
        }
    }

    public bool LockedStatus
    {
        get
        {
            return lockedStatus;
        }

        set
        {
            lockedStatus = value;
        }
    }

    public int TeamRating
    {
        get
        {
            return teamRating;
        }

        set
        {
            teamRating = value;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyTeam : MonoBehaviour {

    /// <summary>
    /// In this script we are goin to buy the team based on baskey coins
    /// </summary>
    public Button buyButton;        // Need to set interactable true/false based on amount
    public Button teamUp;
    public Button teamDown;	

    public void ConfirmedBuyTeam()
    {
        Debug.Log("Successfully bought this team");
        // Need to check amount then allow player to buy team    
    }

    public void GoUp()
    {
        Debug.Log("Up");
        //Going up
    }

    public void GoDown()
    {
        Debug.Log("Down");
        //Going down
    }
}

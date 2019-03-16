using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyTeam : MonoBehaviour {

    /// <summary>
    /// In this script we are goin to buy the team based on baskey coins
    /// </summary>
    public Button buyButton;        // Need to set interactable true/false based on amount
    public Button teamUp;
    public Button teamDown;
    public TeamDataController teamDataController;
    private Dictionary<string, TeamStatus> teamDict;
    public Image teamImage;
    public Sprite[] flags;
    private int teamCounter;
    private int index;
    [SerializeField] public TextMeshProUGUI teamNameText;
    [SerializeField] public TextMeshProUGUI teamCostText;
    [SerializeField] public TextMeshProUGUI baskyCoins;
    private UserDataController userDataController;
    public GameObject rewardedPanel;
    public GameObject noTeamToBuyPopup;

    void Start()
    {
        teamDict = new Dictionary<string, TeamStatus>();
        teamCounter = 0;
        index = 0;
        userDataController = new UserDataController();
        userDataController.LoadGameData();
        baskyCoins.SetText(userDataController.userData.baskyCoins.ToString());
        teamDataController = new TeamDataController();
        LoadBuyTeamData();
        if (teamDict==null || teamDict.Count == 0)
        {
            noTeamToBuyPopup.SetActive(true);
        }
        else
        {
            noTeamToBuyPopup.SetActive(false);
        }
        flags = Resources.LoadAll<Sprite>("Flags");        
    }
   

    public void ConfirmedBuyTeam()
    {
        Debug.Log("Successfully bought this team");
        // Need to check amount then allow player to buy team
        if (teamDict.ContainsKey(teamNameText.GetParsedText().ToString()))
        {
            userDataController.userData.baskyCoins -= teamDict[teamNameText.GetParsedText()].TeamCost;
            userDataController.SaveGameData();
            baskyCoins.SetText(userDataController.userData.baskyCoins.ToString());
            teamDict[teamNameText.GetParsedText()].LockedStatus = false;
            teamDataController.EditTeamData(teamDict[teamNameText.GetParsedText().ToString()]);
            LoadBuyTeamData();
            if (teamDict == null || teamDict.Count == 0)
            {
                noTeamToBuyPopup.SetActive(true);
            }
            else
            {
                noTeamToBuyPopup.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Team does not exist in dictionary");
        }
    }

    public void GoUp()
    {
        index--;
        Debug.Log("Up");
        //Going up
        if (index < 0)
        {
            index = teamCounter - 1;
        }
        SetTeamFlag(teamImage, teamDict.Keys.ElementAt(index));
        teamNameText.SetText(teamDict.Keys.ElementAt(index));
        teamCostText.SetText(teamDict.Values.ElementAt(index).TeamCost.ToString());
    }

    public void GoDown()
    {
        index++;
        Debug.Log("Down");
        //Going down
        if (index >= teamCounter)
        {
            index = 0;
        }
        SetTeamFlag(teamImage, teamDict.Keys.ElementAt(index));
        teamNameText.SetText(teamDict.Keys.ElementAt(index));
        teamCostText.SetText(teamDict.Values.ElementAt(index).TeamCost.ToString());
    }

    public void SetTeamFlag(Image image, string teamName)
    {
        for (int i=0;i < flags.Length; i++)
        {
            if (flags[i].name.ToLower().Equals(teamName.ToLower()))
            {
                image.sprite = flags[i];
                image.overrideSprite = flags[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (teamDict.ContainsKey(teamNameText.GetParsedText().ToString()))
        {
            if (userDataController.userData.baskyCoins < teamDict[teamNameText.GetParsedText().ToString()].TeamCost)
            {
                buyButton.enabled = false;
            }
            else
            {
                buyButton.enabled = true;
            }
        }

        if (AdManager.rewardedPanel)
        {
            rewardedPanel.SetActive(true);
        }
    }

    /*This method calls when user clicks on Shop button
    * IMP -> Enabled on Button clicks -->  TeamShop(Game obj) all scripts -> From Inspector
    * Called the Select Team script if TeamData.json doesn't exists.
    */

    public void LoadBuyTeamData()         
    {
        if(teamDataController == null)
        {
            teamDataController = new TeamDataController();
            teamDataController.LoadGameData();
        }
        teamDataController.LoadGameData(); // loading the data from file
        teamDict.Clear();
        teamCounter = 0;
        for (int i = 0; i < teamDataController.teamData.Length; i++)
        {
            if (teamDataController.teamData[i].LockedStatus)
            {
                teamDict.Add(teamDataController.teamData[i].TeamName, teamDataController.teamData[i]); // setting the data in the dictionary which was fetched from file
                teamCounter++;
            }
        }
        if (teamDataController.teamData!=null && teamDict.Count!=0)
        {
            SetTeamFlag(teamImage, teamDict.Keys.First());
            teamNameText.SetText(teamDict.Keys.First());
            teamCostText.SetText(teamDict.Values.First().TeamCost.ToString());
        }
    }

    public void UpdateBasketCoinsAfterPurchase()
    {
        userDataController.LoadGameData();
        baskyCoins.SetText((userDataController.userData.baskyCoins + 30).ToString());
        AdManager.rewardedPanel = false;
    }

}

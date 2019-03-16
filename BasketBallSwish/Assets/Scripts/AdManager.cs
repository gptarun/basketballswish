using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {


    // Use this for initialization
    InterstitialAd interstitial;
    // Update is called once per frame
    SinglePlayerController singlePlayer;
    TournamentController tournamentController;
    private RewardBasedVideoAd rewardedAd;
    private UserDataController userDataController;
    private string rewardedAdID = "ca-app-pub-3940256099942544/5224354917";        //Test id need to change in production
    //private string rewardedAdID = "ca-app-pub-3940256099942544/5224354917";        //give real rewarded id
    public static bool rewardedPanel;
    string adMessageType;

    private void Start()
    {
        //string adID = "ca-app-pub-7244224353357409/2886059541";   //User actual id
        string adID = "ca-app-pub-3940256099942544/1033173712";     //Test interstitial test id - Need to change in production        
        
#if UNITY_ANDROID
    string adUnitId = adID;
#elif UNITY_IOS
        string adUnitId = adID;
#else
        string adUnitId = adID;
#endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        RequestInterstitialAds();
        if (FindObjectOfType<SinglePlayerController>() != null)
        {
            singlePlayer = FindObjectOfType<SinglePlayerController>();
        }

        if (FindObjectOfType<TournamentController>() !=null )
        {
            tournamentController = FindObjectOfType<TournamentController>();
        }


        rewardedAd = RewardBasedVideoAd.Instance;

        RequestRewardedAd();


        rewardedAd.OnAdLoaded += HandleRewardBasedVideoLoaded;

        rewardedAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;

        rewardedAd.OnAdRewarded += HandleRewardBasedVideoRewarded;

        rewardedAd.OnAdClosed += HandleRewardBasedVideoClosed;

        userDataController = new UserDataController();
        userDataController.LoadGameData();
        rewardedPanel = false;
    }

    void Update () {
        if (singlePlayer != null)
        {
            if(singlePlayer.quaterCounter == 3)
            {
                showInterstitialAd();
            }
            if (singlePlayer.matchEnded)
            {
                showInterstitialAd();
            }
        }
        if (tournamentController != null)
        {
            if (tournamentController.quaterCounter == 3)
            {
                showInterstitialAd();
            }
            if (tournamentController.matchEnded)
            {
                showInterstitialAd();
            }
        }
    }

    public void showInterstitialAd()
    {
        //Show Ad
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }

    }
 
    private void RequestInterstitialAds()
    {

        AdRequest request = new AdRequest.Builder().Build();

        //Register Ad Close Event
        interstitial.OnAdClosed += Interstitial_OnAdClosed;

        // Load the interstitial with the request.
        interstitial.LoadAd(request);

    }

    //Ad Close Event
    private void Interstitial_OnAdClosed(object sender, System.EventArgs e)
    {
        //Resume Play Sound

    }

    public void RequestRewardedAd()
    {
        AdRequest request = new AdRequest.Builder().Build();

        rewardedAd.LoadAd(request, rewardedAdID);
    }

    public new void SendMessage(string messageType)
    {
        Debug.Log(messageType);
        adMessageType = messageType;
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("Rewarded ad not loaded");
        }
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("Rewarded Video ad loaded successfully");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Failed to load rewarded video ad : " + args.Message);
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("You have been rewarded with  " + amount.ToString() + " " + type);
        //call add coins method after watching video
        //will be reflected in the android device.
        if (adMessageType.Contains("twice"))
        {
            //need to handle the 2x coins
        }
        else
        {
            userDataController.userData.baskyCoins += 30;
            userDataController.SaveGameData();
            rewardedPanel = true;
        }
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("Rewarded video has closed");
        RequestRewardedAd();        
    }
}

using GoogleMobileAds.Api;
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
    private void Start()
    {
        string adID = "ca-app-pub-7244224353357409/2886059541";   //User actual id
        //string adID = "ca-app-pub-3940256099942544/1033173712";     //Test interstitial test id - Need to change in production
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
}

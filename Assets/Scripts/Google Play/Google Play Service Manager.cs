using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;


/// <summary>
/// This class holds all the logic for accessing the Google Play Services
/// </summary>
public static class GooglePlayServiceManager {

    public const string HISCORE_BOARD_ID = "CgkIhsicxpocEAIQAw";
    static public bool hasSignedIn { get; private set; }
    static public bool hasCachedHiScore { get; private set; }

    //This was added so that sign in attemps won't occur every time the player dies
    //Since the user might not be connected to the internet or want to log in
    static public bool hasTriedToSignIn { get; private set; }

    //Added to make sure activation is only called once
    static public bool hasBeenActivated;

    public static void signIn()
    {
        if (hasSignedIn) return;
        if(!hasBeenActivated)PlayGamesPlatform.Activate();
        hasBeenActivated = true;

        showSigningInToast();
        Social.Active.localUser.Authenticate((bool success) => {
            hasSignedIn = success;
            if (!success) showErrorToast();
            hasTriedToSignIn = true;
        });
        

    }


    public static void showHighScoreBoard()
    {
        if (hasSignedIn) Social.Active.ShowLeaderboardUI();
    }

    public static void postHighScore(int score)
    {
        if (!hasSignedIn) return;
        Social.Active.ReportScore(score, HISCORE_BOARD_ID, (bool success) => {
            //Handle successes and failures here      
        });
    }

    public static int retrieveHighScore()
    {
        int score = 0;
        if (!hasSignedIn) return score;

        //Score will stay 0 if this fails...
        PlayGamesPlatform.Instance.LoadScores(

           HISCORE_BOARD_ID,
           LeaderboardStart.PlayerCentered,
           1,
           LeaderboardCollection.Public,
           LeaderboardTimeSpan.AllTime,
           (LeaderboardScoreData data) => {
               if (data.PlayerScore != null)
               {
                   score = int.Parse(data.PlayerScore.formattedValue);
                   hasCachedHiScore = true;
               }     
        });
        return score;
    }

    static void showErrorToast()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        ToastMessageScript toast = new ToastMessageScript();
        toast.initialize();
        toast.showToastOnUiThread(LocalizationText.GetText("google sign-in error"), true);
#endif
    }


    static void showSigningInToast()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        ToastMessageScript toast = new ToastMessageScript();
        toast.initialize();
        toast.showToastOnUiThread(LocalizationText.GetText("signing in"), true);
#endif
    }



}

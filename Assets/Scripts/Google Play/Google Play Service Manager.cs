using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;



public static class GooglePlayServiceManager
{

    public const string HISCORE_BOARD_ID = "CgkIhsicxpocEAIQAw";
    static public bool hasSignedIn { get; private set; }


    static public bool hasCachedHiScore { get; private set; }
    static public bool isRetrievingHiScore { get; private set; } //Was added so that global doesn't try to retrieve the hiscore twice at the same time

    //This was added so that sign in attemps won't occur every time the player dies
    //Since the user might not be connected to the internet or not want to log in
    static public bool hasTriedToSignIn { get; private set; }

    //Added to make sure activation is only called once
    static public bool hasBeenActivated;

    //Had to do this because I hac C#4 and I couldn't assign values to auto-implemented properites
    private static int _cachedLeaderboardScore = 0;
    public static int cachedLeaderboardScore
    {
        get { return _cachedLeaderboardScore; }
        private set { _cachedLeaderboardScore = value; }
    }


    public static void signIn()
    {
        if (hasSignedIn) return;
        if (!hasBeenActivated) PlayGamesPlatform.Activate();
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

    /*
     *This method always gives the score to the the variable GooglePlayServiceManager.leaderboardScore
     *Because the process takes a while, I think the function ends before the Leaderboard callback can come to update score.
     *So it would always return 0. This was done to correct that
     */
    public static void retrieveHighScore()
    {
        cachedLeaderboardScore = 0;
        if (!hasSignedIn) return;
        isRetrievingHiScore = true;

        //Score will stay 0 if this fails...
        PlayGamesPlatform.Instance.LoadScores(

           HISCORE_BOARD_ID,
           LeaderboardStart.PlayerCentered,
           1,
           LeaderboardCollection.Social,
           LeaderboardTimeSpan.AllTime,
           (LeaderboardScoreData data) => {
               if (data.PlayerScore != null)
               {
                   cachedLeaderboardScore = (int)data.PlayerScore.value;
                   hasCachedHiScore = true;
                   isRetrievingHiScore = false;
               }
           });
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class leaderboardButton : MonoBehaviour {

    Button button;
    Image crownImage;
    public GameObject loadingImage;

	public void showLeaderboard () {
        button = GetComponent<Button>();
        crownImage = GetComponent<Image>();
        StartCoroutine("leaderboard");
    }

    public IEnumerator leaderboard()
    {
        GooglePlayServiceManager.signIn();

        //Disabling button
        button.enabled = false;
        crownImage.enabled = false;
        loadingImage.SetActive(true);

        //Waiting till sign-in process is complete, then renabling the button
        yield return new WaitUntil(() => GooglePlayServiceManager.hasTriedToSignIn);
        button.enabled = true;
        crownImage.enabled = true;
        loadingImage.SetActive(false);

        GooglePlayServiceManager.showHighScoreBoard();
    }


}

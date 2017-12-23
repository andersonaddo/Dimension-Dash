using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopSoundsAndToast : MonoBehaviour {

    public AudioClip moneySound, beepSound;
    AudioSource audioSource;

    ToastMessageScript toastScript;

	// Use this for initialization
	void Start () {
        toastScript = GetComponent<ToastMessageScript>();
        audioSource = GetComponent<AudioSource>();
	}


    public void playBuySong()
    {
        audioSource.PlayOneShot(moneySound);
    }

    public void playDenialSong()
    {
        audioSource.PlayOneShot(beepSound);

#if UNITY_ANDROID && !UNITY_EDITOR
        toastScript.showToastOnUiThread(LocalizationText.GetText("shop toast"));
#endif


    }
}



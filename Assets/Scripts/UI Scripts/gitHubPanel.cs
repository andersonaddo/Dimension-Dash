using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gitHubPanel : MonoBehaviour {

    public GameObject  textPanel, backButton;
    public Image MainPanel;

    void Start()
    {
        textPanel.transform.localScale = new Vector3(0, 0, 0);
        MainPanel.enabled = false;
        textPanel.SetActive(false);
    }

    public void open()
    {
        MainPanel.enabled = true;
        textPanel.transform.localScale = new Vector3(0, 0, 0);
        textPanel.SetActive(true);
        iTween.ScaleTo(textPanel, iTween.Hash("scale", Vector3.one,
                                              "easeType", iTween.EaseType.easeOutBack,
                                              "onComplete", "enableText",
                                              "oncompletetarget", gameObject,
                                              "time", 0.7f));
    }

    public void close()
    {
        backButton.SetActive(false);
        textPanel.transform.localScale = new Vector3(1, 1, 1);
        iTween.ScaleTo(textPanel, iTween.Hash("scale", Vector3.zero,
                                              "easeType", iTween.EaseType.easeInBack,
                                              "onComplete", "finalClose",
                                              "oncompletetarget", gameObject,
                                              "time", 0.5f));
    }

    void enableText()
    {
        textPanel.SetActive(true);
        backButton.SetActive(true);
    }

    void finalClose()
    {
        MainPanel.enabled = false;
        textPanel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrollCreditsManager : MonoBehaviour {

    ScrollRect scrollRect;
    bool hasStarted;

    float waitTime;

    bool canScroll = true;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 1;
        hasStarted = true;
    }

    // Resetting the position of the rect every time is viewed afresh
    void OnEnable () {
        if (hasStarted) scrollRect.verticalNormalizedPosition = 1;
        canScroll = true;
        waitTime = Time.time + 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
        //Slowly scrolling down...
        if (canScroll && Time.time > waitTime) scrollRect.verticalNormalizedPosition = Mathf.MoveTowards(scrollRect.verticalNormalizedPosition, 0, 0.001f);

        if (scrollRect.verticalNormalizedPosition < 0.05) canScroll = false;
	}
}

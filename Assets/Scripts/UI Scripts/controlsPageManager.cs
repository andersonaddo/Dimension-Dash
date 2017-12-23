using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlsPageManager : MonoBehaviour {

    public int currentIndex = 0;

    public Text currentIndexText;
    public List<GameObject> pages = new List<GameObject>();

    public GameObject previousButton, nextButton;


	// Use this for initialization
	void Start () {
        previousButton.SetActive(false);
    }
    
    public void getPage(int newIndex)
    {
        disable(pages[currentIndex]);
        currentIndex = newIndex;
        enable(pages[currentIndex]);

        //Disabling the buttons then the user gets to the end or beginning of the controls manual
        nextButton.SetActive(currentIndex != pages.Count-1);
        previousButton.SetActive(currentIndex != 0);

        //Setting the number text accordingly (if the text exists)
        if (currentIndexText) currentIndexText.text = getIndexStr();
    }



    void disable(GameObject thing)
    {
        thing.SetActive(false);
    }

    void enable(GameObject thing)
    {
        thing.SetActive(true);
    }

    public void changeCurrentBy(int change)
    {
        getPage(currentIndex + change);
    }

    string getIndexStr()
    {
        if (currentIndex == 0) return "-";
        return "" + currentIndex;
    }
}

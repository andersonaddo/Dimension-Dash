using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class openScene : MonoBehaviour {

	public void loadScene(int index)
    {
        SceneManager.LoadScene(index);
    }


    public void loadRandomScene()
    {
        List<int> availableDimensions = globalDataPreserver.Instance.getAvailableDimensions();

        //Don't go to the Amy Scene if you're in the main menu    
        if (SceneManager.GetActiveScene().buildIndex == 0 && availableDimensions.Contains(5)) availableDimensions.Remove(5);
       
        SceneManager.LoadScene(availableDimensions[Random.Range(0, availableDimensions.Count)]);

    }

    public void reset()
    {
        globalDataPreserver.Instance.reset();
    }
}

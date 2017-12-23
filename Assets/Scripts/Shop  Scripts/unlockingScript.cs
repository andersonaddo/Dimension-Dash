using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is behind the padlock seen over locked dimensions in the character shop
/// </summary>
public class unlockingScript : MonoBehaviour {

    public GameObject lockPage, unlockPage;
    public int sceneIndex;

    void OnEnable()
    {
        //Enabling the padlock page if the user hasn't bought the associated dimension
        lockPage.SetActive(!globalDataPreserver.Instance.characterChoices.ContainsKey(sceneIndex));
        unlockPage.SetActive(!lockPage.activeSelf);
    }
}

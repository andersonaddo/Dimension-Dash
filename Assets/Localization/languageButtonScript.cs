using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class languageButtonScript : MonoBehaviour {

    public void setCurrentlanguage(string languageCode)
    {
        PlayerPrefs.SetString(globalDataPreserver.KEY_FOR_LANGUAGE, languageCode);
        LocalizationText.SetLanguage(languageCode);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class languageChanger : MonoBehaviour {

    public string key;

    TextMeshProUGUI textMeshPro;
    Text normalText;

    public Font normalFontUI, russianFontUI;
    public TMP_FontAsset normalFontTMP, russianFontTMP;

    void Awake()
    {
        normalText = GetComponent<Text>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }


	void OnEnable () {
        localize();
	}
	
	public void localize () {

        if (normalText)
        {
            normalText.text = LocalizationText.GetText(key);
            if (LocalizationText.GetLanguage().Equals("RU")) normalText.font = russianFontUI;
            else normalText.font = normalFontUI;
            return;
        }

        textMeshPro.text = LocalizationText.GetText(key);
        if (LocalizationText.GetLanguage().Equals("RU")) textMeshPro.font = russianFontTMP;
        else textMeshPro.font = normalFontTMP;
    }
}

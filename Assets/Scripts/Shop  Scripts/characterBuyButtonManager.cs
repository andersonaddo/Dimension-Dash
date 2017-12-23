using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class characterBuyButtonManager : MonoBehaviour {

    public shopSoundsAndToast effectsManager;

    //Purchace variables
    public int price;
    bool isCharacterEquipped;
    bool isCharacterBought;

    public TextMeshProUGUI priceText;
    public TextMeshProUGUI equipText;

    //Internal memory variables
    public int sceneIndex; //The scene index of the dimension this button is affiliated with
    public int characterIndex;
    bool dimensionAvailable;
    CharacterOptionsForDimensions cachedChoices;

    public GameObject buyingVisual, equippingVisual; //The two styles of the button

	// Use this for initialization
	void OnEnable () {
        setUp();
	}

    public void setUp()
    {
        //Needs to be reset every setup. Else it will stay true once made true once.
        isCharacterEquipped = false;

        //Was added to avoid NullPointerException
        //In retrospect, this isn't needed because the button would only be active if the dimension was available.
        //But there's no point in resructuring the whole code, so whatever.
        dimensionAvailable = globalDataPreserver.Instance.characterChoices.TryGetValue(sceneIndex, out cachedChoices);

        if (dimensionAvailable && cachedChoices.availableCharacterIndexes.Contains(characterIndex))
        {
            equippingVisual.SetActive(true);
            buyingVisual.SetActive(false);
            isCharacterBought = true;

            if (cachedChoices.selectedCharacter == characterIndex)
            {
                isCharacterEquipped = true;
                equipText.text = LocalizationText.GetText("equipped");
            }else
            {
                equipText.text = LocalizationText.GetText("equip");
            }

        }else
        {
            equippingVisual.SetActive(false);
            buyingVisual.SetActive(true);
            priceText.text = price.ToString();
        }
    }

    public void buyCharacterOrEquip()
    {
        if (!isCharacterBought)
        {
            buyCharacter();
            return;
        }

        if (isCharacterBought && !isCharacterEquipped)
        {
            equipCharacter();
        }
    }

    public void equipCharacter()
    {
        globalDataPreserver.Instance.updateSelectedCharacter(sceneIndex, characterIndex);
        setUp();
    }

    public void buyCharacter()
    {
        if (globalDataPreserver.Instance.coinCount >= price)
        {
            globalDataPreserver.Instance.incrementCoinCount(-price);
            globalDataPreserver.Instance.addCharacter(sceneIndex, characterIndex);
            setUp();

            effectsManager.playBuySong();
        }else
        {
            effectsManager.playDenialSong();
        }
    }
}

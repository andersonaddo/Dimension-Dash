using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class dimensionBuyButtonManager : MonoBehaviour {

    public shopSoundsAndToast effectsManager;

    //Purchace variables
    public int price;
    bool isDimensionBought;

    //Visual Button variables
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI equipText;
    public GameObject buyingVisual, equippingVisual; //The two styles of the button

    //Internal memory variables
    public int sceneIndex; //The dimension's scene index

    void OnEnable()
    {
        setUp();
    }


    public void setUp()
    {

        isDimensionBought = globalDataPreserver.Instance.characterChoices.ContainsKey(sceneIndex);

        if (isDimensionBought)
        {
            equippingVisual.SetActive(true);
            buyingVisual.SetActive(false);
            equipText.text = LocalizationText.GetText("bought");
                       
        }
        else
        {
            equippingVisual.SetActive(false);
            buyingVisual.SetActive(true);
            priceText.text = price.ToString();
        }
    }



    public void buyDimension()
    {
        if (isDimensionBought) return;

        if (globalDataPreserver.Instance.coinCount >= price)
        {
            globalDataPreserver.Instance.incrementCoinCount(-price);
            globalDataPreserver.Instance.addNewDimension(sceneIndex);
            setUp();

            effectsManager.playBuySong();
        }
        else
        {
            effectsManager.playDenialSong();
        }
    }


}

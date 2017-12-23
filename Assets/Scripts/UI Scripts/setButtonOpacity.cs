using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setButtonOpacity : MonoBehaviour {

    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        slider.value = globalDataPreserver.Instance.getButtonOpacity();
    }

	public void setOpacity()
    {
        PlayerPrefs.SetFloat(globalDataPreserver.KEY_FOR_OPACITY,  slider.value);
    }
}
